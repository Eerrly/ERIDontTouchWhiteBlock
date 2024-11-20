using System.Collections;
using System.Collections.Generic;
using SonicBloom.Koreo;
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
    private float currentKoreographyEventValue;

    /// <summary>
    /// 获取一个数组中的任意一个
    /// </summary>
    /// <returns></returns>
    public byte GetRandomResult()
    {
        var intValue = (int)Mathf.Ceil(currentKoreographyEventValue);
        var minValue = intValue - 1 < 0 ? 0 : intValue - 1;
        var maxValue = intValue + 1 >= GameConstant.BlockResults.Length ? GameConstant.BlockResults.Length : intValue + 1;
        return GameConstant.BlockResults[random.Next(minValue, maxValue)];
    }

    public void OnInitialize()
    {
        AudioManager.Instance.OnKoreographyEventAction += OnKoreographyEventActionMethod;
        random = new System.Random();
        problemLevel = (ProblemLevel)PlayerPrefs.GetInt("Setting_ProblemLevel", 0);
        IsInitialized = true;
    }
    
    /// <summary>
    /// 获取节奏返回值
    /// </summary>
    /// <param name="koreographyEvent"></param>
    private void OnKoreographyEventActionMethod(KoreographyEvent koreographyEvent)
    {
        if(!Global.Instance.isGamePlaying) 
            return;
        currentKoreographyEventValue = koreographyEvent.GetFloatValue();
    }

    public void OnRelease()
    {
        AudioManager.Instance.OnKoreographyEventAction -= OnKoreographyEventActionMethod;
    }
}
