using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ManagePlayerFullView : ManagePlayerBaseFullView {
    [SerializeField] protected ManageBatterView _batterView;
    [SerializeField] protected ManagePitcherView _pitcherView;

    TradePlayerItem _tradeObj;

    public override void BeforeClose() {
        if(_batterView) {
            _batterView.ClearSelected();
        }
        if(_pitcherView) {
            _pitcherView.ClearSelected();
        }
    }

    public void Ready(
        Action<PlayerBase, bool> viewDetailAction, 
        Action<PlayerBase, bool> changeGroupAction, 
        Action<PlayerBase, int, bool> changePositionAction, 
        Action<PlayerBase, PlayerBase, bool> changePlayerAction
    ) {
        if(_batterView) { 
            _batterView.Ready(
                viewDetailAction     : viewDetailAction, 
                changeGroupAction    : changeGroupAction, 
                changePositionAction : changePositionAction,
                changePlayerAction   : changePlayerAction, 
                getTradePlayerAction : SetTradePlayer
            ); 
        }

        if(_toPitcherButton) { 
            _toPitcherButton.onClick.RemoveAllListeners();
            _toPitcherButton.onClick.AddListener(() => {
                SetActive(isBatter:false);
            });
        }

        if(_pitcherView) { 
            _pitcherView.Ready(
                viewDetailAction     : viewDetailAction,
                changeGroupAction    : changeGroupAction, 
                changePositionAction : changePositionAction,
                changePlayerAction   : changePlayerAction, 
                getTradePlayerAction : SetTradePlayer
            ); 
        }
        if(_toBatterButton) { 
            _toBatterButton.onClick.RemoveAllListeners();
            _toBatterButton.onClick.AddListener(() => {
                SetActive(isBatter:true);
            });
        }
    }

    public void SetDataTrade(GameDataMediator gameData, int teamId, TradePlayerItem tradeObj) {
        _tradeObj = tradeObj;
        if(_batterView) {
            _batterView.SetDataTrade(gameData, teamId); 
        }
        if(_pitcherView) { 
            _pitcherView.SetDataTrade(gameData, teamId); 
        }
        SetActive(isBatter:true);

        if(_countText != null) {
            _countText.gameObject.SetActive(false);
        }
    }

    void SetTradePlayer(PlayerBase player) {
        if(_tradeObj != null) {
            _tradeObj.SetData(player);
        }
        if(_backButton) {
            _backButton.onClick.Invoke();
        }
    }

    public void SetData(GameDataMediator gameData, int teamId, bool isManageAble = false, bool isUpdate = false) {
        if(gameData != null) {
            Team team = gameData.Teams.TeamDict[teamId];

            if(team != null) {
                if(_titleText) {_titleText.text = team.Name;}
                if(_iconImage) {_iconImage.sprite  = ResourcesUtils.GetTeamIconImage(team.LogoName);}
            }

            if(_batterView) { 
                if(isManageAble) { 
                    _batterView.SetDataManage(gameData, teamId, isUpdate); 
                }
                else {
                    _batterView.SetDataView(gameData, teamId, isUpdate); 
                }
            }
            if(_pitcherView) { 
                if(isManageAble) {
                    _pitcherView.SetDataManage(gameData, teamId, isUpdate); 
                }
                else {
                    _pitcherView.SetDataView(gameData, teamId, isUpdate);
                }
            }

            if(!isUpdate) {
                SetActive(isBatter:true);
            }

            if(isManageAble) {
                _countText.gameObject.SetActive(true);
                if(_countText != null) {
                    (int batterCount, int pitcherCount) = gameData.Lineup.GetMyGroup1Count();
                    SetCurrentTotalText(count:batterCount+pitcherCount, pitcherCount:pitcherCount, isTotal:false);
                }
            }

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
