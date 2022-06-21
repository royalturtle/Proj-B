using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuickProgressDialog : SelectDialog {
    int GetDay(int index) {
        switch(index) {
            case 0: return 1;
            case 1: return 3;
            case 2: return 7;
            case 3: return 30;
            default: return 1;
        }
    }

    Action<int> _confirmAction;
    protected override void Confirm() {
        int index = OnIndex;
        if(index != GameConstants.NULL_INT) {
            QuickMatchOptionToggle option = _toggleList[index].GetComponent<QuickMatchOptionToggle>();
            if(option != null && _confirmAction != null) {
                _confirmAction(option.TurnCount);
            }

        }
    }

    public void Ready(Action<int> confirmAction, int year, int turn) {
        _confirmAction = confirmAction;

        for(int i = 0; i< _toggleList.Count; i++) {
            QuickMatchOptionToggle option = _toggleList[i].GetComponent<QuickMatchOptionToggle>();
            if(option != null) {
                int add = GetDay(i);
                DateObj date = new DateObj(year:year, turn:turn+add);
                option.SetData(turnCount:add, date:date.YYYYAMMADDString);
            }
        }
    }
    
    protected override void BeforeOn() {
        for(int i = 0; i < _toggleList.Count; i++) {
            _toggleList[i].isOn = false;
        }
        if(_button) {_button.interactable = false;}
    }

    protected override void ToggleChanged() {
        if(_button) {
            _button.interactable = OnIndex != GameConstants.NULL_INT;
        }
    }

}
