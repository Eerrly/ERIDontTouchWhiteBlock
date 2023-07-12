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
        var curState = _viewDic[ViewManager.Instance.currViewId];
        curState.OnUpdate(deltaTime, unscaleDeltaTime);
    }

    public bool DoChangeView()
    {
        var nextId = ViewManager.Instance.nextViewId;
        var nextState = _viewDic[nextId] as View;
        if (nextId != 0 && nextState != null && nextState.TryEnter())
        {
            var currId = ViewManager.Instance.currViewId;
            var currState = _viewDic[currId] as View;
            if (currId != 0 && currState != null && currState.TryExit())
            {
                currState.OnExit();
            }
            ViewManager.Instance.nextViewId = (int)EView.None;
            nextState.Reset();
            nextState.OnEnter();
            if (ViewManager.Instance.nextViewId != (int)EView.None)
            {
                return DoChangeView();
            }
            return true;
        }
        return false;
    }

}
