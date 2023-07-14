using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[View(EView.GameOver)]
public class GameOverView : View
{
    Text infoTxt;
    Button confirmBtn;

    public override bool TryEnter()
    {
        viewGo = ViewManager.Instance.CreateView((int)EView.GameOver);
        return viewGo != null;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        infoTxt = viewGo.transform.Find("Text_Info").GetComponent<Text>();
        confirmBtn = viewGo.transform.Find("Button_Confirm").GetComponent <Button>();
        confirmBtn.onClick.AddListener(OnConfirmButtonClicked);
        infoTxt.text = $"时间 : {Util.FormatTimeStamp2HMS((int)BlockScrollManager.Instance.gameTimeStamp)}\n分数 : {BlockScrollManager.Instance.gameScore}";
    }

    public override void OnUpdate(float deltaTime, float unscaleDeltaTime)
    {
    }

    public override void OnExit()
    {
        base.OnExit();
        confirmBtn.onClick.RemoveListener(OnConfirmButtonClicked);
    }

    private void OnConfirmButtonClicked()
    {
        ViewManager.Instance.ChangeView((int)EView.Launcher);
    }

}
