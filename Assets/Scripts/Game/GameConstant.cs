/// <summary>
/// 游戏静态数据类
/// </summary>
public class GameConstant
{
    /// <summary>
    /// 所有的结果
    /// 1000 -> 8
    /// 0100 -> 4
    /// 0010 -> 2
    /// 0001 -> 1
    /// </summary>
    public static readonly byte[] BlockResults = new byte[] { 8, 4, 2, 1 };

    /// <summary>
    /// 初始滚动速度
    /// </summary>
    public static readonly int InitScrollSpeed = 1000;

    /// <summary>
    /// 本地排行容量
    /// </summary>
    public static readonly int MaxRankInfoCount = 7;


}
