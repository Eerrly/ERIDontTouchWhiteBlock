using UnityEngine;
using UnityEngine.UI;

[View(EView.Loading)]
public class LoadingWindowState : View
{

    public override bool TryEnter()
    {
        viewGo = GameObject.Instantiate(Resources.Load<GameObject>("LoadingWindow"));
        return viewGo != null;
    }

    public override void OnEnter()
    {
        Debug.Log("LoadingWindowState OnEnter!");
        base.OnEnter();
        Button btn = viewGo.GetComponentInChildren<Button>();
        if (btn != null)
        {
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(OnButtonClicked);
        }
    }

    private void OnButtonClicked()
    {
        ViewManager.Instance.ChangeView((int)EView.Launcher);
    }

    public override void OnUpdate(float deltaTime, float unscaleDeltaTime)
    {
        Debug.Log("LoadingWindowState OnUpdate!");
    }

    public override void OnExit()
    {
        Debug.Log("LoadingWindowState OnExit!");
        base.OnExit();
    }

}
