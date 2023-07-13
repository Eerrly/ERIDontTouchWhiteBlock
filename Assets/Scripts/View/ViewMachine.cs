public class ViewMachine : BaseMachine
{

    private static ViewMachine instance;

    public static ViewMachine Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new ViewMachine();
            }
            return instance;
        }
    }

    private ViewMachine():base()
    {
        var types = GetType().Assembly.GetExportedTypes();
        _viewDic = new BaseView[(int)EView.Count];
        for (var i = 0; i < types.Length; ++i)
        {
            if (types[i].IsDefined(typeof(ViewAttribute), false))
            {
                var view = System.Activator.CreateInstance(types[i]) as View;
                var attributes = types[i].GetCustomAttributes(false);
                for (var j = 0; j < attributes.Length; ++j)
                {
                    if (attributes[j] is ViewAttribute)
                    {
                        var attr = (attributes[j] as ViewAttribute);
                        view.ViewId = attr._view;
                    }
                }

                var viewId = (int)view.ViewId;

#if UNITY_EDITOR
                if (null != _viewDic[viewId])
                {
                    UnityEngine.Debug.LogErrorFormat("The {0} state has a instance, please check. now {1} other {2}", view.ViewId, types[i], _viewDic[viewId].GetType());
                }
                else
#endif
                {
                    _viewDic[viewId] = view;
                }

            }
        }
    }

    public override void Update(float deltaTime, float unscaleDeltaTime)
    {
        var currView = _viewDic[ViewManager.Instance.currViewId];
        currView.OnUpdate(deltaTime, unscaleDeltaTime);
    }

    public bool DoChangeView()
    {
        var nextViewId = ViewManager.Instance.nextViewId;
        var nextView = _viewDic[nextViewId] as View;
        if (nextViewId != 0 && nextView != null && nextView.TryEnter())
        {
            var currViewId = ViewManager.Instance.currViewId;
            var currView = _viewDic[currViewId] as View;
            if (currViewId != 0 && currView != null && currView.TryExit())
            {
                currView.OnExit();
            }
            ViewManager.Instance.nextViewId = (int)EView.None;
            nextView.Reset();
            nextView.OnEnter();
            if (ViewManager.Instance.nextViewId != (int)EView.None)
            {
                return DoChangeView();
            }
            return true;
        }
        return false;
    }

}
