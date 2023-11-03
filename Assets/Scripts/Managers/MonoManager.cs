using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoManager : SingletonMono<MonoManager>, IManager
{
    public bool IsInitialized { get; set; }

    public void OnInitialize()
    {
        IsInitialized = true;
    }

    public void OnRelease()
    {
        StopAllCoroutines();
        IsInitialized = false;
    }
}
