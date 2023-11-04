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
                nextNode.item.result = BlockScrollManager.Instance.GetRandomResult();
                nextNode.item.RefreshBlockList();
            }
        }
        if (viewHead.item.transform.localPosition.y <= rectViewport.height - singleBlockHeight)
        {
            // 漏了没点
            if (newLastNode.item.result != default(byte) && !newLastNode.item.isTriggerPointDown)
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
    void OnBlockClickAction(byte clickBlockIndex, Block block)
    {
        if (Global.Instance.timeScale == 0 || Global.Instance.isGameover)
            return;
        if (GameConstant.BlockResults[clickBlockIndex] == block.blockRaw.result)
        {
            BlockScrollManager.Instance.gameScore++;
            block.SetImageColor(Color.gray);
        }
        // 点到白的了
        else
        {
            block.SetImageColor(Color.red);
            MonoManager.Instance.StartCoroutine(CoDoGameOver());
        }
    }

    /// <summary>
    /// 游戏结束
    /// </summary>
    /// <returns></returns>
    private IEnumerator CoDoGameOver()
    {
        Global.Instance.isGameover = true;
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
