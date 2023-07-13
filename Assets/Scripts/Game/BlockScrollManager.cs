using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 别踩白块游戏核心管理器
/// </summary>
public class BlockScrollManager : MonoBehaviour, IManager
{
    /// <summary>
    /// 滚动速度
    /// </summary>
    [System.NonSerialized] public float scrollSpeed = 300;
    /// <summary>
    /// 游戏总时长时间戳
    /// </summary>
    [System.NonSerialized] public float gameTimeStamp = 0;
    /// <summary>
    /// 是否按钮按下
    /// </summary>
    [System.NonSerialized] public bool IsPointerDown = false;

    private static BlockScrollManager instance = null;
    public static BlockScrollManager Instance
    {
        get
        {
            if (instance == null)
            {
                var go = new GameObject("BlockScrollManager");
                go.transform.SetParent(Global.Instance.transform, false);
                instance = go.AddComponent<BlockScrollManager>();
            }
            return instance;
        }
    }

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
        IsInitialized = true;
    }

    public void OnRelease()
    {
    }
}
