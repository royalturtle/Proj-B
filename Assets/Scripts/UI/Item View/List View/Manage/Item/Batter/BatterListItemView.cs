using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatterListItemView : PlayerListItemView {
    protected override string PositionToString(int position) {
        return LanguageSingleton.Instance.GetBatterPosition(position:position);
    }

    protected override Color32 GetMainLabelColor() {
        BatterPositionEnum position = (BatterPositionEnum)positionIndex;
        switch(position) {
            case BatterPositionEnum.NONE:
            case BatterPositionEnum.GROUP2:
                return ColorConstants.Instance.MainLabelColorGroup2;
            case BatterPositionEnum.CANDIDATE:
                return ColorConstants.Instance.MainLabelColorSub;
            default:
                return ColorConstants.Instance.MainLabelColorMain;
        }
    }
}
