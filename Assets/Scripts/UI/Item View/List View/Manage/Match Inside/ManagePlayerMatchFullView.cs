using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManagePlayerMatchFullView : ManagePlayerBaseFullView {
    [SerializeField] ManageBatterMatchView _batterView;
    [SerializeField] ManagePitcherMatchView _pitcherView;

    bool _isMyTeam = true;
    MatchStatus _matchStatus;
    PlayerStatusInMatch _playerStatus;

    LineupBatter _batterLineup;
    LineupPitcher _pitcherLineup;

    Action _beforeCloseAction;
    
    public void SetData(MatchStatus matchStatus, bool isMyTeam) {
        _matchStatus = matchStatus;
        _isMyTeam = isMyTeam;
        SetViewData();
    }

    public override void BeforeClose() {
        if(_beforeCloseAction != null) {
            _beforeCloseAction();
        }
    }

    void SetViewData() {
        if(_matchStatus != null && _playerStatus != PlayerStatusInMatch.NONE) {
            bool isHomeMyTeam = _playerStatus == PlayerStatusInMatch.HOME;
            TeamDataInMatch teamData = _isMyTeam ?
                (isHomeMyTeam ? _matchStatus.HomeTeamInfo : _matchStatus.AwayTeamInfo) :
                (isHomeMyTeam ? _matchStatus.AwayTeamInfo : _matchStatus.HomeTeamInfo);

            if(teamData != null) {
                if(_iconImage) {
                    _iconImage.sprite = teamData.TeamLogo;
                }

                if(_titleText) {
                    _titleText.text = teamData.RegisteredTeam.Name;
                }

                if(_batterView != null) {
                    _batterView.SetData(lineup:teamData.BatterLineup);
                }
                if(_pitcherView != null) {
                    _pitcherView.SetData(lineup:teamData.PitcherLineup);
                }
            }
        }
    }

    public void SetPlayerStatus(PlayerStatusInMatch playerStatus) {
        _playerStatus = playerStatus;
    }

    public void Ready(
        Action<PlayerBase, bool> viewDetailAction,
        Action beforeCloseAction
    ) {
        _beforeCloseAction = beforeCloseAction;
        
        if(_batterView) { 
            _batterView.Ready(
                viewDetailAction : (PlayerBase player) => {
                    if(viewDetailAction != null) {
                        viewDetailAction(player, true);
                    }
                }
            );
        }
        if(_toPitcherButton) { 
            _toPitcherButton.onClick.RemoveAllListeners();
            _toPitcherButton.onClick.AddListener(() => {SetActive(isBatter:false);});
        }

        if(_pitcherView) { 
            _pitcherView.Ready(
                viewDetailAction  : (PlayerBase player) => {
                    if(viewDetailAction != null) {
                        viewDetailAction(player, false);
                    }
                }
            ); 
        }
        if(_toBatterButton) { 
            _toBatterButton.onClick.RemoveAllListeners();
            _toBatterButton.onClick.AddListener(() => {SetActive(isBatter:true);});
        }
    }

    protected void SetActive(bool isBatter) {
        if(_batterView) {
            _batterView.gameObject.SetActive(isBatter); 
            _batterView.ClearSelected();
        }
        if(_pitcherView) {
            _pitcherView.gameObject.SetActive(!isBatter); 
            _pitcherView.ClearSelected();
        }
    }
}
