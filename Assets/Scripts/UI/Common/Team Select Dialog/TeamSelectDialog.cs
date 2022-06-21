using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamSelectDialog : DialogBase {
    [SerializeField] List<TeamSelectItemRow> _rowList;
    List<List<Team>> _data;
    int _index, _myTeamIndex;
    bool _isMyTeamSelectable;

    Action<Team> _clickAction;

    protected override void AwakeAfter() {
        for(int i = 0; i < _rowList.Count; i++) {
            _rowList[i]._team1.SetClickAction(ClickAction);
            _rowList[i]._team2.SetClickAction(ClickAction);
        }
    }

    public void SetData(List<List<Team>> data) {
        _data = data;
        if(_data != null) {
            SetIndex(0);
        }
    }

    public void SetMyTeamIndex(int id) {
        _myTeamIndex = id;
    }

    public void TurnOnTeamId(int id) {
        int result = GameConstants.NULL_INT;

        if(_data != null) {
            for(int i = 0; i < _data.Count; i++) {
                for(int j = 0; j < _data.Count; j++) {
                    if(_data[i][j].ID == id) {
                        result = i;
                        break;
                    }
                }

                if(result != GameConstants.NULL_INT) {
                    break;
                }
            }
        }

        SetIndex(result);
    }

    public void Next() {
        if(_data != null && ++_index >= _data.Count) {
            _index = 0;
        }

        SetIndex(_index);
    }

    public void Previous() {
        if(_data != null && --_index < 0) {
            _index = _data.Count - 1;
        }

        SetIndex(_index);
    }

    void SetIndex(int value) {
        _index = value;

        if(_data != null) {
            int index = 0, length = _data[_index].Count, repeat = (length + 1) / 2, i;

            for(i = 0; i < repeat; i++) {
                Team team1 = _data[_index][index++];
                Team team2 = index < length ? _data[_index][index++] : null;
                _rowList[i].SetData(team1, team2);
                _rowList[i].SetInteractable(
                    value1 : (team1 == null) || (!_isMyTeamSelectable && team1.ID == _myTeamIndex) ? false : true,
                    value2 : (team2 == null) || (!_isMyTeamSelectable && team2.ID == _myTeamIndex) ? false : true
                );
            }

            for(; i < _rowList.Count; i++) {
                _rowList[i].SetData();
                _rowList[i].SetInteractable();
            }
        }
    }

    void ClickAction(Team team) {
        if(_clickAction != null) { 
            _clickAction(team); 
        }
        
        TurnActive(false);
    }

    public void OpenButtonMode(Action<Team> clickAction, bool isMyTeamSelectable, int teamIndex = -1) {
        _isMyTeamSelectable = isMyTeamSelectable;
        _clickAction = clickAction;
        TurnOnTeamId(teamIndex == -1 ? _myTeamIndex : teamIndex);
        TurnActive(true);
    }
}
