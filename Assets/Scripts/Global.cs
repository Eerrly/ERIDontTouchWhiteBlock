using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global : MonoBehaviour
{
    private static Global instance;

    public static Global Instance => instance;

    private void Awake()
    {
        instance = this;
    }


    void Start()
    {
        ViewManager.Instance.OnInitialize();
    }

}
