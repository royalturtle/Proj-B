using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuickProgressCalendarDialog : ScheduleBaseDialog {
    [SerializeField] TextMeshProUGUI _endDateText, _matchCountText;
    [SerializeField] Button _confirmButton;
    Action<int> _confirmAction;

    protected override void AwakeAfter() {
        if(_confirmButton) {
            _confirmButton.onClick.AddListener(() => {
                if(_confirmAction != null && IsSelected) {
                    _confirmAction(_selectedTurn - _todayTurn + 1);
                }
            });
        }
    }

    public void SetConfirmAction(Action<int> confirmAction) {
        _confirmAction = confirmAction;
    }

    protected override bool GetIsClickable(MatchInfo matchInfo) {
        return matchInfo != null && matchInfo.MatchData.Turn >= _todayTurn;
    }

    protected override void DayClickedAfter(DateObj date) {
        if(_confirmButton != null) {
            _confirmButton.interactable = IsSelected;
        }

        if(_endDateText != null) {
            _endDateText.text = (date != null) ? date.YYYYAMADDString : "-";
        }

        if(_matchCountText != null) {
            _matchCountText.text = (IsSelected) ? GetCountOfMyMatches().ToString() : "-";
        }
    }

    int GetCountOfMyMatches() {
        int result = 0;

        for(int i = _todayTurn; i <= _selectedTurn; i++) {
            if(GetMatch(i) != null) {
                result++;
            }
        }

        return result;
    }
}
