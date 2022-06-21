using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SetConditionItem : MonoBehaviour {
    [SerializeField] bool _isGreater;
    [SerializeField] Toggle _toggle;
    [SerializeField] TMP_InputField _inputField;

    TMP_InputField.ContentType _contentType = TMP_InputField.ContentType.DecimalNumber;
    public double Value {get; private set;}

    void Awake() {
        if(_toggle != null) {
            _toggle.onValueChanged.AddListener((bool isOn) => {
                if(_inputField != null) {
                    _inputField.interactable = isOn;
                    if(!isOn) {
                        _inputField.text = "";
                    }
                }
                if(!isOn) {
                    Value = DefaultValue;
                }
            });
        }
    }

    public void Ready(NumberType numberType, double value) {
        if(_inputField != null && _toggle != null) {
            bool isNumberInt = numberType == NumberType.Int;
            _contentType = isNumberInt ?
                TMP_InputField.ContentType.IntegerNumber : TMP_InputField.ContentType.DecimalNumber;
            _inputField.contentType = _contentType;

            double defaultValue = DefaultValue;
            bool isOn = false;
            if(_isGreater) {
                isOn = defaultValue < value;
            }
            else {
                isOn = defaultValue > value;
            }

            if(!isOn) {
                value = defaultValue;
            }

            _toggle.isOn = isOn;
            Value = value;
        }
    }

    double DefaultValue {
        get {
            return _isGreater ? Int32.MinValue : Int32.MaxValue;
        }
    }
}
