using System.Collections;
using System.Collections.Generic;

public class BaseMultipleStatus {
    public readonly int Count = 4;
    public BaseSingleStatus batter, B1, B2, B3;
    public bool IsB1Error, IsB2Error, IsB3Error;

    public BaseMultipleStatus(
        BaseSingleStatus b1=null,
        BaseSingleStatus b2=null,
        BaseSingleStatus b3=null) {
        B1 = b1;
        B2 = b2;
        B3 = b3;
    }

    public bool IsB1Filled() { return B1 != null; }
    public bool IsB2Filled() { return B2 != null; }
    public bool IsB3Filled() { return B3 != null; }
    public bool IsEmpty {
        get {
            return !IsB1Filled() && !IsB2Filled() && !IsB3Filled();
        }
    }
    public bool IsFull {
        get {
            return IsB1Filled() && IsB2Filled() && IsB3Filled();
        }
    }

    public BaseSingleStatus GetSingleStatus(int index) {
        int _index = Utils.GetSafeIndex(value:index, min:0, count:4);
        if(_index == 3) return B3;
        else if(_index == 2) return B2;
        else if(_index == 1) return B1;
        else return batter;
    }

    public void SetSingleStatus(int index, BaseSingleStatus status) {
        int _index = Utils.GetSafeIndex(value:index, min:0, count:4);
        if(_index == 3) B3 = status;
        else if(_index == 2) B2 = status;
        else if(_index == 1) B1 = status;
        else batter = status;
    }

    public bool IsBaseFilled(int baseNum) {
        int _index = Utils.GetSafeIndex(value:baseNum, min:1, count:3);
        if(_index == 3) return IsB3Filled();
        else if(_index == 2) return IsB2Filled();
        else return IsB1Filled();
    }

    public override string ToString() {
        return "\n(B0) : " + (batter != null ? batter.ToString() : "None") +
            "\n(B1) : " + (IsB1Filled() ? B1.ToString() : "None") +
            "\n(B2) : " + (IsB2Filled() ? B2.ToString() : "None") +
            "\n(B3) : " + (IsB3Filled() ? B3.ToString() : "None");
    }
}
