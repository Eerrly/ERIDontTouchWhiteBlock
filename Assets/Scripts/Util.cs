using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class ScoreData
{
    public int Score;
    public float PassedTime;
    public string DateTime;

    public ScoreData(int score, float passedTime, string dateTime)
    {
        Score = score;
        PassedTime = passedTime;
        DateTime = dateTime;
    }
}

[System.Serializable]
public class Serialization<T>
{
    [SerializeField]
    List<T> list;
    public List<T> ToList() { return list; }

    public Serialization(List<T> list)
    {
        this.list = list;
    }
}

public class Util
{
    private static List<Transform> _setGameObjectLayerList = new List<Transform>();

    /// <summary>
    /// 设置物体的Layer
    /// </summary>
    /// <param name="go"></param>
    /// <param name="layer"></param>
    /// <param name="includeChildren"></param>
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

    /// <summary>
    /// 将时间戳转成时分秒或者分秒
    /// </summary>
    /// <param name="timeStamp"></param>
    /// <returns></returns>
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

    /// <summary>
    /// 获取本地排行分数数据
    /// </summary>
    /// <returns></returns>
    public static List<ScoreData> GetScoreDatas()
    {
        if (!File.Exists(Setting.P_RANK_PATH))
        {
            return new List<ScoreData>();
        }
        List<ScoreData> scoreDatas = JsonUtility.FromJson<Serialization<ScoreData>>(File.ReadAllText(Setting.P_RANK_PATH)).ToList();
        return scoreDatas;
    }

    /// <summary>
    /// 设置本地排行分数数据
    /// </summary>
    /// <param name="scoreData"></param>
    /// <returns></returns>
    public static void SetScoreDatas(ScoreData scoreData)
    {
        List<ScoreData> scoreDatas = new List<ScoreData>();
        if (File.Exists(Setting.P_RANK_PATH))
        {
            scoreDatas = JsonUtility.FromJson<Serialization<ScoreData>>(File.ReadAllText(Setting.P_RANK_PATH)).ToList();
        }
        scoreDatas.Add(scoreData);
        scoreDatas.Sort((x, y) => -x.Score.CompareTo(y.Score));
        if (scoreDatas.Count > GameConstant.MaxRankInfoCount) scoreDatas.RemoveAt(scoreDatas.Count - 1);
        File.WriteAllText(Setting.P_RANK_PATH, JsonUtility.ToJson(new Serialization<ScoreData>(scoreDatas)));
    }

}
