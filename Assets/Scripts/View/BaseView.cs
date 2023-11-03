public class BaseView
{

    protected int _viewId;

    /// <summary>
    /// 重置数据
    /// </summary>
    public virtual void Reset() { }

    /// <summary>
    /// 尝试进入
    /// </summary>
    /// <returns>是否可以进入当前界面</returns>
    public virtual bool TryEnter() { return false; }

    /// <summary>
    /// 进入当前界面
    /// </summary>
    public virtual void OnEnter() { }

    /// <summary>
    /// 界面轮询
    /// </summary>
    /// <param name="deltaTime"></param>
    /// <param name="unscaleDeltaTime"></param>
    public virtual void OnUpdate(float deltaTime, float unscaleDeltaTime) { }

    /// <summary>
    /// 尝试退出
    /// </summary>
    /// <returns></returns>
    public virtual bool TryExit() { return true; }

    /// <summary>
    /// 推出界面
    /// </summary>
    public virtual void OnExit() { }

}
