using UnityEngine;
using UnityEngine.UI;

[View(EView.Launcher)]
public class LauncherWindowState : View
{
    public override bool TryEnter()
    {
        viewGo = GameObject.Instantiate(Resources.Load<GameObject>("LauncherWindow"));
        return viewGo != null;
    }

    public override void OnEnter()
    {
        Debug.Log("LauncherWindowState OnEnter!");
        base.OnEnter();
        Button btn = viewGo.GetComponentInChildren<Button>();
        if(btn != null )
        {
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(OnButtonClicked);
        }
    }

    public override void OnUpdate(float deltaTime, float unscaleDeltaTime)
    {
        Debug.Log("LauncherWindowState OnUpdate!");
    }

    public override void OnExit()
    {
        Debug.Log("LauncherWindowState OnExit!");
        base.OnExit();
    }

    private void OnButtonClicked()
    {
        Debug.Log("LauncherWindowState OnButtonClicked!");
        ViewManager.Instance.ChangeView((int)EView.Loading);
    }

}
