using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectPitcherConditionDialog : SelectConditionDialog{
    public override void Ready() {
        PitcherStatType statStart = PitcherStatType.Stamina;
        PitcherStatType statEnd   = PitcherStatType.ROpp;
        SetPanels(new int[]{0}, start:(int)statStart, end:(int)statEnd);

        PitcherStatType recordStart = PitcherStatType.G;
        PitcherStatType recordEnd   = PitcherStatType.FIP;
        SetPanels(new int[]{1, 2}, start:(int)recordStart, end:(int)recordEnd);
    }

    protected override LocalizationTypes GetTextKey() {return LocalizationTypes.PITCHER_STAT;}
}
