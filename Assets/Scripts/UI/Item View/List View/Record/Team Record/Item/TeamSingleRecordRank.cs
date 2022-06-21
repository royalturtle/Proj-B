using UnityEngine;
using TMPro;

public class TeamSingleRecordRank : TeamSingleRecordBase {
    [SerializeField] TextMeshProUGUI _gameText, _winText, _drawText, _loseText, _rateText, _diffText;

    public override void SetData(TeamSeason team) {
        if(team != null && team._season != null) {
            SetLeagueSeason(team._season);
        }
    }

    public void SetLeagueSeason(TupleLeagueSeason leagueSeason) {
        if(leagueSeason != null) {
            SetText(_gameText, leagueSeason.GameCount.ToString(), maxLength:3);
            SetText(_winText,  leagueSeason.WinCount.ToString(), maxLength:3);
            SetText(_drawText, leagueSeason.DrawCount.ToString(), maxLength:3);
            SetText(_loseText, leagueSeason.LoseCount.ToString(), maxLength:3);
            SetText(_rateText, Utils.doubleToString(leagueSeason.WinRate, 3));
            SetText(
                _diffText, 
                leagueSeason.WinDiff == 0.0 ? "-" : Utils.doubleToString(leagueSeason.WinDiff, 1)
            );
        }
    }
}
