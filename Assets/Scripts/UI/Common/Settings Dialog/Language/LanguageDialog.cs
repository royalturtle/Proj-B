using UnityEngine;
using UnityEngine.UI;

public class LanguageDialog : SelectDialog {
    int GetLangIndex { get { return PlayerPrefs.GetInt(GameConstants.PrefLanguage, -1); } }

    protected override void Confirm() {
        int langIndex = GetLangIndex;
        int onIndex = OnIndex;

        if(langIndex != onIndex) {
            LanguageSingleton.Instance.ChangeLangIndex(onIndex);
        }

        TurnActive(false);
    }

    protected override void BeforeOn() {
        int langIndex = GetLangIndex;
        
        if(Utils.IsValidIndex(_toggleList, langIndex)) {
            _toggleList[langIndex].isOn = true;
        }
    }
}
