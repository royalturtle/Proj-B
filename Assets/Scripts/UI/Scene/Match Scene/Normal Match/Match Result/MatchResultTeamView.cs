using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MatchResultTeamView : MonoBehaviour {
    [SerializeField] TextMeshProUGUI _scoreText, _teamNameText, _pitcherResultText, _pitcherNameText;
    [SerializeField] Image _image;

    public void SetData(Team team, int score, MatchResultTypes result, string pitcherName="") {
        if(_scoreText) {_scoreText.text = score.ToString();}
        if(_pitcherResultText) {
            switch(result) {
                case MatchResultTypes.WIN:
                    _pitcherResultText.gameObject.SetActive(true);
                    _pitcherNameText.gameObject.SetActive(true);
                    _pitcherResultText.text = "W";
                    break;
                case MatchResultTypes.LOSE:
                    _pitcherResultText.gameObject.SetActive(true);
                    _pitcherNameText.gameObject.SetActive(true);
                    _pitcherResultText.text = "L";
                    break;
                default:
                    _pitcherResultText.gameObject.SetActive(false);
                    _pitcherNameText.gameObject.SetActive(false);
                    break;
            }
        }
        if(_pitcherNameText) {
            _pitcherNameText.text = pitcherName;
        }
        if(team != null) {
            if(_teamNameText) {_teamNameText.text = team.Name;}
            // if(_image) {_image.sprite = ResourcesUtils.GetTeamIconImage(team.LogoName);}
        }
        
    }

}
