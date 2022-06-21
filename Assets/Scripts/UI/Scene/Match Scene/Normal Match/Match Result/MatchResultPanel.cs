using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MatchResultPanel : MonoBehaviour {
    [SerializeField] MatchResultTeamView _awayTeam, _homeTeam;
    [SerializeField] TextMeshProUGUI _resultText;
    [SerializeField] List<Image> _awayImageList;
    [SerializeField] List<Image> _homeImageList;

    public void SetData(MatchStatus matchStatus) {
        if(matchStatus != null && matchStatus.Score != null) {
            MatchResultTypes homeResult = matchStatus.Score.GetResultOfHomeTeam;
            if(_awayTeam) {
                _awayTeam.SetData(
                    team  : matchStatus.AwayTeamInfo.RegisteredTeam,
                    score : matchStatus.Score.GetScore(isHome:false),
                    result : homeResult == MatchResultTypes.WIN ? MatchResultTypes.LOSE : homeResult
                );
            }
            if(_homeTeam) {
                _homeTeam.SetData(
                    team  : matchStatus.HomeTeamInfo.RegisteredTeam,
                    score : matchStatus.Score.GetScore(isHome:true),
                    result : homeResult
                );
            }

            if(_resultText != null) {
                MatchResultTypes result = matchStatus.PlayerResult;
                switch(result) {
                    case MatchResultTypes.WIN:
                        _resultText.text = "WIN";
                        _resultText.color = ColorConstants.Instance.Green;
                        break;
                    case MatchResultTypes.DRAW:
                        _resultText.text = "DRAW";
                        _resultText.color = ColorConstants.Instance.White;
                        break;
                    case MatchResultTypes.LOSE:
                        _resultText.text = "LOSE";
                        _resultText.color = ColorConstants.Instance.Red;
                        break;
                    default:
                        _resultText.text = "";
                        _resultText.color = ColorConstants.Instance.White;
                        break;
                }
            }
            

            for(int i = 0; i < _awayImageList.Count; i++) {
                _awayImageList[i].sprite = matchStatus.AwayTeamInfo.TeamLogo;
            }

            for(int i = 0; i < _homeImageList.Count; i++) {
                _homeImageList[i].sprite = matchStatus.HomeTeamInfo.TeamLogo;
            }
        }
    }
}
