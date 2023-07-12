using UnityEngine;
using UnityEngine.UIElements;

[View(EView.None)]
public class View : BaseView
{

    public EView ViewId
    {
        get { return (EView)_viewId; }
        set { _viewId = (int)value; }
    }

    public GameObject viewGo;

    public override void Reset()
    {
        ViewManager.Instance.currViewId = (int)ViewId;
        ViewManager.Instance.nextViewId = (int)EView.None;

        Canvas canvas = viewGo.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.worldCamera = ViewManager.Instance.UICamera;
        canvas.pixelPerfect = false;
        canvas.sortingOrder = (int)ViewId * 10;
        canvas.planeDistance = 8000 - canvas.sortingOrder;
        if (canvas.planeDistance <= 0)
        {
            canvas.planeDistance = 0;
        }
    }

    public override bool TryEnter() { return false; }

    public override void OnEnter() {
        Util.SetGameObjectLayer(viewGo, Setting.LAYER_UI, true);
    }

    public override void OnUpdate(float deltaTime, float unscaleDeltaTime) { }

    public override bool TryExit() { return true; }

    public override void OnExit() {
        Util.SetGameObjectLayer(viewGo, Setting.LAYER_HIDE, true);
    }

}
