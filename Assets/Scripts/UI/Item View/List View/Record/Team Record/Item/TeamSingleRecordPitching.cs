using UnityEngine;
using TMPro;

public class TeamSingleRecordPitching : TeamSingleRecordBase {
    [SerializeField] TextMeshProUGUI _kText, _bbText, _erText, _eraText, _whipText;

    public override void SetData(TeamSeason team) {
        if(team != null && team._season != null) {
            SetText(_eraText, Utils.doubleToString(team._season.PitcherERA, 3));
            SetText(_whipText, Utils.doubleToString(team._season.PitcherWHIP, 3));
        }

    }
}
