using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util
{
    private static List<Transform> _setGameObjectLayerList = new List<Transform>();

    public static void SetGameObjectLayer(GameObject go, int layer, bool includeChildren)
    {
        if (null != go && go.layer != layer)
        {
            if (includeChildren)
            {
                _setGameObjectLayerList.Clear();
                go.GetComponentsInChildren<Transform>(true, _setGameObjectLayerList);
                foreach (var igo in _setGameObjectLayerList)
                {
                    igo.gameObject.layer = layer;
                }
            }
            else
            {
                go.layer = layer;
            }
        }
    }

    public static string FormatTimeStamp2HMS(int timeStamp)
    {
        int hour = timeStamp / 3600;
        int min = 0;
        int sec = 0;
        if (hour > 0)
        {
            min = (timeStamp % 3600) / 60;
            sec = min > 0 ? (timeStamp % 3600) % 60 : timeStamp % 3600;
        }
        else
        {
            min = timeStamp / 60;
            sec = min > 0 ? timeStamp % 60 : timeStamp;
        }
        return hour > 0 ? string.Format("{0:00}:{1:00}:{2:00}", hour, min, sec) : string.Format("{0:00}:{1:00}", min, sec);
    }

}
