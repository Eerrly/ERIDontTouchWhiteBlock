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

    float topY;
    float height;
    int score = 0;

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
        Debug.Log("GameView OnEnter!");
        base.OnEnter();
        oneLoopList = new OneLoopList<BlockRawView>();
        blockClickEvent.AddListener(OnBlockClickAction);
        blockRawList = new List<BlockRawView>(viewGo.GetComponentsInChildren<BlockRawView>(true));
        timeTxt = viewGo.transform.Find("Text_Time").GetComponent<Text>();
        scoreTxt = viewGo.transform.Find("Text_Score").GetComponent<Text>();

        topY = blockRawList[0].transform.localPosition.y;
        height = blockRawList[0].transform.GetComponent<RectTransform>().sizeDelta.y;
        for (int i = 0; i < blockRawList.Count; i++)
        {
            if (i < blockRawList.Count - 1)
            {
                blockRawList[i].result = BlockScrollManager.Instance.GetRandomResult();
            }
            blockRawList[i].OnInitialize(blockClickEvent);
            oneLoopList.AddLast(new OneLoopNode<BlockRawView>() { item = blockRawList[i] });
        }
        viewHead = oneLoopList.Head;
    }

    public override void OnUpdate(float deltaTime, float unscaleDeltaTime)
    {
        Debug.Log("GameView OnUpdate!");
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

        timeTxt.text = $"Time:{BlockScrollManager.Instance.gameTimeStamp}";
        scoreTxt.text = $"Score:{score}";
    }

    /// <summary>
    /// 块点击的具体逻辑
    /// </summary>
    /// <param name="clickBlockIndex"></param>
    /// <param name="block"></param>
    void OnBlockClickAction(byte clickBlockIndex, Block block)
    {
        if (GameConstant.BlockResults[clickBlockIndex] == block.blockRaw.result)
        {
            score++;
            block.SetImageColor(Color.gray);
        }
        else
        {
            block.SetImageColor(Color.red);
        }
    }

    public override void OnExit()
    {
        base.OnExit();
        blockRawList.Clear();
        blockClickEvent.RemoveListener(OnBlockClickAction);
    }

}
