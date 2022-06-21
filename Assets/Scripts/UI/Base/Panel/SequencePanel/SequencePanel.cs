using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequencePanel : MonoBehaviour {
    public Action<bool> CheckAction;

    public bool IsFinished {
        get {
            bool result = true;
            if(_checkList != null) {
                for(int i = 0; i < _checkList.Count; i++) {
                    if(!_checkList[i]) {
                        result = false;
                        break;
                    }
                }
            }
            return result;
        }
    }

    protected List<bool> _checkList;

    protected void Check(int index, bool value) {
        if(Utils.IsValidIndex(_checkList, index)){
            _checkList[index] = value;
        }

        if(CheckAction != null) {
            CheckAction(IsFinished);
        }
    }    
}
