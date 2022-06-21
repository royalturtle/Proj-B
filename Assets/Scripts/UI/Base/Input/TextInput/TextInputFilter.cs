using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextInputFilter : MonoBehaviour {
    bool isWarningAppeared;
    [SerializeField] TMP_InputField _inputField;
    [SerializeField] int _lengthMin, _lengthMax;
    [SerializeField] TextMeshProUGUI _warningText;

    public string Text {get{return _inputField == null ? "" : _inputField.text;}}
    public Action<bool> CheckValidAction;

    void Awake() {
        if(_inputField) {
            _inputField.onValueChanged.AddListener(delegate {
                bool result = CheckLength();
                if(CheckValidAction != null) {
                    CheckValidAction(result);
                }
            });
        }

        if(_warningText) {
            _warningText.gameObject.SetActive(false);
        }
    }

    void SetWarningText() {
        if(_warningText) {
            string text = LanguageSingleton.Instance.GetWarning(0);
            _warningText.text = string.Format(text, _lengthMin, _lengthMax);
        }
    }

    bool CheckLength() {
        bool result = !IsLengthLimited;
        if(_inputField && IsLengthLimited) {
            int length = _inputField.text.Length;
            result = _lengthMin <= length && length <= _lengthMax;

            if(_warningText) {
                if(!isWarningAppeared) {
                    isWarningAppeared = true;
                    SetWarningText();
                    LanguageSingleton.Instance.LocalizeChanged += SetWarningText;
                }

                _warningText.gameObject.SetActive(!result && _warningText != null);
            }
        }
        
        return result;
    }

    bool IsLengthLimited {
        get {
            return _lengthMin > 0 && _lengthMax > 0;
        }
    }
}
