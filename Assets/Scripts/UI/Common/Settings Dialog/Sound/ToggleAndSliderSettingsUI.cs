using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ToggleAndSliderSettingsUI : MonoBehaviour {
    [SerializeField] Toggle SettingToggle;
    [SerializeField] Slider SettingSlider;
    [SerializeField] protected TextMeshProUGUI Txt;

    void Start()  { 
        if(SettingToggle != null) {
            SettingToggle.onValueChanged.AddListener(ToggleChanged);
        }

        if(SettingSlider != null) {
            SettingSlider.onValueChanged.AddListener(SliderChanged);
        }

        StartAter(); 
    }
    
    protected virtual void StartAter() {}
    protected void Ready(bool isOn, float value) {
        if(SettingToggle != null) {
            SettingToggle.isOn = isOn;
        }

        if(SettingSlider != null) {
            SettingSlider.value = value;
        }
    }

    public virtual void ToggleChanged(bool isOn) {}
    public virtual void SliderChanged(float value) {}
}
