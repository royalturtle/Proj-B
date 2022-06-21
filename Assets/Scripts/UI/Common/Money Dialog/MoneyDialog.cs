using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MoneyDialog : DialogBase {
    [SerializeField] TMP_InputField _input;
    [SerializeField] Button _confirmButton, _clearButton;
    Action<int> _confirmAction;
    int _data;

    protected override void AwakeAfter() {
        if(_confirmButton) {
            _confirmButton.onClick.RemoveAllListeners();
            _confirmButton.onClick.AddListener(() => {
                Confirm();
            });
        }

        if(_confirmButton) {
            _clearButton.onClick.RemoveAllListeners();
            _clearButton.onClick.AddListener(() => {
                SetData(0);
            });
        }

        if(_input) {
            _input.onValueChanged.RemoveAllListeners();
            _input.onValueChanged.AddListener((result) => {
                SetData(Convert.ToInt32(result));
            });
        }
    }

    void Confirm() {
        if(_confirmAction != null) {
            _confirmAction(_data);
        }

        TurnActive(false);
    }

    public void Open(int money, Action<int> confirmAction) {
        _confirmAction = confirmAction;
        SetData(money);
        TurnActive(true);
    }

    void SetData(int value) {
        _data = value;
        
        if(_input) {
            _input.text = _data.ToString();
        }
    }
}
