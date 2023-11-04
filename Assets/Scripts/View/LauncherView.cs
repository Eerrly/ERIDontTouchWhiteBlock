using System;
using UnityEngine.UI;

[View(EView.Launcher)]
public class LauncherView : View
{
    readonly string Background_BGM = "Audios/BackgroundMusic";

    Button gameBtn;
    Button settingBtn;
    Button localrankBtn;

    public override bool TryEnter()
    {
        viewGo = ViewManager.Instance.CreateView((int)EView.Launcher);
        return viewGo != null;
    }

    public override void OnEnter()
    {
        base.OnEnter();

        gameBtn = viewGo.transform.Find("Button_Game").GetComponent<Button>();
        settingBtn = viewGo.transform.Find("Button_Setting").GetComponent<Button>();
        localrankBtn = viewGo.transform.Find("Button_LocalRank").GetComponent<Button>();
        gameBtn.onClick.AddListener(OnGameButtonClicked);
        settingBtn.onClick.AddListener(OnSettingButtonClicked);
        localrankBtn.onClick.AddListener(OnLocalRankButtonClicked);
    }

    private void OnGameButtonClicked()
    {
        if (!SDKManager.Instance.IsAntiAddictionSuccess)
        {
            SDKManager.Instance.StartUp(OnAntiAddictionLoginSuccess, OnAntiAddictionLogoutAccount);
            return;
        }
        OnAntiAddictionLoginSuccess();
    }

    /// <summary>
    /// 登录&防沉迷 成功
    /// </summary>
    private void OnAntiAddictionLoginSuccess()
    {
        BlockScrollManager.Instance.gameScore = 0;
        BlockScrollManager.Instance.gameTimeStamp = 0;
        ViewManager.Instance.ChangeView((int)EView.Game);
    }

    /// <summary>
    /// 登录&防沉迷失败
    /// </summary>
    private void OnAntiAddictionLogoutAccount()
    {
        ViewManager.Instance.ChangeView((int)EView.Launcher);
    }

    private void OnSettingButtonClicked()
    {
        ViewManager.Instance.ChangeView((int)EView.Setting);
    }

    private void OnLocalRankButtonClicked()
    {
        ViewManager.Instance.ChangeView((int)EView.LocalRank);
    }

    public override void OnExit()
    {
        base.OnExit();
        gameBtn.onClick.RemoveListener(OnGameButtonClicked);
        settingBtn.onClick.RemoveListener(OnSettingButtonClicked);
    }

}
