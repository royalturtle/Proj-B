using UnityEngine;
using TMPro;

public class TeamSingleRecordBatting : TeamSingleRecordBase {
    [SerializeField] TextMeshProUGUI _avgText, _opsText, _homerunText, _scoredText, _strealingBaseText;

    public override void SetData(TeamSeason team) {
        if(team != null && team._season != null) {
            SetText(_opsText, Utils.doubleToString(team._season.BatterOPS, 3));
            SetText(_homerunText, team._season.BatterHR.ToString());
            SetText(_strealingBaseText, team._season.BatterSB.ToString());
        }

    }
}
