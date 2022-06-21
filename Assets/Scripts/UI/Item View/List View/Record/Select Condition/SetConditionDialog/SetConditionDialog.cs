using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SetConditionDialog : DialogBase {
    [SerializeField] LocalizeScript _labelText;
    [SerializeField] Button _confirmButton, _deleteButton;
    [SerializeField] SetConditionItem _greaterCondition, _lessCondition;

    Action<int> _deleteAction, _setAction;
    int _type;

    protected override void AwakeAfter() {
        if(_deleteButton != null) {
            _deleteButton.onClick.AddListener(() => {
                if(_deleteAction != null) {
                    _deleteAction(_type);
                }
            });
        }

        if(_confirmButton != null) {
            _confirmButton.onClick.AddListener(() => {
                if(_setAction != null) {
                    _setAction(_type);
                }
            });
        }
    }

    public void Ready(Action<int> deleteAction, Action<int> setAction) {
        _deleteAction = deleteAction;
        _setAction = setAction;
    }

    public void Open(LocalizationTypes localize, RecordCondition condition) {
        if(condition != null) {
            _type = condition.Type;
            if(_labelText != null) {
                _labelText.SetData(type:localize, value:_type);
            }
            TurnActive(true);

            if(_greaterCondition) {
                _greaterCondition.Ready(
                    numberType : condition.NumberType,
                    value      : condition.MinValue
                );
            }
            if(_lessCondition) {
                _lessCondition.Ready(
                    numberType : condition.NumberType,
                    value      : condition.MaxValue
                );
            }
            TurnActive(true);
        }
    }
}
