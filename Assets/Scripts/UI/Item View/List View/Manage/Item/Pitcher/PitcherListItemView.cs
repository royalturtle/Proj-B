using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitcherListItemView : PlayerListItemView {
    protected override string PositionToString(int position) {
        return LanguageSingleton.Instance.GetPitcherPosition(position:position);
    }

    protected override Color32 GetMainLabelColor() {
        PitcherPositionEnum position = (PitcherPositionEnum)positionIndex;
        switch(position) {
            case PitcherPositionEnum.NONE:
            case PitcherPositionEnum.GROUP2:
                return ColorConstants.Instance.MainLabelColorGroup2;
            case PitcherPositionEnum.RELIEF:
                return ColorConstants.Instance.MainLabelColorSub;
            default:
                return ColorConstants.Instance.MainLabelColorMain;
        }
    }
}
