using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OtherMatchView : MonoBehaviour {
    [SerializeField] TextMeshProUGUI _awayScoreText, _homeScoreText;
    [SerializeField] Image _awayImage, _homeImage;

    public void SetData(MatchInfo match) {

        if(match == null) {
            gameObject.SetActive(false);
        }
        /*
        if(_awayImage) {
            _awayImage.gameObject.SetActive(match != null);
        }
        if(_homeImage) {
            _homeImage.gameObject.SetActive(match != null);
        }

        if(match == null) {
            if(_awayScoreText) {_awayScoreText.text="";}
            if(_homeScoreText) {_homeScoreText.text="";}
        }
        */
        else {
            gameObject.SetActive(true);
            if(_awayImage) {    
                _awayImage.sprite = ResourcesUtils.GetTeamIconImage(match.AwayTeam.LogoName);
            }
            if(_homeImage) {
                _homeImage.sprite = ResourcesUtils.GetTeamIconImage(match.HomeTeam.LogoName);
            }

            if(_awayScoreText) {
                _awayScoreText.text= match.MatchData.IsEnded ? match.MatchData.AwayScore.ToString() : "";
            }
            if(_homeScoreText) {
                _homeScoreText.text= match.MatchData.IsEnded ? match.MatchData.HomeScore.ToString() : "";
            }

        }
    }

}
