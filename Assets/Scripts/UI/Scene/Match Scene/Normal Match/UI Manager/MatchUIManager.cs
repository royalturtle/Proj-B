using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchUIManager : MonoBehaviour {
    [SerializeField] MatchCurrentPitcherView _currentPitcherObj;
    [SerializeField] MatchCurrentBatterView _currentBatterObj;
    [SerializeField] OutsObject _outsObject;
    [SerializeField] ScoreBoardObject _scoreObject;
    [SerializeField] NextBatterListObject _nextBatterListObj;

    [SerializeField] TeamSingleRecordBasic _awayTeamName, _homeTeamName;

    public void SetTeam(string homeTeamName, Sprite homeTeamSprite, string awayTeamName, Sprite awayTeamSprite) {
        if(_awayTeamName) {
            _awayTeamName.SetTeamData(name:awayTeamName,sprite:awayTeamSprite);
        }
        if(_homeTeamName) {
            _homeTeamName.SetTeamData(name:homeTeamName,sprite:homeTeamSprite);
        }
    }

    public void ReadyNextSituation(Pitcher pitcher, Batter batter, int batterOrder) {
        _currentPitcherObj.SetData(pitcher);
        _currentBatterObj.SetData(batter);

        _currentPitcherObj.SetTurn(true);
        _currentBatterObj.SetTurn(true);

        if(_nextBatterListObj != null) {
            _nextBatterListObj.SetBatterOrder(batterOrder);
        }
    }

    public void EndReadyAnimation() {
        _currentPitcherObj.SetTurn(false);
        _currentBatterObj.SetTurn(false);
    }

    public void SetDataByStatus(MatchStatus statusData) {
        if(_outsObject != null) {
            _outsObject.SetOutCount(statusData.Outs);
        }

        if(_scoreObject != null) { 
            _scoreObject.SetScoreBoard(statusData.Score);
        }

        if(_nextBatterListObj != null) {
            if(_nextBatterListObj.InningOrderShouldChange(statusData.InningInfo.Order)) {
                _nextBatterListObj.SetData(statusData.CurrentBattersList);
            }
        }
    }
}
