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

}
