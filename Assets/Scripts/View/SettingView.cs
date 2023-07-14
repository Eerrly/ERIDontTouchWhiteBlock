using UnityEngine;
using UnityEngine.UI;

[View(EView.Setting)]
public class SettingView : View
{
    Button closeBtn;
    Slider audioVolSlider;
    Toggle audioToggle;

    public override bool TryEnter()
    {
        viewGo = ViewManager.Instance.CreateView((int)EView.Setting);
        return viewGo != null;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        closeBtn = viewGo.transform.Find("Button_Close").GetComponent<Button>();
        audioVolSlider = viewGo.transform.Find("Slider_AudioVol").GetComponent<Slider>();
        audioToggle = viewGo.transform.Find("Toggle_Audio").GetComponent<Toggle>();
        audioVolSlider.value = AudioManager.Instance.Volume;

        closeBtn.onClick.AddListener(OnCloseButtonClicked);
        audioVolSlider.onValueChanged.AddListener(OnAudioVolSliderValueChanged);
        audioToggle.onValueChanged.AddListener(OnAudioToggleValueChanged);
    }

    private void OnCloseButtonClicked()
    {
        ViewManager.Instance.ChangeView((int)EView.Launcher);
    }

    private void OnAudioVolSliderValueChanged(float value)
    {
        AudioManager.Instance.Volume = value;
    }

    private void OnAudioToggleValueChanged(bool isOn)
    {
        if(isOn)
        {
            AudioManager.Instance.PlayAudio();
        }
        else
        {
            AudioManager.Instance.StopAudio();
        }
    }

    public override void OnUpdate(float deltaTime, float unscaleDeltaTime)
    {
    }

    public override void OnExit()
    {
        base.OnExit();
        closeBtn.onClick.RemoveListener(OnCloseButtonClicked);
        audioVolSlider.onValueChanged.RemoveListener(OnAudioVolSliderValueChanged);
        audioToggle.onValueChanged.RemoveListener(OnAudioToggleValueChanged);
    }

}
