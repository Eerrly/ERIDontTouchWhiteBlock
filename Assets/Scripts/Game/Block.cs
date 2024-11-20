using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// 块
/// </summary>
public class Block : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
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
    /// 边框
    /// </summary>
    private Image frame;

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="index">当前块的索引</param>
    /// <param name="blockRaw">当前行</param>
    public void OnInitialize(int index, BlockRawView blockRaw)
    {
        img = GetComponent<Image>();
        frame = transform.GetChild(0).GetComponent<Image>();
        frame.color = GameConstant.ClickSurefireColor;
        this.index = index;
        this.blockRaw = blockRaw;
    }

    /// <summary>
    /// 点击按下
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerDown(PointerEventData eventData)
    {
        this.blockRaw.clickIndex = this.index;
        this.blockRaw.isTriggerPointDown = true;
        this.blockRaw.blockClickEvent?.Invoke(this);
    }

    /// <summary>
    /// 点击抬起
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerUp(PointerEventData eventData)
    {
        this.blockRaw.isTriggerPointUp = true;
    }

    /// <summary>
    /// 设置图片颜色
    /// </summary>
    /// <param name="color">颜色</param>
    public void InitBlockInfo(Color imgColor, Vector3 frameScale)
    {
        img.color = imgColor;
        frame.transform.localScale = frameScale;
        frame.gameObject.SetActive(false);
    }

    /// <summary>
    /// 颜色渐变
    /// </summary>
    /// <param name="color"></param>
    /// <param name="duration"></param>
    public void SetBlockInfo(Color color, float colorDuration, float scaleDuration)
    {
        frame.gameObject.SetActive(true);
        img.DOColor(color, colorDuration).SetEase(Ease.InSine);
        frame.transform.DOScale(Vector3.one, scaleDuration).SetEase(Ease.InSine).OnComplete(() =>
        {
            frame.gameObject.SetActive(false);
        });
    }

    /// <summary>
    /// 获取结果
    /// </summary>
    /// <returns></returns>
    public byte GetBlockViewResult()
    {
        return blockRaw.result;
    }

}
