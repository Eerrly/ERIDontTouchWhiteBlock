using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonMono<T> : MonoBehaviour where T : MonoBehaviour, IManager
{

    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                var go = new GameObject(typeof(T).Name);
                go.transform.SetParent(Global.Instance.transform, false);
                instance = go.AddComponent<T>();
            }
            return instance;
        }
    }

}
