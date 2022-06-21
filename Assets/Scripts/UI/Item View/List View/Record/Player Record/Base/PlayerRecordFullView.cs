using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerRecordFullView : FullPanel {
    GameDataMediator GameData;
    Dictionary<int, int> _currentYearGameCount;

    NationTypes _nation;
    int _currentYear, _minYear, _maxYear;

    [SerializeField] BatterRecordView _batterView;
    [SerializeField] PitcherRecordView _pitcherView;
    [SerializeField] Button _toBatterButton, _toPitcherButton;

    public void Ready(
        GameDataMediator gameData, 
        Action<PlayerBase> batterItemAction,
        Action<PlayerBase> pitcherItemAction
    ) {
        GameData = gameData;
        if(GameData != null) {
            _nation = GameData.LeagueNation;
            _maxYear = GameData.CurrentYear;
            _minYear = _maxYear;
            _currentYearGameCount = GameData.Seasons.GetGameCountOfCurrentYear();

            
            if(_batterView) { 
                _batterView.Ready(
                    itemAction:batterItemAction, 
                    nation:GameData.LeagueNation, 
                    getGameCountAction : GetTeamGameCount, 
                    getSpriteAction:GameData.Teams.GetLogo
                ); 
            }

            if(_toPitcherButton) { 
                _toPitcherButton.onClick.RemoveAllListeners();
                _toPitcherButton.onClick.AddListener(() => {SetActive(isBatter:false);});
            }

            if(_pitcherView) {
                _pitcherView.Ready(
                    itemAction:pitcherItemAction,
                    nation:GameData.LeagueNation,
                    getGameCountAction : GetTeamGameCount,
                    getSpriteAction:GameData.Teams.GetLogo
                );
            }

            if(_toBatterButton) { 
                _toBatterButton.onClick.RemoveAllListeners();
                _toBatterButton.onClick.AddListener(() => {SetActive(isBatter:true);});
            }
        }
    }

    int GetTeamGameCount(int teamId) {
        int result = 500;
        if(_currentYear >= _maxYear) {
            if(_currentYearGameCount != null && _currentYearGameCount.ContainsKey(teamId)) {
                result = _currentYearGameCount[teamId];
            }
        }
        else {
            switch(_nation) {
                case NationTypes.KOREA:
                    result = 144;
                    break;
                case NationTypes.USA:
                    result = 162;
                    break;
                case NationTypes.JAPAN:
                    result = 143;
                    break;
            }
        }
        return result;
    }

    public void SetData(int year = -1, bool isUpdate = true) {
        if(GameData != null) {
            if(year == GameConstants.NULL_INT || year >= _maxYear) {
                _currentYear = _maxYear;
                if(_batterView) {
                    _batterView.SetData(GameData.Batters.DataList);
                }
                if(_pitcherView) {
                    _pitcherView.SetData(GameData.Pitchers.DataList);
                }
            }
            else if(_minYear <= year) {
                _currentYear = year;
            }
            
            if(isUpdate) {
                SetActive(isBatter:true);
            }
        }
    }

    void SetActive(bool isBatter) {
        if(_batterView) {
            _batterView.gameObject.SetActive(isBatter);
        }

        if(_pitcherView) {
            _pitcherView.gameObject.SetActive(!isBatter);
        }
    }
}
