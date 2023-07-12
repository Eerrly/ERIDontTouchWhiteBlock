/// <summary>
/// 单向循环列表节点
/// </summary>
/// <typeparam name="T">节点类</typeparam>
public class OneLoopNode<T>
{
    /// <summary>
    /// 节点类实例
    /// </summary>
    public T item;

    /// <summary>
    /// Next指针
    /// </summary>
    public OneLoopNode<T> next;
}
