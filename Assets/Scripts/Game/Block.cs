using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// 块
/// </summary>
public class Block : MonoBehaviour, IPointerDownHandler
{
    /// <summary>
    /// 当前行
    /// </summary>
    [System.NonSerialized] public BlockRawView blockRaw;

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
    public void OnInitialize(int index, BlockRawView blockRaw)
    {
        img = GetComponent<Image>();
        //btn = GetComponent<Button>();
        this.index = index;
        this.blockRaw = blockRaw;

    }

    /// <summary>
    /// 点击按下
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerDown(PointerEventData eventData)
    {
        this.blockRaw.isTriggerPointDown = true;
        this.blockRaw.blockClickEvent?.Invoke((byte)this.index, this);
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
