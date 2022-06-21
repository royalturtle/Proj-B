using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatterRecordView : RecordFullView<Batter> {
    public override int StartType() { return (int)BatterStatType.OPS; }

    protected override bool CheckRegular(Batter player) {
        bool result = false;
        if(player != null) {
            int gameCount = _getGameCountAction(player.Stats.TeamID);
            int minPA = (int)(_nation == NationTypes.KOREA ? Mathf.Floor(((float)gameCount * 3.1f)) : Mathf.Round((float)gameCount * 3.1f));
            result = player.Season.PA >= minPA;
        }
        return result;
    }

    protected override bool IsBiggerStatGood(int statType) {
        BatterStatType type = (BatterStatType)statType;
        switch(type) {
            case BatterStatType.SO:
            case BatterStatType.GIDP:
            case BatterStatType.E:
                return false;
        }
        
        return true;
    }
}
