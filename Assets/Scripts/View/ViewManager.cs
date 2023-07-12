using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewManager : MonoBehaviour, IManager
{
    public int currViewId;

    public int nextViewId;

    public bool IsInitialized { get; set; }

    private static ViewManager instance;

    public static ViewManager Instance
    {
        get
        {
            if(instance == null)
            {
                var go = new GameObject("ViewManager");
                go.transform.SetParent(Global.Instance.transform, false);
                instance = go.AddComponent<ViewManager>();
            }
            return instance;
        }
    }

    private Camera _camera;
    public Camera UICamera => _camera;

    private Camera _noneCamera;
    public Camera NoneCamera => _noneCamera;


    private Dictionary<int, View> _windows = new Dictionary<int, View>();

    public void OnInitialize()
    {
        currViewId = (int)EView.None;
        nextViewId = (int)EView.Launcher;
        StartCoroutine(nameof(CoInitialize));
    }

    private IEnumerator CoInitialize()
    {
        var go = new GameObject("UI");

        _camera = go.AddComponent<Camera>();
        _camera.backgroundColor = new Color(0, 0, 0, 0);
        _camera.clearFlags = CameraClearFlags.Depth;
        _camera.cullingMask = 1 << Setting.LAYER_UI;
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

    public void OnRelease()
    {
        currViewId = (int)EView.None;
        nextViewId = (int)EView.None;

        _windows.Clear();

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

    public void ChangeView(int stateId)
    {
        if(stateId > (int)EView.None && stateId < (int)EView.Count)
        {
            nextViewId = stateId;
        }
    }

}
