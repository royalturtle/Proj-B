using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectDialog : DialogBase {
    [SerializeField] protected List<Toggle> _toggleList;
    [SerializeField] protected Button _button;
    
    protected int OnIndex {
        get {
            int result = GameConstants.NULL_INT;

            if(_toggleList != null) {
                for(int i = 0; i < _toggleList.Count; i++) {
                    if(_toggleList[i] != null && _toggleList[i].isOn) {
                        result = i;
                        break;
                    }
                }
            }
            return result;
        }
    }
    
    protected override void AwakeAfter() {
        for(int i = 0; i < _toggleList.Count; i++) {
            _toggleList[i].onValueChanged.AddListener((bool value) => {
                ToggleChanged();
            });
        }

        if(_button) {
            _button.onClick.RemoveAllListeners();
            _button.onClick.AddListener(() => {
                Confirm();
                TurnActive(false);
            });
        }
    }

    protected virtual void ToggleChanged() {}
    protected virtual void Confirm() { }
}
