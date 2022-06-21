using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TeamRecordListItem : MonoBehaviour {
    TeamSeason _team;
    Button _button;

    [SerializeField] TeamSingleRecordBase _basicStat;
    [SerializeField] List<TeamSingleRecordBase> _statList;
    int _statIndex;
    public int CategoryCount { get { return _statList == null ? 0 : _statList.Count;}}
    [SerializeField] TextMeshProUGUI _diffText, _teamNameText, _rankText;
    [SerializeField] Image IconImage;

    public void Ready(Action<TeamSeason> clickAction) {
        _button = GetComponent<Button>();
        if(_button) {
            _button.onClick.AddListener(() => {
                if(clickAction != null) {
                    clickAction(_team);
                }
            });
        }
    }

    public void SetData(TeamSeason team, int rank = 0) {
        _team = team;
        if(_team != null) {
            if(_basicStat) { _basicStat.SetData(_team); }
            for(int i = 0; i < _statList.Count; i++) {
                _statList[i].SetData(team);
            }
            if(IconImage) {
                IconImage.sprite = ResourcesUtils.GetTeamIconImage(team._team.LogoName);
            }

            if(_teamNameText != null && _rankText != null) {
                _teamNameText.fontSize = _rankText.fontSize;

                _rankText.text = rank.ToString();
            }
        }
    }

    public void SetDiff(float value) {
        if(_diffText) {
            _diffText.text = value.ToString();
        }
    }

    public void SetDetailPage(int index) {
        int _index = (index + 1 >= CategoryCount) ? 0 : index + 1;
        _statIndex = (_index - 1 < 0) ? CategoryCount - 1 : _index - 1;

        for(int i = 0; i < _statList.Count; i++) {
            _statList[i].gameObject.SetActive(_statIndex == i);
        }
        /*
        if (_index != _statIndex) {
            int formerIndex = _statIndex;
            _statIndex = _index;

            if(Utils.IsValidIndex(_statList, _statIndex)) {
                GameObject currentObject = _statList[_statIndex].gameObject;
                if(currentObject) {
                    currentObject.SetActive(true);
                }
            }

            if(Utils.IsValidIndex(_statList, formerIndex)) {
                GameObject formerObject = _statList[formerIndex].gameObject;
                if(formerObject) {
                    formerObject.SetActive(false);
                }
            }
        }*/
    }
}
