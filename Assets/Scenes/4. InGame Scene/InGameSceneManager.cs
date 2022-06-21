using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameSceneManager : SceneBase {
    [SerializeField] InGameTopBar _topBar;

    [SerializeField] ManagePlayerFullView _teamPlayerView;
    [SerializeField] BatterIndividualView _batterIndividualView;
    [SerializeField] PitcherIndividualView _pitcherIndividualView;
    [SerializeField] PlayerRecordFullView _playerRecordView;
    [SerializeField] TeamRecordView _teamRecordView;
    [SerializeField] TradeFullView _tradeView;

    [SerializeField] TeamSelectDialog _teamSelectDialog;
    [SerializeField] ScheduleDialog _scheduleDialog;
    [SerializeField] QuickProgressCalendarDialog _quickCalendarDialog;

    [SerializeField] ProgressButton _progressButton;
    [SerializeField] Button _quickMatchButton;
    [SerializeField] QuickProgressDialog _quickProgressDialog;
    [SerializeField] SelectBatterPositionDialog _selectBatterPositionDialog;
    [SerializeField] SelectPitcherPositionDialog _selectPitcherPositionDialog;
    [SerializeField] ProgressMatchDialog _progressMatchDialog;

    PlayerLineupManageMediator _lineupMediator;
    FullPanelManager _fullPanelManager;

    bool isNewDay;

    protected override void AwakeAfter() {
        _fullPanelManager = new FullPanelManager(
            new Dictionary<InGameScenePanelModes, FullPanel> {
                {InGameScenePanelModes.TeamPlayer, _teamPlayerView},
                {InGameScenePanelModes.TeamOther, _teamPlayerView},
                {InGameScenePanelModes.BatterInvididual, _batterIndividualView},
                {InGameScenePanelModes.PitcherInvididual, _pitcherIndividualView},
                {InGameScenePanelModes.RecordPlayer, _playerRecordView},
                {InGameScenePanelModes.RecordTeam, _teamRecordView},
                {InGameScenePanelModes.Trade, _tradeView},
                {InGameScenePanelModes.GetTradePlayer, _teamPlayerView}
            }
        );

        if(_teamPlayerView) { _teamPlayerView.SetTopBarActions(backAction:_fullPanelManager.PrevPanel, closeAction:_fullPanelManager.ClosePanel); }
        if(_batterIndividualView) { _batterIndividualView.SetTopBarActions(backAction:_fullPanelManager.PrevPanel, closeAction:_fullPanelManager.ClosePanel); }
        if(_pitcherIndividualView) { _pitcherIndividualView.SetTopBarActions(backAction:_fullPanelManager.PrevPanel, closeAction:_fullPanelManager.ClosePanel); }
        if(_playerRecordView) { _playerRecordView.SetTopBarActions(backAction:_fullPanelManager.PrevPanel, closeAction:_fullPanelManager.ClosePanel); }
        if(_teamRecordView) { _teamRecordView.SetTopBarActions(backAction:_fullPanelManager.PrevPanel, closeAction:_fullPanelManager.ClosePanel); }
        if(_tradeView) { _tradeView.SetTopBarActions(backAction:_fullPanelManager.PrevPanel, closeAction:_fullPanelManager.ClosePanel); }
    }

    protected override void StartAfter() {
        if(_notDestroyObj != null && GameData == null) {
            _notDestroyObj.LoadCurrentGame();
        }

        GameDataMediator gameData = GameData;
        NationTypes nation = gameData.LeagueNation;

        _lineupMediator = new PlayerLineupManageMediator(
            gameData                    : gameData,
            teamPlayerView              : _teamPlayerView,
            askDialog                   : _askDialog,
            checkDialog                 : _checkDialog,
            selectBatterPositionDialog  : _selectBatterPositionDialog,
            selectPitcherPositionDialog : _selectPitcherPositionDialog,
            openAskDialog               : OpenAskDialog,
            updateTeamView              : UpdateTeamView
        );

        RegisterButtons();
        if(_topBar) {
            _topBar.Ready(gameData:gameData);
        }
        if(_playerRecordView) { 
            _playerRecordView.Ready(
                gameData : gameData,
                batterItemAction:(PlayerBase player) => {
                    OpenPlayer(player:player, isRecord:true, isBatter:true);
                },
                pitcherItemAction:(PlayerBase player) => {
                    OpenPlayer(player:player, isRecord:true, isBatter:false);
                }
            );
        }
        if(_teamRecordView) {
            _teamRecordView.Ready(clickAction:OpenTeamOther); 
            _teamRecordView.SetData(gameData);
        }
        if(gameData != null) {
            if(_batterIndividualView) {
                _batterIndividualView.Ready(teamData:gameData.Teams);
            }

            if(_pitcherIndividualView) {
                _pitcherIndividualView.Ready(teamData:gameData.Teams);
            }

            if(_teamPlayerView != null && _lineupMediator != null) { 
                _teamPlayerView.Ready(
                    viewDetailAction:(PlayerBase player, bool isBatter) => {OpenPlayer(player, isRecord:false, isBatter:isBatter);},
                    changeGroupAction:ChangeGroup,
                    changePlayerAction:_lineupMediator.ChangePlayer,
                    changePositionAction:_lineupMediator.ChangePosition
                ); 
                _teamPlayerView.SetCurrentTotalText(
                    count:gameData.Lineup.Group1RegisterLimitation,
                    pitcherCount:gameData.Lineup.Group1PitcherLimitation,
                    isTotal:true
                );
            }

            if(gameData.Teams != null) {
                if(_tradeView) {_tradeView.SetData(team: gameData.Teams.MyTeam, isPlayer:true );}
                if(_teamSelectDialog) {
                    _teamSelectDialog.SetData(gameData.Teams.GetTeamList());
                    _teamSelectDialog.SetMyTeamIndex(gameData.MyTeamIndex);
                }
            }
            if(_scheduleDialog) {
                _scheduleDialog.Ready(gameData.Situation);
                _scheduleDialog.SetDataByTurn(
                    year:gameData.CurrentYear,
                    turn:gameData.CurrentTurn
                );
            }
            if(_quickCalendarDialog) {
                _quickCalendarDialog.Ready(gameData.Situation);
                _quickCalendarDialog.SetDataByTurn(
                    year:gameData.CurrentYear,
                    turn:gameData.CurrentTurn
                );
                _quickCalendarDialog.SetConfirmAction(GotoMatchSceneQuickMatch);
            }
            if(_quickProgressDialog) {
                _quickProgressDialog.Ready(
                    confirmAction:GotoMatchSceneQuickMatch,
                    year:gameData.CurrentYear,
                    turn:gameData.CurrentTurn
                );
            }

            if(_progressMatchDialog) {
                _progressMatchDialog.Ready(
                    confirmAction : GotoMatchSceneNormalMatch,
                    viewAction    : OpenTeamOther,
                    getTeamLogo   : gameData.Teams.GetLogo
                );
                _progressMatchDialog.SetData(
                    matchInfo : gameData.Situation.GetPlayerMatch()
                );
            }
            // MatchInfo GetPlayerMatch
        }
    }

    void Update() {
        if(isNewDay) {
            GameDataMediator gameData = GameData;
            isNewDay = false;
            RegisterButtons();
            if(_topBar != null) {
                _topBar.UpdateDate(gameData);
            }
        
            if(_quickCalendarDialog) {
                _quickCalendarDialog.SetDataByTurn(
                    year:gameData.CurrentYear,
                    turn:gameData.CurrentTurn
                );
            }
            if(_scheduleDialog) {
                _scheduleDialog.SetDataByTurn(
                    year:gameData.CurrentYear,
                    turn:gameData.CurrentTurn
                );  
            }

            if(_progressMatchDialog) {
                _progressMatchDialog.SetData(
                    matchInfo : gameData.Situation.GetPlayerMatch()
                );
            }
        }
    }

    #region PlayerActions
    public void OpenTeamPlayer() {
        GameDataMediator gameData = GameData;
        if(Utils.NotNull(_teamPlayerView, gameData) && _fullPanelManager != null) {
            _teamPlayerView.SetData(
                gameData     : gameData,
                teamId       : gameData.MyTeamIndex,
                isManageAble : true
            );
            _fullPanelManager.NextPanel(mode:InGameScenePanelModes.TeamPlayer);
        }
    }

    public void OpenTeamOther(Team team) {
        GameDataMediator gameData = GameData;
        if(Utils.NotNull(gameData, team) && _fullPanelManager != null) {
            _teamPlayerView.SetData(
                gameData     : gameData,
                teamId       : team.ID,
                isManageAble : false
            );
            _fullPanelManager.NextPanel(mode:InGameScenePanelModes.TeamOther, id:team.ID);
        }
    }

    public void OpenTeamOther(TeamSeason team) {
        if(team != null) {
            OpenTeamOther(team._team);
        }
    }

    public void OpenPlayer(PlayerBase player, bool isRecord, bool isBatter) {
        if(player != null && player.Base != null && _fullPanelManager != null) {
            if(isBatter) {
                if(_batterIndividualView) {
                    _batterIndividualView.SetData((Batter)player, isRecord:isRecord);
                }
            }
            else if(_pitcherIndividualView) {
                _pitcherIndividualView.SetData((Pitcher)player, isRecord:isRecord);
            }

            _fullPanelManager.NextPanel(
                mode : isBatter ? InGameScenePanelModes.BatterInvididual : InGameScenePanelModes.PitcherInvididual, 
                id   : player.Base.ID
            );
        }
    }

    public void ChangeGroup(PlayerBase player, bool isBatter) {
        if(isBatter) {
            ChangeBatterGroup((Batter)player);
        }
        else {
            ChangePitcherGroup((Pitcher)player);
        }
    }

    void ChangeBatterGroup(Batter player) {
        GameDataMediator gameData = GameData;
        if(player != null && gameData != null) {
            if(player.Stats.Group == 1) {
                ErrorType errorType = gameData.Lineup.BatterToGroup2(player);
                CheckAndUpdateView(gameData:gameData, teamId:player.Stats.TeamID, errorType:errorType);
            }
            else if(player.Stats.Group == 2) {
                ErrorType errorType = gameData.Lineup.BatterToGroup1(player);
                CheckAndUpdateView(gameData:gameData, teamId:player.Stats.TeamID, errorType:errorType);
            } 
        }
    }

    void ChangePitcherGroup(Pitcher player) {
        GameDataMediator gameData = GameData;
        if(player != null && gameData != null) {
            if(player.Stats.Group == 1) {
                ErrorType errorType = gameData.Lineup.PitcherToGroup2(player);
                CheckAndUpdateView(gameData:gameData, teamId:player.Stats.TeamID, errorType:errorType);
            }
            else if(player.Stats.Group == 2) {
                ErrorType errorType = gameData.Lineup.PitcherToGroup1(player);
                CheckAndUpdateView(gameData:gameData, teamId:player.Stats.TeamID, errorType:errorType);
            } 
        }
    }

    void CheckAndUpdateView(GameDataMediator gameData, int teamId, ErrorType errorType) {
        if(errorType == ErrorType.None) {
            UpdateTeamView(gameData,teamId);
        }
        else if(_checkDialog != null) {
            _checkDialog.Open(errorType);
        }
    }

    void UpdateTeamView(GameDataMediator gameData, int teamId) {
        Debug.Log("UPDATE1");
        if(_teamPlayerView != null) {
            Debug.Log("UPDATE2");
            _teamPlayerView.SetData(
                gameData:gameData,
                teamId:teamId,
                isManageAble:true,
                isUpdate:true
            );
        }
    }

    public void OpenPlayerRecord() {
        if(_playerRecordView && _fullPanelManager != null) {
            _playerRecordView.SetData();
            _fullPanelManager.NextPanel(mode: InGameScenePanelModes.RecordPlayer);
        }
    }

    public void OpenTeamRecord() {
        if(_teamRecordView && _fullPanelManager != null) {
            // _teamRecordView.SetData(GameData);
            _fullPanelManager.NextPanel(mode: InGameScenePanelModes.RecordTeam);
        }
    }

    public void OpenTrade() {
        if(_tradeView && _fullPanelManager != null) {
            _fullPanelManager.NextPanel(mode: InGameScenePanelModes.Trade);
        }
    }

    public void OpenGetTradePlayer(Team team, TradePlayerItem tradeObj) {
        if(_teamPlayerView != null && team != null && _fullPanelManager != null) {
            GameDataMediator gameData = GameData;
            if(_teamPlayerView != null && gameData != null) {
                _teamPlayerView.SetDataTrade(
                    gameData:gameData,
                    teamId:team.ID,
                    tradeObj:tradeObj
                );
            }
            _fullPanelManager.NextPanel(mode:InGameScenePanelModes.GetTradePlayer, id:team.ID);
        }
    }
    #endregion

    #region Progress
    public void GotoNextDay() {
        Debug.Log("EEEEEEEEEEEE");
        DateObj date = new DateObj(year:GameData.CurrentYear, turn:GameData.CurrentTurn);
        TransitionObject.DateString = date.YYYYAMMADDString;
        TransitionSlide(
            threadAction : () => {
                GameDataMediator gameData = GameData;
                if(gameData != null) {
                    gameData.Situation.NextTurn();
                }
            },
            workAction : () => {
                DateObj newDate = new DateObj(year:GameData.CurrentYear, turn:GameData.CurrentTurn);
                TransitionObject.DateString = newDate.YYYYAMMADDString;
                isNewDay = true;
            },
            mode:TransitionModes.THREAD,
            isDate : true
        );
    }
    
    void RegisterButtons() {
        GameDataMediator gameData = GameData;
        if(gameData != null && gameData.Situation != null) {
            bool isMatchExist = gameData.Situation.IsMatchExist;
            if (_progressButton != null) {
                if (isMatchExist) { _progressButton.SetMatchMode(); }
                else { _progressButton.SetNextMode(); }
            }

            if(_quickMatchButton != null) {
                _quickMatchButton.gameObject.SetActive(gameData.Situation.IsMatchExist);
            }
        }
    }
    #endregion

    #region Match
    void GotoMatchScene(bool isQuickMatch, int turnCount = 1) {
        TransitionSlide(
            workAction : () => {
                if(_notDestroyObj != null) {
                    _notDestroyObj.SetSceneArguments(
                        matchArguments : new MatchSceneArguments(
                            isQuickMatch : isQuickMatch, 
                            turnCount    : turnCount
                        )
                    );
                    LoadMatchScene();    
                }
            },
            mode:TransitionModes.COROUTINE
        );

    }

    public void GotoMatchSceneQuickMatch(int turnCount) {
        GotoMatchScene(isQuickMatch: true, turnCount: turnCount);
    }

    public void GotoMatchSceneNormalMatch() {
        GotoMatchScene(isQuickMatch: false);
    }
    #endregion

    public void TestFunction() {
        Debug.Log("TEST");
    }
}

