using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 滚动列表视窗
/// </summary>
public class BlockScrollViewPort : MonoBehaviour, IPointerExitHandler
{
    /// <summary>
    /// 按钮退出
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerExit(PointerEventData eventData)
    {
        BlockScrollManager.Instance.IsPointerDown = false;
    }
}
