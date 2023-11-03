using System.Text;
using UnityEngine;
using UnityEngine.UI;

[View(EView.LocalRank)]
public class LocalRankView : View
{
    readonly string strFormatRankInfo1 = "<size=70>{0}.</size> 时间:{1} 分数:{2}\n日期:{3}";
    readonly string strForamtRankInfo2 = "<size=70>{0}.</size>";

    Button closeBtn;
    Text rankTxt;

    StringBuilder sb;

    public override bool TryEnter()
    {
        viewGo = ViewManager.Instance.CreateView((int)EView.LocalRank);
        return viewGo != null;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        sb = new StringBuilder();
        sb.Clear();

        closeBtn = viewGo.transform.Find("Button_Close").GetComponent<Button>();
        rankTxt = viewGo.transform.Find("Text_RankInfo").GetComponent<Text>();

        closeBtn.onClick.AddListener(OnCloseButtonClicked);
        SetRankInfoText();
    }

    public override void OnExit()
    {
        base.OnExit();
        closeBtn.onClick.RemoveListener(OnCloseButtonClicked);
    }

    private void OnCloseButtonClicked()
    {
        ViewManager.Instance.ChangeView((int)EView.Launcher);
    }

    private void SetRankInfoText()
    {
        var rankList = Util.GetScoreDatas();
        for (int i = 0; i < GameConstant.MaxRankInfoCount; i++)
        {
            if (rankList.Count > i)
                sb.AppendLine(string.Format(strFormatRankInfo1, i + 1, Util.FormatTimeStamp2HMS((int)rankList[i].PassedTime), rankList[i].Score, rankList[i].DateTime));
            else
                sb.AppendLine(string.Format(strForamtRankInfo2, i + 1));
        }
        rankTxt.text = sb.ToString();
    }

}
