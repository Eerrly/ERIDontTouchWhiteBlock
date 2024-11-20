using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 滚动视窗类
/// </summary>
[View(EView.Game)]
public class GameView : View
{
    Text timeTxt;
    Text scoreTxt;
    Button passBtn;
    Button playBtn;

    int passedBlockCount;
    float singleBlockHeight;
    Rect rectViewport;

    /// <summary>
    /// 行的列表
    /// </summary>
    List<BlockRawView> blockRawList = new List<BlockRawView>();
    /// <summary>
    /// 视窗上的最上面那行
    /// </summary>
    OneLoopNode<BlockRawView> viewHead;
    /// <summary>
    /// 行的单向循环列表
    /// </summary>
    OneLoopList<BlockRawView> oneLoopList;
    /// <summary>
    /// 上一次生成的结果
    /// </summary>
    private byte lastGeneratedResult;
    /// <summary>
    /// 重复生成同一个结果的次数
    /// </summary>
    private int RepetitionsNum;

    BlockClickEvent blockClickEvent = new BlockClickEvent();

    public override bool TryEnter()
    {
        viewGo = ViewManager.Instance.CreateView((int)EView.Game);
        return viewGo != null;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        BlockScrollManager.Instance.scrollSpeed = GameConstant.InitScrollSpeed[(int)BlockScrollManager.Instance.problemLevel];
        BlockScrollManager.Instance.faultToleranceTime = GameConstant.InitFaultToleranceTime[(int)BlockScrollManager.Instance.problemLevel];

        passedBlockCount = 0;
        Global.Instance.isGameover = false;

        oneLoopList = new OneLoopList<BlockRawView>();
        blockClickEvent.AddListener(OnBlockClickAction);

        timeTxt = viewGo.transform.Find("Text_Time").GetComponent<Text>();
        scoreTxt = viewGo.transform.Find("Text_Score").GetComponent<Text>();

        passBtn = viewGo.transform.Find("Button_Pass").GetComponent<Button>();
        playBtn = viewGo.transform.Find("Button_Play").GetComponent<Button>();
        passBtn.gameObject.SetActive(true);
        playBtn.gameObject.SetActive(false);
        passBtn.onClick.AddListener(OnPassButtonClicked);
        playBtn.onClick.AddListener(OnPlayButtonClicked);

        blockRawList = new List<BlockRawView>(viewGo.GetComponentsInChildren<BlockRawView>(true));
        rectViewport = viewGo.transform.Find("BlockScrollView").GetComponent<RectTransform>().rect;
        singleBlockHeight = rectViewport.height / (blockRawList.Count - 1);

        InitBlockViewItems();
    }

    public override void OnUpdate(float deltaTime, float unscaleDeltaTime)
    {
        if(Global.Instance.isGameover)
        {
            return;
        }
        for (int i = 0; i < blockRawList.Count; i++)
        {
            blockRawList[i].transform.localPosition += Vector3.down * deltaTime * BlockScrollManager.Instance.scrollSpeed;
        }
        OneLoopNode<BlockRawView> nextNode = viewHead.next.next.next.next;
        OneLoopNode<BlockRawView> newLastNode = viewHead.next.next.next;
        if (viewHead.item.transform.localPosition.y <= rectViewport.height)
        {
            nextNode.item.transform.localPosition = viewHead.item.transform.localPosition + Vector3.up * singleBlockHeight;
            if (nextNode.item.result == default(byte) && passedBlockCount > 1)
            {
                // 不会出现连续4个一列
                var result = BlockScrollManager.Instance.GetRandomResult();
                if (result == lastGeneratedResult)
                    RepetitionsNum++;
                else
                    RepetitionsNum = 0;
                if (RepetitionsNum > GameConstant.RepetitionsAllowedNum)
                {
                    while (result == lastGeneratedResult)
                        result = BlockScrollManager.Instance.GetRandomResult();
                    RepetitionsNum = 0;
                }
                lastGeneratedResult = result;
                nextNode.item.result = result;
                nextNode.item.RefreshBlockList();
            }
        }
        if (viewHead.item.transform.localPosition.y <= rectViewport.height - singleBlockHeight)
        {
            var item = newLastNode.item;
            // 漏了没点
            if (item.result != default(byte) && (!item.isTriggerPointDown || (item.isTriggerPointDown && item.result != GameConstant.BlockResults[item.clickIndex])))
            {
                ViewManager.Instance.ChangeView((int)EView.GameOver);
                return;
            }
            viewHead = nextNode;
            newLastNode.item.result = default(byte);
            passedBlockCount++;
        }

        BlockScrollManager.Instance.scrollSpeed += deltaTime;
        BlockScrollManager.Instance.gameTimeStamp += deltaTime;
        Time.timeScale = Global.Instance.timeScale;

        timeTxt.text = $"时间 {Util.FormatTimeStamp2HMS((int)BlockScrollManager.Instance.gameTimeStamp)}";
        scoreTxt.text = $"分数 {BlockScrollManager.Instance.gameScore}";
    }

    /// <summary>
    /// 初始化每一个块的数据与位置
    /// </summary>
    private void InitBlockViewItems()
    {
        var anchordPosition = Vector2.zero;
        var sizeDelta = Vector2.zero;
        for (int i = 0; i < blockRawList.Count; i++)
        {
            anchordPosition.x = 0;
            anchordPosition.y = -i * singleBlockHeight;
            blockRawList[i].GetComponent<RectTransform>().anchoredPosition = anchordPosition;
            sizeDelta.x = rectViewport.width;
            sizeDelta.y = singleBlockHeight;
            blockRawList[i].GetComponent<RectTransform>().sizeDelta = sizeDelta;
            blockRawList[i].result = default(byte);
            blockRawList[i].OnInitialize(blockClickEvent);
            oneLoopList.AddLast(new OneLoopNode<BlockRawView>() { item = blockRawList[i] });
        }
        viewHead = oneLoopList.Head;
    }

    /// <summary>
    /// 块点击的具体逻辑
    /// </summary>
    /// <param name="clickBlockIndex"></param>
    /// <param name="block"></param>
    void OnBlockClickAction(Block block)
    {
        if (Global.Instance.timeScale == 0 || Global.Instance.isGameover)
            return;
        if (GameConstant.BlockResults[block.blockRaw.clickIndex] == block.GetBlockViewResult())
        {
            BlockScrollManager.Instance.gameScore++;
            block.SetBlockInfo(GameConstant.ClickSurefireColor, GameConstant.TweenColorDuration, GameConstant.TweenScaleDuration);
            return;
        }
        // 点到白的了
        MonoManager.Instance.StartCoroutine(OnClickWhiteBlock(block));
    }

    /// <summary>
    /// 点到白块逻辑
    /// </summary>
    /// <returns></returns>
    private IEnumerator OnClickWhiteBlock(Block block)
    {
        yield return new WaitForSeconds(BlockScrollManager.Instance.faultToleranceTime);
        // 如果在容错时间内抬起，则不算它点到白块
        if (block.blockRaw.isTriggerPointUp) yield break;
        // 否则game-over
        Global.Instance.isGameover = true;
        block.InitBlockInfo(GameConstant.ClickFallaciousColor, GameConstant.DefaultSurefireFrameScale);
        yield return new WaitForSeconds(1.0f);
        ViewManager.Instance.ChangeView((int)EView.GameOver);
    }

    /// <summary>
    /// 暂停键按钮点击事件
    /// </summary>
    private void OnPassButtonClicked()
    {
        if(Global.Instance.isGameover) { return; }
        Global.Instance.timeScale = 0;
        passBtn.gameObject.SetActive(false);
        playBtn.gameObject.SetActive(true);
        AudioManager.Instance.PauseAudio();
    }

    /// <summary>
    /// 继续键按钮点击事件
    /// </summary>
    private void OnPlayButtonClicked()
    {
        if (Global.Instance.isGameover) { return; }
        Global.Instance.timeScale = 1;
        passBtn.gameObject.SetActive(true);
        playBtn.gameObject.SetActive(false);
        AudioManager.Instance.UnPauseAudio();
    }

    public override void OnExit()
    {
        base.OnExit();
        blockRawList.Clear();
        blockClickEvent.RemoveListener(OnBlockClickAction);
    }

}
