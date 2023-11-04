using System;
using UnityEngine;
using UnityEngine.UI;

[View(EView.Setting)]
public class SettingView : View
{
    Button closeBtn;
    Slider audioVolSlider;
    Toggle audioToggle;
    Dropdown modelDropdown;

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
        modelDropdown = viewGo.transform.Find("Dropdown_Model").GetComponent<Dropdown>();
        audioVolSlider.value = AudioManager.Instance.Volume;
        modelDropdown.value = PlayerPrefs.GetInt("Setting_ProblemLevel", 0);

        closeBtn.onClick.AddListener(OnCloseButtonClicked);
        audioVolSlider.onValueChanged.AddListener(OnAudioVolSliderValueChanged);
        audioToggle.onValueChanged.AddListener(OnAudioToggleValueChanged);
        modelDropdown.onValueChanged.AddListener(OnModelDropdownValueChanged);
    }

    private void OnModelDropdownValueChanged(int level)
    {
        BlockScrollManager.Instance.problemLevel = (ProblemLevel)level;
        PlayerPrefs.SetInt("Setting_ProblemLevel", level);
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

    public override void OnExit()
    {
        base.OnExit();
        closeBtn.onClick.RemoveListener(OnCloseButtonClicked);
        audioVolSlider.onValueChanged.RemoveListener(OnAudioVolSliderValueChanged);
        audioToggle.onValueChanged.RemoveListener(OnAudioToggleValueChanged);
    }

}
