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
    public static readonly byte[] BlockResults = new byte[] { 1<<3, 1<<2, 1<<1, 1<<0 };

    /// <summary>
    /// 滚动速度难度数组
    /// </summary>
    public static readonly int[] InitScrollSpeed = new int[] { 2000, 2500, 3200 };

    /// <summary>
    /// 本地排行容量
    /// </summary>
    public static readonly int MaxRankInfoCount = 7;


}
