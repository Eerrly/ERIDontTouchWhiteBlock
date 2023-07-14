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
    VerticalLayoutGroup layoutGroup;
    Button passBtn;
    Button playBtn;

    float topY;
    float height;

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

        layoutGroup = viewGo.transform.Find("BlockScrollView/BlockScrollViewPort").GetComponent<VerticalLayoutGroup>();
        layoutGroup.enabled = true;

        blockRawList = new List<BlockRawView>(viewGo.GetComponentsInChildren<BlockRawView>(true));
        topY = blockRawList[0].transform.localPosition.y;
        height = blockRawList[0].transform.GetComponent<RectTransform>().sizeDelta.y;

        for (int i = 0; i < blockRawList.Count; i++)
        {
            // 初始化，最后几行不用点
            blockRawList[i].result = i < 2 ? BlockScrollManager.Instance.GetRandomResult() : default(byte);
            blockRawList[i].OnInitialize(blockClickEvent);
            oneLoopList.AddLast(new OneLoopNode<BlockRawView>() { item = blockRawList[i] });
        }
        viewHead = oneLoopList.Head;

        layoutGroup.enabled = false;
    }

    public override void OnUpdate(float deltaTime, float unscaleDeltaTime)
    {
        if(layoutGroup.enabled)
        {
            return;
        }
        for (int i = 0; i < blockRawList.Count; i++)
        {
            blockRawList[i].transform.localPosition += Vector3.down * Time.deltaTime * BlockScrollManager.Instance.scrollSpeed;
        }
        OneLoopNode<BlockRawView> nextNode = viewHead.next.next.next.next;
        OneLoopNode<BlockRawView> newLastNode = viewHead.next.next.next;
        if (viewHead.item.transform.localPosition.y <= topY)
        {
            nextNode.item.transform.localPosition = viewHead.item.transform.localPosition + Vector3.up * height;
            if (nextNode.item.result == default(byte))
            {
                nextNode.item.result = BlockScrollManager.Instance.GetRandomResult();
                nextNode.item.RefreshBlockList();
            }
        }
        if (viewHead.item.transform.localPosition.y <= topY - height)
        {
            viewHead = nextNode;
            newLastNode.item.result = default(byte);
        }

        BlockScrollManager.Instance.scrollSpeed += deltaTime;
        BlockScrollManager.Instance.gameTimeStamp += deltaTime;
        Time.timeScale = Global.Instance.timeScale;

        timeTxt.text = $"时间 {Util.FormatTimeStamp2HMS((int)BlockScrollManager.Instance.gameTimeStamp)}";
        scoreTxt.text = $"分数 {BlockScrollManager.Instance.gameScore}";
    }

    /// <summary>
    /// 块点击的具体逻辑
    /// </summary>
    /// <param name="clickBlockIndex"></param>
    /// <param name="block"></param>
    void OnBlockClickAction(byte clickBlockIndex, Block block)
    {
        if (Global.Instance.timeScale == 0)
            return;
        if (GameConstant.BlockResults[clickBlockIndex] == block.blockRaw.result)
        {
            BlockScrollManager.Instance.gameScore++;
            block.SetImageColor(Color.gray);
        }
        else
        {
            block.SetImageColor(Color.red);
            ViewManager.Instance.ChangeView((int)EView.GameOver);
        }
    }

    private void OnPassButtonClicked()
    {
        Global.Instance.timeScale = 0;
        passBtn.gameObject.SetActive(false);
        playBtn.gameObject.SetActive(true);
        AudioManager.Instance.PauseAudio();
    }

    private void OnPlayButtonClicked()
    {
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
        layoutGroup.enabled = true;
    }

}
