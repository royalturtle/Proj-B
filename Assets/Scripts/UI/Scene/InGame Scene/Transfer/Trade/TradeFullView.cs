using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TradeFullView : FullPanel {
    [SerializeField] TradeTeamItem _my, _opponent;
    [SerializeField] TradeResult _result;

    public void SetData(Team team, bool isPlayer) {
        if(isPlayer) {
            if(_my) {_my.SetData(team);}
        }
        else {
            if(_opponent) {_opponent.SetData(team);}
        }
    }
}
