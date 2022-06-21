using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreBoardObject : MonoBehaviour {
    [SerializeField] ScoreScrollView _scoreBoard;
    [SerializeField] ScoreBoardAdditionalObject _additionalBoard;

    public void SetScoreBoard(MatchScore scoreInfo) {
        if(_scoreBoard != null && scoreInfo != null) {
            for(int i = 0; i < scoreInfo.ScoreListHome.Count; i++) {
                _scoreBoard.SetScore(i + 1, scoreInfo.ScoreListHome[i], MatchPosition.HOME);
            }
            for (int i = 0; i < scoreInfo.ScoreListAway.Count; i++) {
                _scoreBoard.SetScore(i + 1, scoreInfo.ScoreListAway[i], MatchPosition.AWAY);
            }
        }

        _additionalBoard.SetScore(MatchPosition.HOME, scoreInfo.GetScore(isHome:true));
        _additionalBoard.SetScore(MatchPosition.AWAY, scoreInfo.GetScore(isHome:false));

        _additionalBoard.SetHit(MatchPosition.HOME, scoreInfo.HitCountHome);
        _additionalBoard.SetHit(MatchPosition.AWAY, scoreInfo.HitCountAway);

        _additionalBoard.SetError(MatchPosition.HOME, scoreInfo.ErrorCountHome);
        _additionalBoard.SetError(MatchPosition.AWAY, scoreInfo.ErrorCountAway);

        _additionalBoard.SetBB(MatchPosition.HOME, scoreInfo.BBCountHome);
        _additionalBoard.SetBB(MatchPosition.AWAY, scoreInfo.BBCountAway);
    }
}
