using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuickMatchPlayButton : MonoBehaviour {
    [SerializeField] Sprite _playSprite, _pauseSprite, _stoppingSprite, _addSprite;
    [SerializeField] TextMeshProUGUI _textObj;
    [SerializeField] Image _image;
    Button _button;

    Func<QuickMatchModes> _getModeAction;
    Action<QuickMatchModes> _setModeAction;

    void Awake() {
        _button = GetComponent<Button>();
        if(_button) {
            _button.onClick.RemoveAllListeners();
            _button.onClick.AddListener(() => {
                ButtonClicked();
            });
        }
    }

    public void Ready(
        Func<QuickMatchModes> getModeAction,
        Action<QuickMatchModes> setModeAction
    ) {
        _getModeAction = getModeAction;
        _setModeAction = setModeAction;
    }

    void ButtonClicked() {
        if(_getModeAction == null || _setModeAction == null) { return ; }

        QuickMatchModes mode = _getModeAction();
        QuickMatchModes newMode = QuickMatchModes.NONE;

        switch(mode) {
            case QuickMatchModes.STARTING:
            case QuickMatchModes.RUNNING:
                newMode = QuickMatchModes.PAUSING;
                break;
            case QuickMatchModes.PAUSING:
                newMode = QuickMatchModes.RUNNING;
                break;
            case QuickMatchModes.PAUSED:
                newMode = QuickMatchModes.STARTING;
                break;
        }

        if(newMode != QuickMatchModes.NONE) {
            SetMode(newMode);
            _setModeAction(newMode);
        }
    }

    public void SetMode(QuickMatchModes mode) {
        Sprite sprite = null;
        string text = "";
        bool isInteractable = true;
        bool isChange = true;

        switch(mode) {
            case QuickMatchModes.PAUSING:
                sprite = _stoppingSprite;
                text = LanguageSingleton.Instance.GetActionName(17);
                break;
            case QuickMatchModes.PAUSED:
                sprite = _playSprite;
                text = LanguageSingleton.Instance.GetActionName(18);
                break;
            case QuickMatchModes.ALL_FINISHED:
                isInteractable = false;
                isChange = false;
                break;
            default:
                sprite = _pauseSprite;
                text = LanguageSingleton.Instance.GetActionName(16);
                break;
        }

        if(isChange) {
            if(_textObj) {
                _textObj.text = text;
            }

            if(_image) {
                _image.sprite = sprite;
            }
        }
        

        if(_button) {
            _button.interactable = isInteractable;
        }
    }

    // LanguageSingleton.Instance.GetActionName(isPaused ? 17 : 16)
    // 16 : _pauseSprite
    // 17 : _stoppingSprite
    // 18 : _playSprite
}
