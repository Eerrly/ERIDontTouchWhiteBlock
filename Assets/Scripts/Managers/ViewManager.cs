using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 界面管理器
/// </summary>
public class ViewManager : SingletonMono<ViewManager>, IManager
{
    /// <summary>
    /// 当前界面ID
    /// </summary>
    [System.NonSerialized] public int currViewId;

    /// <summary>
    /// 下一个界面ID
    /// </summary>
    [System.NonSerialized] public int nextViewId;

    public bool IsInitialized { get; set; }

    private Camera _camera;
    public Camera ViewCamera => _camera;

    private Camera _noneCamera;
    public Camera NoneCamera => _noneCamera;

    public Dictionary<int, GameObject> cacheViewDic = new Dictionary<int, GameObject>();

    /// <summary>
    /// 初始化
    /// </summary>
    public void OnInitialize()
    {
        currViewId = (int)EView.None;
        nextViewId = (int)EView.Launcher;
        StartCoroutine(nameof(CoInitialize));
    }

    /// <summary>
    /// 初始化
    /// </summary>
    /// <returns></returns>
    private IEnumerator CoInitialize()
    {
        var go = new GameObject("UI");

        _camera = go.AddComponent<Camera>();
        _camera.backgroundColor = new Color(0, 0, 0, 0);
        _camera.clearFlags = CameraClearFlags.Depth;
        _camera.cullingMask = 1 << Setting.LAYER_VIEW;
        _camera.orthographic = true;
        _camera.transform.position = new Vector2(Screen.width / 2, Screen.height / 2);
        _camera.nearClipPlane = -200000;
        _camera.farClipPlane = 200000;
        _camera.depth = 1;
        _camera.allowHDR = false;
        _camera.allowMSAA = false;

        if (_noneCamera == null)
        {
            var noneCamraGo = new GameObject("NoneCamera");
            noneCamraGo.transform.parent = go.transform;
            _noneCamera = noneCamraGo.AddComponent<Camera>();
            _noneCamera.clearFlags = CameraClearFlags.SolidColor;
            _noneCamera.backgroundColor = Color.black;
            _noneCamera.depth = -50;
            _noneCamera.cullingMask = 0;
            _noneCamera.allowHDR = false;
            _noneCamera.allowMSAA = false;
            _noneCamera.useOcclusionCulling = false;
        }

        yield return null;
        IsInitialized = true;
    }

    /// <summary>
    /// 释放
    /// </summary>
    public void OnRelease()
    {
        currViewId = (int)EView.None;
        nextViewId = (int)EView.None;

        cacheViewDic.Clear();

        IsInitialized = false;
    }

    private void Update()
    {
        if (!IsInitialized)
        {
            return;
        }
        ViewMachine.Instance.Update(Time.deltaTime, Time.unscaledDeltaTime);
        ViewMachine.Instance.DoChangeView();
    }

    /// <summary>
    /// 更改当前界面
    /// </summary>
    /// <param name="viewId">界面ID</param>
    public void ChangeView(int viewId)
    {
        if(viewId == currViewId)
        {
            return;
        }
        if(viewId > (int)EView.None && viewId < (int)EView.Count)
        {
            nextViewId = viewId;
        }
    }

    /// <summary>
    /// 创建界面
    /// </summary>
    /// <param name="viewId">界面ID</param>
    /// <returns>界面实体</returns>
    public GameObject CreateView(int viewId)
    {
        GameObject viewGo;
        if(!cacheViewDic.TryGetValue(viewId, out viewGo))
        {
            viewGo = GameObject.Instantiate(Resources.Load<GameObject>($"Prefabs/{System.Enum.GetName(typeof(EView), viewId)}View"));
            cacheViewDic.Add(viewId, viewGo);
        }
        return viewGo;
    }

}
