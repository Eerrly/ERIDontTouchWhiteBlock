using UnityEngine;
using UnityEngine.UI;

[View(EView.None)]
public class View : BaseView
{

    public EView ViewId
    {
        get { return (EView)_viewId; }
        set { _viewId = (int)value; }
    }

    public GameObject viewGo;
    public GraphicRaycaster raycaster;

    public override void Reset()
    {
        ViewManager.Instance.currViewId = (int)ViewId;
        ViewManager.Instance.nextViewId = (int)EView.None;

        Canvas canvas = viewGo.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.worldCamera = ViewManager.Instance.ViewCamera;
        canvas.pixelPerfect = false;
        canvas.sortingOrder = (int)ViewId * 10;
        canvas.planeDistance = 8000 - canvas.sortingOrder;
        if (canvas.planeDistance <= 0)
        {
            canvas.planeDistance = 0;
        }

        raycaster = viewGo.GetComponent<GraphicRaycaster>();
    }

    public override bool TryEnter() { return false; }

    public override void OnEnter() {
        Util.SetGameObjectLayer(viewGo, Setting.LAYER_VIEW, true);
        raycaster.enabled = true;
    }

    public override void OnUpdate(float deltaTime, float unscaleDeltaTime) { }

    public override bool TryExit() { return true; }

    public override void OnExit() {
        Util.SetGameObjectLayer(viewGo, Setting.LAYER_HIDE, true);
        raycaster.enabled = false;
    }

}
