using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TeamSingleRecordBasic : TeamSingleRecordBase {
    [SerializeField] TextMeshProUGUI _rankText, _nameText;
    [SerializeField] Image _clubImage;

    public override void SetData(TeamSeason team) {
        if(Utils.NotNull(team, team._team)) {
            SetText(_nameText, team._team.Name);

            if(_clubImage) {
                _clubImage.sprite = ResourcesUtils.GetTeamIconImage(team._team.LogoName);
            }
            if(_rankText) {
                _rankText.text = team._season.Rank.ToString();
            }
        }
    }

    public void SetLeagueSeason(TupleLeagueSeason leagueSeason) {
        if(leagueSeason != null && _rankText != null) {
            _rankText.text = leagueSeason.Rank.ToString();
            
            if(!_rankText.gameObject.activeSelf) {
                _rankText.gameObject.SetActive(true);
            }
        }
    }

    public void SetTeamData(string name, Sprite sprite) {
        SetText(_nameText, name);
        if(_clubImage) {
            _clubImage.sprite = sprite;
        }
        if(_rankText) {
            _rankText.gameObject.SetActive(false);
        }
    }
}
