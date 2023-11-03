using System.IO;
using UnityEngine;

public class Setting
{
    /// <summary>
    /// 默认层级
    /// </summary>
    public static readonly int LAYER_DEFAULT = 0;

    /// <summary>
    /// 界面显示层级
    /// </summary>
    public static readonly int LAYER_VIEW = 5;

    /// <summary>
    /// 界面隐藏层级
    /// </summary>
    public static readonly int LAYER_HIDE = 31;

    /// <summary>
    /// 本地排行数据
    /// </summary>
    public static readonly string P_RANK_PATH = Path.Combine(Application.persistentDataPath, "rank.json");
}
