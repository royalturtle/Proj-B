using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class QuickMatchResultPanel : MonoBehaviour {
    [SerializeField] BasePager _pageManager;
    [SerializeField] Button _recordsBtn, _lobbyBtn, _settingBtn;
    [SerializeField] QuickMatchPlayButton _playPauseBtn;
    [SerializeField] List<QuickMatchResultItem> _resultItemList;

    [SerializeField] TeamSingleRecordBasic _basicRecord;
    [SerializeField] TeamSingleRecordRank _standingRecord;

    List<QuickMatchData>[] QuickMatchList;
    const int ShowItemCount = 10;
    int _currentSelectedIndex = 0;
    
    // Quick Match Result
    [SerializeField] ExternalText _winCountText, _drawCountText, _loseCountText;
    int _winCount = 0, _drawCount = 0, _loseCount = 0;

    int QuickMatchCount {
        get {
            int result = 0;
            if(QuickMatchList != null) {
                for(int i = 0; i < QuickMatchList.Length; i++) {
                    if(QuickMatchList[i] != null) {
                        result += QuickMatchList[i].Count;
                    }
                }
            }
            return result;
        }
    }

    public void Ready(
        Func<QuickMatchModes> getModeAction,
        Action<QuickMatchModes> setModeAction,
        TeamSeason myTeam, 
        Func<int, Sprite> getTeamLogoAction,
        Action openTeamAction
    ) {
        if(_playPauseBtn) {
            _playPauseBtn.Ready(
                getModeAction : getModeAction,
                setModeAction : setModeAction
            );
        }

        if(_resultItemList != null) {
            for(int i = 0; i < _resultItemList.Count; i++) {
                if(_resultItemList[i] != null) {
                    _resultItemList[i].Ready(getTeamLogoAction:getTeamLogoAction);
                }
            }
        }

        if(myTeam != null) {
            if(_basicRecord != null && getTeamLogoAction != null) {
                _basicRecord.SetTeamData(
                    name:myTeam._team.Name,
                    sprite:getTeamLogoAction(myTeam._team.ID)
                );
            
                _basicRecord.SetLeagueSeason(myTeam._season);
            }
            if(_standingRecord != null) {
                _standingRecord.SetData(team:myTeam);
            }
        }

        if(_recordsBtn != null) {
            _recordsBtn.onClick.AddListener(() => {
                if(openTeamAction != null) {
                    openTeamAction();
                }
            });
        }
    }

    public void SetLeagueSeason(TupleLeagueSeason leagueSeason) {
        if(leagueSeason != null) {
            if(_basicRecord != null) {
                _basicRecord.SetLeagueSeason(leagueSeason);
            }

            if(_standingRecord != null) {
                _standingRecord.SetLeagueSeason(leagueSeason);
            }
        }
    }

    public void SetMode(QuickMatchModes mode) {
        bool isClickable = mode == QuickMatchModes.PAUSED || mode == QuickMatchModes.ALL_FINISHED;
            
        if(_playPauseBtn != null) {
            _playPauseBtn.SetMode(mode);
        }
        if(_recordsBtn != null) {
            _recordsBtn.interactable = isClickable; 
        }
        if(_settingBtn != null) {
            _settingBtn.interactable = isClickable; 
        }
        if(_lobbyBtn != null) {
            _lobbyBtn.interactable = isClickable; 
        }
        if(_pageManager != null) {
            _pageManager.SetInteractable(isClickable);
        } 
    }

    public void SetData(List<QuickMatchData> quickMatchList) {
        int listCount = (quickMatchList.Count > 0) ? (quickMatchList.Count - 1) / ShowItemCount + 1 : 1;

        QuickMatchList = new List<QuickMatchData>[listCount];

        for (int i = 0; i < listCount; i++) {
            if (i == listCount - 1) {
                QuickMatchList[i] = quickMatchList.GetRange(i * ShowItemCount, quickMatchList.Count - i * ShowItemCount);
            }
            else {
                QuickMatchList[i] = quickMatchList.GetRange(i * ShowItemCount, ShowItemCount);
            }
        }

        if (_pageManager != null) { 
            _pageManager.PageChanged += SetQuickMatchItemInViewList;
            _pageManager.InitObject(totalPage: listCount);
        }
    }

    void SetQuickMatchItemInViewList(int page) {
        List<QuickMatchData> matches = QuickMatchList[page];
        int i = 0;
        for (; i < matches.Count; i++) {
            _resultItemList[i].SetData(matches[i]);
        }
        for (; i < ShowItemCount; i++) {
            _resultItemList[i].SetData(quickMatchData:null);
        }
    }

    public void NextMatchUIChange(int value) {
        int _value = (value < 0) ? 0 : value;
        _value = (_value >= QuickMatchCount) ? QuickMatchCount - 1 : _value;

        int page = _value / ShowItemCount;

        // 페이지 설정
        if (_pageManager != null) {
            _pageManager.SetPage(page : page, isForce : true);
        }

        ChangeSelected(value: value);
    }

    void ChangeSelected(int value) {
        int _value = (value < 0) ? 0 : value % ShowItemCount;

        _resultItemList[_value].ChangedSelected(isSelected: true);
        _resultItemList[_value].SetAnimator(true);
        _currentSelectedIndex = _value;
    }

    public void TurnOff(int index) {
        if(index >= 0) {
            index = index % ShowItemCount;

            _resultItemList[index].ChangedSelected(isSelected: false);
            _resultItemList[index].SetAnimator(false);
        }
    }

    public void SetCurrentMatchResultScore(int homeScore, int awayScore, PlayerStatusInMatch playerStatus) {
        if(playerStatus != PlayerStatusInMatch.NONE) {
            QuickMatchResultItem currentItem = _resultItemList[_currentSelectedIndex];

            int _index = Utils.GetSafeIndex(
                value : _currentSelectedIndex, 
                count : QuickMatchList[_pageManager.CurrentPage].Count, 
                min   : 0
            );

            if(QuickMatchList[_pageManager.CurrentPage][_currentSelectedIndex] == null) {
                Debug.Log("Quickmatchis null");
            }
            QuickMatchList[_pageManager.CurrentPage][_currentSelectedIndex].SetScore(homeScore: homeScore, awayScore: awayScore);

            int matchResult = currentItem.SetScore(homeScore: homeScore, awayScore: awayScore, playerStatus: playerStatus);

            if(!_resultItemList[_currentSelectedIndex].IsEmpty) {
                if(matchResult > 0) {
                    _winCount++;
                    if(_winCountText) { 
                        _winCountText.SetData(_winCount.ToString()); 
                    }
                }
                else if(matchResult < 0) {
                    _loseCount++;
                    if(_loseCountText) { 
                        _loseCountText.SetData(_loseCount.ToString()); 
                    }
                }
                else {
                    _drawCount++;
                    if(_drawCountText) { 
                        _drawCountText.SetData(_drawCount.ToString()); 
                    }
                }
            }
        }
    }
    
    public void CheckScroll(BaseEventData eventData) {
        if(_pageManager != null && eventData != null && eventData is PointerEventData) {
            Vector2 vec = ((PointerEventData)eventData).delta;

            if(vec.y < 0.0f) {
                _pageManager.PreviousPage();
            }
            else if(vec.y > 0.0f) {
                _pageManager.NextPage();
            }
        }
    }
}
