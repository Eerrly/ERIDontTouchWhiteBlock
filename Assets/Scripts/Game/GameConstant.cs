using UnityEngine;

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
    /// 点击容错时间
    /// </summary>
    public static readonly float[] InitFaultToleranceTime = new float[] { 0.08f, 0.05f, 0.02f };

    /// <summary>
    /// 本地排行容量
    /// </summary>
    public static readonly int MaxRankInfoCount = 7;

    /// <summary>
    /// 允许重复生成同一个结果的次数
    /// </summary>
    public static readonly int RepetitionsAllowedNum = 2;

    /// <summary>
    /// 点击后颜色渐变的耗时
    /// </summary>
    public static readonly float TweenColorDuration = 0.2f;

    /// <summary>
    /// 点击后边框渐变的耗时
    /// </summary>
    public static readonly float TweenScaleDuration = 0.5f;

    /// <summary>
    /// 白块颜色
    /// </summary>
    public static readonly Color BlockWhiteColor = Color.white;

    /// <summary>
    /// 黑块颜色
    /// </summary>
    public static readonly Color BlockBlackColor = Color.black;

    /// <summary>
    /// 点击正确的颜色
    /// </summary>
    public static readonly Color ClickSurefireColor = Color.gray;

    /// <summary>
    /// 点击错误的颜色
    /// </summary>
    public static readonly Color ClickFallaciousColor = Color.red;

    /// <summary>
    /// 默认的块的边框的缩放
    /// </summary>
    public static readonly Vector3 DefaultSurefireFrameScale = new Vector3(1.3f, 1.3f, 0);

}
