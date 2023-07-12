public class BaseView
{

    protected int _viewId;

    public virtual void Reset() { }

    public virtual bool TryEnter() { return false; }

    public virtual void OnEnter() { }

    public virtual void OnUpdate(float deltaTime, float unscaleDeltaTime) { }

    public virtual bool TryExit() { return true; }

    public virtual void OnExit() { }

}
