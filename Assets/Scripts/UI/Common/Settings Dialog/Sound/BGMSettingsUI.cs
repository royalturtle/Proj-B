public class BGMSettingsUI : SoundSettingsUI {
    protected override string IsOnString { get { return GameConstants.PREF_BGM_IS_ON; } }
    protected override string ValueString { get { return GameConstants.PREF_BGM_SOUND; } }
}
