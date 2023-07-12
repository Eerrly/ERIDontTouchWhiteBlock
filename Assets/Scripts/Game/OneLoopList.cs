/// <summary>
/// 单向循环列表
/// </summary>
/// <typeparam name="T">节点类</typeparam>
public class OneLoopList<T>
{
    private int _count = 0;

    /// <summary>
    /// 节点数量
    /// </summary>
    public int Count => _count;

    /// <summary>
    /// 头指针
    /// </summary>
    public OneLoopNode<T> Head = null;

    /// <summary>
    /// 添加节点到末尾
    /// </summary>
    /// <param name="node">节点</param>
    public void AddLast(OneLoopNode<T> node)
    {
        if (_count == 0)
        {
            Head = node;
            Head.next = Head;
        }
        else
        {
            OneLoopNode<T> tmpNode = Head;
            do
            {
                tmpNode = tmpNode.next;
            } while (tmpNode.next != Head);
            node.next = tmpNode.next;
            tmpNode.next = node;
        }
        _count++;
    }

    /// <summary>
    /// 从末尾移除节点
    /// </summary>
    public void RemoveLast()
    {
        if (_count == 0)
            return;
        if (_count == 1)
        {
            Head = null;
        }
        else
        {
            OneLoopNode<T> tmpNode = Head;
            do
            {
                tmpNode = tmpNode.next.next;
            } while (tmpNode.next.next != Head);
            tmpNode.next.next = null;
            tmpNode.next = Head;
        }
        _count--;
    }

    /// <summary>
    /// 获取节点
    /// </summary>
    /// <param name="index">索引</param>
    /// <returns>节点类实例</returns>
    public T GetNode(int index)
    {
        if (_count == 0 || index < 0 || index > _count)
            return default(T);
        int tmpIndex = 0;
        OneLoopNode<T> tmpNode = Head;
        do
        {
            tmpNode = tmpNode.next;
        } while (++tmpIndex < index);
        return tmpNode.item;
    }

    /// <summary>
    /// 设置节点
    /// </summary>
    /// <param name="index">索引</param>
    /// <param name="item">节点类实例</param>
    public void SetNode(int index, T item)
    {
        if (_count == 0 || index < 0 || index > _count)
            return;
        int tmpIndex = 0;
        OneLoopNode<T> tmpNode = Head;
        do
        {
            tmpNode = tmpNode.next;
        } while (++tmpIndex < index);
        tmpNode.item = item;
    }

}