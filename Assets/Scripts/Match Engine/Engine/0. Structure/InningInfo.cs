using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InningInfo {
    public int Inning {get; private set;}
    public InningOrder Order {get; private set;}
    public int Outs {get; private set;}
    bool _endInningExist;
    int _endInning;

    public InningInfo(MatchTypes matchType, NationTypes nationType) {
        Outs = 0;
        Inning = 1;
        Order = InningOrder.PRE;

        _endInningExist = true;
        if(matchType == MatchTypes.PRESEASON) {
            _endInning = 9;
        }
        else if(nationType == NationTypes.USA) {
            _endInningExist = false;
        }
        else if(matchType == MatchTypes.SEASON || nationType == NationTypes.JAPAN) {
            _endInning = 12;
        }
        else {
            _endInning = 15;
        }
    }
    
    public bool IsOrderPre {
        get { return Order == InningOrder.PRE; }
    }

    public bool IsOrderPost {
        get { return Order == InningOrder.POST; }
    }

    public bool IsInningEnd(MatchResultTypes homeResult) {
        return (Inning >= 9 && Outs >= 3 && Order == InningOrder.PRE  && homeResult == MatchResultTypes.WIN) 
            || (Inning >= 9 && Outs >= 3 && Order == InningOrder.POST && homeResult == MatchResultTypes.LOSE)
            || (Inning >= 9 && Order == InningOrder.POST && homeResult == MatchResultTypes.WIN)
            || (_endInningExist && Inning >= _endInning && Outs >= 3 && Order == InningOrder.POST);
    }

    public void SetOut(int value) {
        Outs = value;
    }

    public void Next() {
        if(Order == InningOrder.POST) {
            Inning += 1;
            Order = InningOrder.PRE;
        }
        else {
            Order = InningOrder.POST;
        }
    }
    
    public override string ToString() {
        return Inning.ToString() + "(" + Order.ToString() + ")";
    }
}
