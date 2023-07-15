using System;
using TapTap.AntiAddiction;
using TapTap.AntiAddiction.Model;
using UnityEngine;

public enum EAntiAddictioCode
{
    /// <summary>
    /// 该玩家可以正常进入游戏
    /// </summary>
    LoginSuccess = 500,

    /// <summary>
    /// 玩家在游戏内退出账号时调用该接口，重置防沉迷状态
    /// </summary>
    LogoutAccount = 1000,
}

public class SDKManager : SingletonMono<SDKManager>, IManager
{
    public bool IsInitialized { get; set; }
    public bool IsAntiAddictionSuccess { get; set; }

    AntiAddictionConfig config;

    Action OnAntiAddictionLoginSuccess;
    Action OnAntiAddictionLogoutAccount;

    public void OnInitialize()
    {
        config = new AntiAddictionConfig()
        {
            gameId = "pmb05egy1jbemkik6z",
            showSwitchAccount = false,
        };
        AntiAddictionUIKit.Init(config, AntiAddictionCallback);
        IsInitialized = true;
    }

    /// <summary>
    /// SDK回调
    /// </summary>
    /// <param name="code"></param>
    /// <param name="errorMsg"></param>
    private void AntiAddictionCallback(int code, string errorMsg)
    {
        switch (code)
        {
            case (int)EAntiAddictioCode.LoginSuccess:
                IsAntiAddictionSuccess = true;
                OnAntiAddictionLoginSuccess?.Invoke();
                break;
            case (int)EAntiAddictioCode .LogoutAccount:
                IsAntiAddictionSuccess = false;
                AntiAddictionUIKit.Exit();
                OnAntiAddictionLogoutAccount?.Invoke();
                break;
        }
    }

    /// <summary>
    /// 实名认证
    /// </summary>
    public void StartUp(Action onLoginSuccessCallback, Action onLogoutAccountCallback)
    {
        OnAntiAddictionLoginSuccess += onLoginSuccessCallback;
        OnAntiAddictionLogoutAccount += onLogoutAccountCallback;
        AntiAddictionUIKit.Startup(SystemInfo.deviceUniqueIdentifier);
    }

    /// <summary>
    /// 获取玩家年龄段
    /// -1:未实名 0:0-7 8:8-15 16:16-17 18:成年
    /// </summary>
    /// <returns></returns>
    public int GetAgeRange()
    {
        int ageRange = AntiAddictionUIKit.AgeRange;
        return ageRange;
    }

    public void OnRelease()
    {
        OnAntiAddictionLoginSuccess = null;
        OnAntiAddictionLogoutAccount = null;
    }


}
