using UnityEngine;
using UnityEngine.UI;

[View(EView.Launcher)]
public class LauncherView : View
{
    Button gameBtn;
    Button settingBtn;

    public override bool TryEnter()
    {
        viewGo = ViewManager.Instance.CreateView((int)EView.Launcher);
        return viewGo != null;
    }

    public override void OnEnter()
    {
        Debug.Log("LauncherView OnEnter!");
        base.OnEnter();
        gameBtn = viewGo.transform.Find("Button_Game").GetComponent<Button>();
        settingBtn = viewGo.transform.Find("Button_Setting").GetComponent<Button>();
        gameBtn.onClick.AddListener(OnGameButtonClicked);
        settingBtn.onClick.AddListener(OnSettingButtonClicked);
    }

    private void OnGameButtonClicked()
    {
        Debug.Log("LauncherView OnGameButtonClicked!");
        BlockScrollManager.Instance.gameScore = 0;
        BlockScrollManager.Instance.gameTimeStamp = 0;
        BlockScrollManager.Instance.scrollSpeed = GameConstant.InitScrollSpeed;
        ViewManager.Instance.ChangeView((int)EView.Game);
    }

    private void OnSettingButtonClicked()
    {
        Debug.Log("LauncherView OnSettingButtonClicked!");
        ViewManager.Instance.ChangeView((int)EView.Setting);
    }

    public override void OnUpdate(float deltaTime, float unscaleDeltaTime)
    {
        Debug.Log("LauncherView OnUpdate!");
    }

    public override void OnExit()
    {
        Debug.Log("LauncherView OnExit!");
        base.OnExit();
        gameBtn.onClick.RemoveListener(OnGameButtonClicked);
        settingBtn.onClick.RemoveListener(OnSettingButtonClicked);
    }

}
