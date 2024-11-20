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
    /// 当前行按下的索引
    /// </summary>
    public int clickIndex;

    /// <summary>
    /// 当前行触发过点击
    /// </summary>
    public bool isTriggerPointDown;

    /// <summary>
    /// 当前行触发点击抬起
    /// </summary>
    public bool isTriggerPointUp;

    /// <summary>
    /// 块点击事件
    /// </summary>
    public BlockClickEvent blockClickEvent;

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
        this.isTriggerPointDown = false;
        this.isTriggerPointUp = false;
        for (int i = 0; i < blockList.Length; i++)
        {
            byte tmpIndex = (byte)i;

            blockList[i].OnInitialize(tmpIndex, this);
            var color = result == default(byte) || (result & (1 << (blockList.Length - i - 1))) == 0 ? GameConstant.BlockWhiteColor : GameConstant.BlockBlackColor;
            blockList[i].InitBlockInfo(color, GameConstant.DefaultSurefireFrameScale);
        }
    }

}
