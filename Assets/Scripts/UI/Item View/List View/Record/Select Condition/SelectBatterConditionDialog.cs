using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectBatterConditionDialog : SelectConditionDialog {
    public override void Ready() {
        BatterStatType statStart = BatterStatType.Hit;
        BatterStatType statEnd   = BatterStatType.DefenseOF;
        SetPanels(new int[]{0}, start:(int)statStart, end:(int)statEnd);

        BatterStatType recordStart = BatterStatType.G;
        BatterStatType recordEnd   = BatterStatType.OPS;
        SetPanels(new int[]{1, 2}, start:(int)recordStart, end:(int)recordEnd);
    }
    protected override LocalizationTypes GetTextKey() {return LocalizationTypes.BATTER_STAT;}
}
