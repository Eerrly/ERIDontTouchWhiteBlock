using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global : MonoBehaviour
{
    private static Global instance;

    public static Global Instance => instance;

    public float timeScale = 1;
    public bool isGameover = false;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        SDKManager.Instance.OnInitialize();
        AudioManager.Instance.OnInitialize();
        ViewManager.Instance.OnInitialize();
        BlockScrollManager.Instance.OnInitialize();
        MonoManager.Instance.OnInitialize();
    }

    private void OnDestroy()
    {
        MonoManager.Instance.OnRelease();
        BlockScrollManager.Instance.OnRelease();
        ViewManager.Instance.OnRelease();
        AudioManager.Instance.OnRelease();
        SDKManager.Instance.OnRelease();
    }

}
