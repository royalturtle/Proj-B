using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitcherRecordView : RecordFullView<Pitcher> {
    public override int StartType() { return (int)PitcherStatType.ERA; }

    protected override bool CheckRegular(Pitcher player) {
        bool result = false;
        if(player != null) {
            int gameCount = _getGameCountAction(player.Stats.TeamID);
            result = player.Season.IP >= gameCount;
        }
        return result;
    }

    protected override bool IsBiggerStatGood(int statType) {
        PitcherStatType type = (PitcherStatType)statType;
        switch(type) {
            case PitcherStatType.ERA:
            case PitcherStatType.WHIP:
            case PitcherStatType.FIP:
                return false;
        }
        
        return true;
    }
}
