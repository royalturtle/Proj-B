using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsDialog : DialogBase {
    int GetLangIndex { get { return PlayerPrefs.GetInt(GameConstants.PrefLanguage, -1); } }

    [SerializeField] Button _titleButton, _exitButton;
    [SerializeField] SceneBase _scene;
    [SerializeField] TextMeshProUGUI _langText;

    protected override void AwakeAfter() {
        if(_scene != null) {
            if(_titleButton) {
                _titleButton.onClick.AddListener(() => {
                    _scene.AskLoadTitleScene();
                });
            }

            if(_exitButton) {
                _exitButton.onClick.AddListener(() => {
                    _scene.AskQuitGame();
                });
            }
        }
    }

    protected override void BeforeOn() {
        if(_langText != null) {
            int langIndex = GetLangIndex;
            string value = "";
            
            switch(langIndex) {
                case 1: 
                    value = "ÇÑ±¹¾î";
                    break;
                default:
                    value = "English";
                    break;
            }
            
            _langText.text = value;
        }        
    }
}
