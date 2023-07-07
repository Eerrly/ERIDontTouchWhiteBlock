using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 别踩白块游戏核心管理器
/// </summary>
public class BlockScrollManager : MonoBehaviour
{
    /// <summary>
    /// 事件缩放
    /// </summary>
    public float timeScale = 1;
    /// <summary>
    /// 滚动速度
    /// </summary>
    public float scrollSpeed = 150;
    /// <summary>
    /// 游戏总时长时间戳
    /// </summary>
    public float gameTimeStamp = 0;

    /// <summary>
    /// 是否按钮按下
    /// </summary>
    [System.NonSerialized] public bool IsPointerDown = false;
    /// <summary>
    /// 块的点击事件
    /// </summary>
    [System.NonSerialized] public BlockClickEvent blockClickEvent = new BlockClickEvent();

    private static BlockScrollManager instance = null;
    public static BlockScrollManager Instance
    {
        get { return instance; }
        private set { instance = value; }
    }

    private System.Random random;

    public void Awake()
    {
        Instance = this;
        random = new System.Random();
    }

    public void Start()
    {
        blockClickEvent.AddListener(OnBlockClickAction);
    }

    /// <summary>
    /// 获取一个数组中的任意一个
    /// </summary>
    /// <returns></returns>
    public byte GetRandomResult()
    {
        return GameConstant.BlockResults[random.Next(GameConstant.BlockResults.Length)];
    }

    /// <summary>
    /// 块点击的具体逻辑
    /// </summary>
    /// <param name="clickBlockIndex"></param>
    /// <param name="block"></param>
    void OnBlockClickAction(byte clickBlockIndex, Block block)
    {
        if (GameConstant.BlockResults[clickBlockIndex] == block.blockRaw.result)
        {
            block.SetImageColor(Color.gray);
        }
        else
        {
            block.SetImageColor(Color.red);
        }
    }

}
