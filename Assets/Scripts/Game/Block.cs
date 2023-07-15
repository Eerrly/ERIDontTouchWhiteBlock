using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// 块
/// </summary>
public class Block : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerUpHandler
{
    /// <summary>
    /// 当前行
    /// </summary>
    public BlockRawView blockRaw;

    /// <summary>
    /// 块的点击事件
    /// </summary>
    private BlockClickEvent blockClickEvent;

    /// <summary>
    /// 当前块的索引
    /// </summary>
    private int index;

    /// <summary>
    /// 当前块的图片
    /// </summary>
    private Image img;

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="index">当前块的索引</param>
    /// <param name="blockClickEvent">块的点击事件</param>
    /// <param name="blockRaw">当前行</param>
    public void OnInitialize(int index, BlockClickEvent blockClickEvent, BlockRawView blockRaw)
    {
        img = GetComponent<Image>();
        this.index = index;
        this.blockClickEvent = blockClickEvent;
        this.blockRaw = blockRaw;
    }

    /// <summary>
    /// 按钮按下
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerDown(PointerEventData eventData)
    {
        if (!BlockScrollManager.Instance.IsPointerDown)
        {
            BlockScrollManager.Instance.IsPointerDown = true;
            this.blockRaw.isTriggerPointDown = true;
            blockClickEvent?.Invoke((byte)index, this);
        }
    }

    /// <summary>
    /// 按钮进入
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (BlockScrollManager.Instance.IsPointerDown)
        {
            blockClickEvent?.Invoke((byte)index, this);
        }
    }

    /// <summary>
    /// 按钮抬起
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerUp(PointerEventData eventData)
    {
        BlockScrollManager.Instance.IsPointerDown = false;
    }

    /// <summary>
    /// 设置图片颜色
    /// </summary>
    /// <param name="color">颜色</param>
    public void SetImageColor(Color color)
    {
        img.color = color;
    }

}
