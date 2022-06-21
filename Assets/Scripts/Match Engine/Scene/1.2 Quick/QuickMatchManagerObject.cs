using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class QuickMatchManagerObject : MonoBehaviour {
    // 기본 자료
    GameDataMediator GameData;
    MatchSceneArguments _matchSceneArguments;
    QuickMatchModes _mode = QuickMatchModes.NONE;
    bool _isModeRunned = true;

    List<List<TeamSeason>> _teamList;
    TeamSeason _myTeam;

    // UI 관련
    [SerializeField] QuickMatchResultPanel _quickMatchPanel;

    // 경기 시뮬레이션 관련
    RunMatchManager runMatchManager;

    // 경지 진행 관련
    int turnCount = 0;

    MatchScore scoreData;
    PlayerStatusInMatch playerStatus;

    [SerializeField] AskDialog _askDialog;
 
    FullPanelManager _fullPanelManager;
    [SerializeField] ManagePlayerFullView _teamPlayerView;
    [SerializeField] BatterIndividualView _batterIndividualView;
    [SerializeField] PitcherIndividualView _pitcherIndividualView;
    [SerializeField] SelectBatterPositionDialog _selectBatterPositionDialog;
    [SerializeField] SelectPitcherPositionDialog _selectPitcherPositionDialog;
    [SerializeField] CheckDialog _checkDialog;

    PlayerLineupManageMediator _lineupMediator;

    public void SetData(GameDataMediator gameData, MatchSceneArguments sceneArguments) {
        _fullPanelManager = new FullPanelManager(
            new Dictionary<InGameScenePanelModes, FullPanel> {
                {InGameScenePanelModes.TeamPlayer, _teamPlayerView},
                {InGameScenePanelModes.TeamOther, _teamPlayerView},
                {InGameScenePanelModes.BatterInvididual, _batterIndividualView},
                {InGameScenePanelModes.PitcherInvididual, _pitcherIndividualView},
            }
        );

        if(_teamPlayerView) { _teamPlayerView.SetTopBarActions(backAction:_fullPanelManager.PrevPanel, closeAction:_fullPanelManager.ClosePanel); }
        if(_batterIndividualView) { _batterIndividualView.SetTopBarActions(backAction:_fullPanelManager.PrevPanel, closeAction:_fullPanelManager.ClosePanel); }
        if(_pitcherIndividualView) { _pitcherIndividualView.SetTopBarActions(backAction:_fullPanelManager.PrevPanel, closeAction:_fullPanelManager.ClosePanel); }

        // 기본 자료 등록
        GameData = gameData;
        _matchSceneArguments = sceneArguments;
        runMatchManager = new RunMatchManager(GameData);
        int myTeamIndex = GameData.MyTeamIndex;
        
        if(_batterIndividualView) {
            _batterIndividualView.Ready(teamData:GameData.Teams);
        }

        if(_pitcherIndividualView) {
            _pitcherIndividualView.Ready(teamData:GameData.Teams);
        }

        // 팀 정보 등록
        _myTeam = null;
        _teamList = GameData.Seasons.GetSeaonList();
        for(int i = 0; _myTeam == null && i < _teamList.Count; i++) {
            for(int j = 0; _myTeam == null && j < _teamList[i].Count; j++) {
                if(_teamList[i][j] != null && _teamList[i][j]._team != null && _teamList[i][j]._team.ID == myTeamIndex) {
                    _myTeam = _teamList[i][j];
                }
            }
        }

        // UI 창 등록
        if (_quickMatchPanel != null && GameData != null) {
            _quickMatchPanel.SetData(
                quickMatchList: GameData.Situation.GetPlayerMatchAfter(
                    turnAfter:_matchSceneArguments.TurnCount
                )
            );

            _quickMatchPanel.Ready(
                getModeAction     : GetMode,
                setModeAction     : SetMode,
                myTeam            : _myTeam,
                getTeamLogoAction : GameData.Teams.GetLogo,
                openTeamAction    : OpenTeamPlayer
            );
            _quickMatchPanel.NextMatchUIChange(value: turnCount);
            _quickMatchPanel.gameObject.SetActive(true);
        }
        
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

        if(gameData != null) {
            if(_teamPlayerView != null && _lineupMediator != null) { 
                _teamPlayerView.Ready(
                    viewDetailAction     : (PlayerBase player, bool isBatter) => {OpenPlayer(player, isBatter:isBatter);},
                    changeGroupAction    : ChangeGroup,
                    changePlayerAction   : _lineupMediator.ChangePlayer,
                    changePositionAction : _lineupMediator.ChangePosition
                );

                _teamPlayerView.SetCurrentTotalText(
                    count        : gameData.Lineup.Group1RegisterLimitation,
                    pitcherCount : gameData.Lineup.Group1PitcherLimitation,
                    isTotal      : true
                );
            }
        }

        
        if(_quickMatchPanel != null) {
            _quickMatchPanel.SetMode(QuickMatchModes.STARTING);
        }

        StartCoroutine(StartingWork());
    }

    IEnumerator StartingWork() {
        yield return StartCoroutine(Wait());
        SetMode(QuickMatchModes.STARTING);
        yield return null;
    }

    public void Progress() {
        if(_isModeRunned) { return ; }
        switch(_mode) {
            case QuickMatchModes.STARTING:
                _isModeRunned = true;
                UpdatePanel();
                if(turnCount <= _matchSceneArguments.TurnCount - 1 && _quickMatchPanel != null) {
                    _quickMatchPanel.NextMatchUIChange(turnCount);
                }
                RunAllQuickMatches();
                break;
            case QuickMatchModes.TURN_FINISHED:
            case QuickMatchModes.PAUSED:
                _isModeRunned = true;
                // 경기 결과를 UI에 표시
                if (scoreData != null) {
                    _quickMatchPanel.SetCurrentMatchResultScore(
                        homeScore    : scoreData.GetScore(isHome:true),
                        awayScore    : scoreData.GetScore(isHome:false),
                        playerStatus : playerStatus
                    );
                }

                if(GameData != null && GameData.Seasons != null && _myTeam != null && _myTeam._team != null) {
                    if(_quickMatchPanel != null) {
                        _quickMatchPanel.SetLeagueSeason(GameData.Seasons.GetLeagueSeason(teamId:_myTeam._team.ID));
                    }
                }

                if(_quickMatchPanel) {
                    _quickMatchPanel.TurnOff(turnCount);
                }
                
                if(++turnCount >= _matchSceneArguments.TurnCount) {
                    SetMode(QuickMatchModes.ALL_FINISHED);
                }
                else if(_mode != QuickMatchModes.PAUSED) {
                    SetMode(QuickMatchModes.STARTING);
                }
                else {
                    UpdatePanel();
                }
                break;
            case QuickMatchModes.ALL_FINISHED:
                _isModeRunned = true;
                UpdatePanel();
                break;
            case QuickMatchModes.RUNNING:
            case QuickMatchModes.PAUSING:
                _isModeRunned = true;
                UpdatePanel();
                break;
        }
    }

    QuickMatchModes GetMode() {
        return _mode;
    }

    void SetMode(QuickMatchModes mode) {
        _mode = mode;
        _isModeRunned = false;
        Debug.Log("ModeChanged " + mode.ToString());
    }

    void UpdatePanel() {
        if(_quickMatchPanel != null) {
            _quickMatchPanel.SetMode(_mode);
        }
    }

    void RunAllQuickMatches() {
        Thread thread = new Thread(delegate() {
            // [Player]의 Match 진행
            MatchInfo matchInfo = GameData.Situation.GetPlayerMatch();

            // When No Player Match
            if(matchInfo != null) {
                runMatchManager.RegisterData(matchInfo);
                runMatchManager.ProgressEntireMatch();

                // 경기 결과 등록
                if (_quickMatchPanel != null) {
                    scoreData = runMatchManager.MatchManager.Status.Score;
                    playerStatus = runMatchManager.MatchManager.PlayerStatus;
                }
            }

            // [Player] 이외의 Match들을 진행
            runMatchManager.RunOtherMatches();
            GameData.Seasons.ApplyCurrentSeason();

            // 날짜 넘기기
            runMatchManager.NextTurn();
            if(_mode == QuickMatchModes.PAUSING) {
                SetMode(QuickMatchModes.PAUSED);
            }
            else {
                SetMode(QuickMatchModes.TURN_FINISHED);
            }
        });
        thread.Start();
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

    void OpenPlayer(PlayerBase player, bool isBatter) {
        if(player != null && player.Base != null && _fullPanelManager != null) {
            if(isBatter) {
                if(_batterIndividualView) {
                    _batterIndividualView.SetData((Batter)player, isRecord:false);
                }
            }
            else if(_pitcherIndividualView) {
                _pitcherIndividualView.SetData((Pitcher)player, isRecord:false);
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
        if(_teamPlayerView != null) {
            _teamPlayerView.SetData(
                gameData     : gameData,
                teamId       : teamId,
                isManageAble : true,
                isUpdate     : true
            );
        }
    }
    #endregion

    public void OpenAskDialog(string text, Action okAction=null, Action closeAction=null) {
        if(_askDialog) {
            _askDialog.Open(
                text        : text, 
                okAction    : okAction,
                closeAction : closeAction
            );
        }
    }

    IEnumerator Wait(int time = 2) {
        yield return new WaitForSeconds(time);
    }
}
