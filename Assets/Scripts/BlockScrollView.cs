using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 滚动视窗类
/// </summary>
public class BlockScrollView : MonoBehaviour
{
    /// <summary>
    /// 行的列表
    /// </summary>
    public List<BlockRawView> blockRawList = new List<BlockRawView>();

    /// <summary>
    /// 最上面行的Y
    /// </summary>
    float topY;

    /// <summary>
    /// 块的高
    /// </summary>
    float height;

    /// <summary>
    /// 视窗上的最上面那行
    /// </summary>
    OneLoopNode<BlockRawView> viewHead;

    /// <summary>
    /// 行的单向循环列表
    /// </summary>
    OneLoopList<BlockRawView> oneLoopList;

    private void Awake()
    {
        oneLoopList = new OneLoopList<BlockRawView>();
    }

    private void Start()
    {
        topY = blockRawList[0].transform.localPosition.y;
        height = blockRawList[0].transform.GetComponent<RectTransform>().sizeDelta.y;
        for (int i = 0; i < blockRawList.Count; i++)
        {
            if (i < blockRawList.Count - 1)
            {
                blockRawList[i].result = BlockScrollManager.Instance.GetRandomResult();
            }
            blockRawList[i].OnInitialize(BlockScrollManager.Instance.blockClickEvent);
            oneLoopList.AddLast(new OneLoopNode<BlockRawView>() { item = blockRawList[i] });
        }
        viewHead = oneLoopList.Head;
    }

    private void Update()
    {
        for (int i = 0; i < blockRawList.Count; i++)
        {
            blockRawList[i].transform.localPosition += Vector3.down * Time.deltaTime * BlockScrollManager.Instance.scrollSpeed;
        }
        OneLoopNode<BlockRawView> nextNode = viewHead.next.next.next;
        OneLoopNode<BlockRawView> newLastNode = viewHead.next.next;
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

        BlockScrollManager.Instance.scrollSpeed += Time.deltaTime;
        BlockScrollManager.Instance.gameTimeStamp += Time.deltaTime;
        Time.timeScale = BlockScrollManager.Instance.timeScale;
    }

}
