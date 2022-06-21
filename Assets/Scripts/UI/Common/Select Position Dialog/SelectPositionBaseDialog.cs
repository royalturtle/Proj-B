using System;
using UnityEngine;

public class SelectPositionBaseDialog<T> : SelectDialog where T : System.Enum {
    T _formerPosition;
    Action<T> _confirmAction;

    public void Open(T position, Action<T> confirmAction) {
        _formerPosition = position;
        _confirmAction = confirmAction;
        int index = GetToggleIndexOfPosition(position:position);

        if(Utils.IsValidIndex(_toggleList, index)) {
            _toggleList[index].isOn = true;
        }

        TurnActive(true);
    }

    protected virtual int GetToggleIndexOfPosition(T position) {
        if(0 <= position.CompareTo(StartPosition) && position.CompareTo(EndPosition) <= 0) {
            return (int)(object)position - 1;
        }
        else {
            return GameConstants.NULL_INT;
        }
        
        return GameConstants.NULL_INT;
    }

    protected virtual T StartPosition {get{ return (T)(object)0; }}
    protected virtual T EndPosition {get{ return (T)(object)0; } }

    protected override void Confirm() {
        int onIndex = OnIndex;

        if(onIndex != GameConstants.NULL_INT) {
            T position = (T)(object)((int)(object)StartPosition + onIndex);

            if(_confirmAction != null && position.CompareTo(_formerPosition) != 0)  {
                _confirmAction(position);
            }
        }
    }
}
