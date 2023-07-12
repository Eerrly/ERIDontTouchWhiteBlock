public class BaseMachine
{
    protected BaseView[] _viewDic;

    public BaseMachine() { }

    public virtual BaseView GetView(int viewId)
    {
        return _viewDic[viewId];
    }

    public virtual void Update(float deltaTime, float unscaleDeltaTime) { }
}
