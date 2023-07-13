using UnityEngine;

/// <summary>
/// 当前行
/// </summary>
public class BlockRawView : MonoBehaviour
{
    /// <summary>
    /// 块的集合
    /// </summary>
    public Block[] blockList;

    /// <summary>
    /// 当前行块所处的结果
    /// </summary>
    public byte result;

    /// <summary>
    /// 块点击事件
    /// </summary>
    private BlockClickEvent blockClickEvent;

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="blockClickEvent">块点击事件</param>
    public void OnInitialize(BlockClickEvent blockClickEvent)
    {
        this.blockClickEvent = blockClickEvent;
        RefreshBlockList();
    }

    /// <summary>
    /// 刷新当前行包含的块
    /// </summary>
    public void RefreshBlockList()
    {
        for (int i = 0; i < blockList.Length; i++)
        {
            byte tmpIndex = (byte)i;

            blockList[i].OnInitialize(tmpIndex, blockClickEvent, this);
            blockList[i].SetImageColor((result == default(byte)) || (result & (1 << (blockList.Length - i - 1))) == 0 ? Color.white : Color.black);
        }
    }

}
