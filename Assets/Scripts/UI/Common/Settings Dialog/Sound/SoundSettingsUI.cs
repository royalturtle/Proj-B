using UnityEngine;
using UnityEngine.Audio;

public abstract class SoundSettingsUI : ToggleAndSliderSettingsUI {
    [SerializeField] protected AudioMixer mixer;

    protected abstract string IsOnString { get; }
    protected abstract string ValueString { get; }
    
    protected override void StartAter() {
        Ready(
            isOn  : Utils.Int2Bool(PlayerPrefs.GetInt(IsOnString, 1)),
            value : PlayerPrefs.GetFloat(ValueString, 0.5f)
        );
    }
    
    public override void ToggleChanged(bool isOn) {
        float soundValue = isOn ? PlayerPrefs.GetFloat(ValueString, 0.5f) : 0.0001f;

        if(mixer != null) {
            mixer.SetFloat(ValueString, Utils.Float2Sound(soundValue));
        }

        PlayerPrefs.SetInt(IsOnString, Utils.Bool2Int(isOn));
    }
    
    public override void SliderChanged(float value) {
        bool isOn = Utils.Int2Bool(PlayerPrefs.GetInt(IsOnString, 1));

        if(mixer != null) {
            mixer.SetFloat(ValueString, Utils.Float2Sound(isOn ? value : 0.0001f));
        }

        if(Txt != null) {
            Txt.text = ((int)(value * 100)).ToString();
        }
        
        PlayerPrefs.SetFloat(ValueString, value);
    }
}
