using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScheduleDialog : ScheduleBaseDialog {
    [SerializeField] OtherMatchListView _otherMatchList;
    [SerializeField] TextMeshProUGUI _selectedDateText;

    protected override void AwakeAfter() {
        if(_otherMatchList) {
            _otherMatchList.SetData(null);
        }
    }

    protected override void DayClickedAfter(DateObj date) {
        if(IsSelected && _turnManager != null) {
            List<MatchInfo> result = _turnManager.GetMatchInfoList(year:_year, turn:_selectedTurn);
            if(_otherMatchList != null) {
                _otherMatchList.SetData(result);
            }
        }
        else {
            if(_otherMatchList != null) {
                _otherMatchList.SetData(null);
            }
        }

        if(_selectedDateText) {
            _selectedDateText.text = date == null ? "" : date.MMaDDString;
        }
    }
}
