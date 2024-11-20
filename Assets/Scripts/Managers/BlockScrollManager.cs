using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 难度等级
/// </summary>
public enum ProblemLevel
{
    Easy = 0,
    Medium = 1,
    Challeng = 2,
}

/// <summary>
/// 别踩白块游戏核心管理器
/// </summary>
public class BlockScrollManager : SingletonMono<BlockScrollManager>, IManager
{
    /// <summary>
    /// 滚动速度
    /// </summary>
    [System.NonSerialized] public float scrollSpeed = 0;
    /// <summary>
    /// 点击白块的容错时间
    /// </summary>
    [System.NonSerialized] public float faultToleranceTime = 0;
    /// <summary>
    /// 游戏总时长时间戳
    /// </summary>
    [System.NonSerialized] public float gameTimeStamp = 0;
    /// <summary>
    /// 游戏得分
    /// </summary>
    [System.NonSerialized] public int gameScore = 0;
    /// <summary>
    /// 难度等级
    /// </summary>
    [System.NonSerialized] public ProblemLevel problemLevel = ProblemLevel.Easy;

    public bool IsInitialized { get; set; }

    private System.Random random;

    /// <summary>
    /// 获取一个数组中的任意一个
    /// </summary>
    /// <returns></returns>
    public byte GetRandomResult()
    {
        return GameConstant.BlockResults[random.Next(GameConstant.BlockResults.Length)];
    }

    public void OnInitialize()
    {
        random = new System.Random();
        problemLevel = (ProblemLevel)PlayerPrefs.GetInt("Setting_ProblemLevel", 0);
        IsInitialized = true;
    }

    public void OnRelease()
    {
    }
}
