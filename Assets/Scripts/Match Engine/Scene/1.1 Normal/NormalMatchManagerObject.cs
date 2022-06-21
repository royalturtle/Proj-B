using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NormalMatchManagerObject : MonoBehaviour {
    GameDataMediator GameData;
    [SerializeField] AskDialog _askDialog;

    RunMatchManager runMatchManager;
    bool isCountdown, isAnimation = true;
    float countDownTime;
    int turn;

    NormalMatchModes Mode;

    [SerializeField] MatchUIManager _uiManager;
    [SerializeField] GameObject _askExitPanel;
    [SerializeField] MatchAnimationManager _matchAnimationManager;
    [SerializeField] OutfitManager _outfitManager;
    [SerializeField] Button _progressButton, _changeButton, _settingsButton, _skipButton, _oppButton;
    [SerializeField] MatchResultPanel _resultPanel;

    [SerializeField] ManagePlayerMatchFullView _teamPlayerView;
    [SerializeField] BatterIndividualView _batterIndividualView;
    [SerializeField] PitcherIndividualView _pitcherIndividualView;
    [SerializeField] SelectBatterPositionDialog _selectBatterPositionDialog;

    [SerializeField] SceneBase _scene;

    FullPanelManager _fullPanelManager;

    public void SetData(GameDataMediator gameData) {
        
        _fullPanelManager = new FullPanelManager(
            new Dictionary<InGameScenePanelModes, FullPanel> {
                {InGameScenePanelModes.TeamPlayer, _teamPlayerView},
                {InGameScenePanelModes.TeamOther, _teamPlayerView},
                {InGameScenePanelModes.BatterInvididual, _batterIndividualView},
                {InGameScenePanelModes.PitcherInvididual, _pitcherIndividualView},
            }
        );
        
        if(_progressButton) {
            _progressButton.interactable = false;
            _progressButton.onClick.AddListener(ProgressButtonClicked);
        }
        if(_changeButton) {
            _changeButton.interactable = false;
            _changeButton.onClick.AddListener(OpenTeamPlayer);
        }
        if(_oppButton) {
            _oppButton.interactable = false;
            _oppButton.onClick.AddListener(OpenTeamOther);
        }
        if(_settingsButton) {
            _settingsButton.interactable = false;
        }
        if(_skipButton) {
            _skipButton.interactable = false;
            _skipButton.onClick.AddListener(() => {
                AskSkipMatch();
            });
        }

        if(_teamPlayerView) { _teamPlayerView.SetTopBarActions(backAction:_fullPanelManager.PrevPanel, closeAction:_fullPanelManager.ClosePanel); }
        if(_batterIndividualView) { _batterIndividualView.SetTopBarActions(backAction:_fullPanelManager.PrevPanel, closeAction:_fullPanelManager.ClosePanel); }
        if(_pitcherIndividualView) { _pitcherIndividualView.SetTopBarActions(backAction:_fullPanelManager.PrevPanel, closeAction:_fullPanelManager.ClosePanel); }

        GameData = gameData;
        SetIsCount(false);
        runMatchManager = new RunMatchManager(GameData);
        
        if(_teamPlayerView) { 
            _teamPlayerView.Ready(
                viewDetailAction  : OpenPlayer,
                beforeCloseAction : ReturnToGame
            );
        }
        
        if(_batterIndividualView) {
            _batterIndividualView.Ready(teamData:GameData.Teams);
        }

        if(_pitcherIndividualView) {
            _pitcherIndividualView.Ready(teamData:GameData.Teams);
        }

        MatchInfo matchInfo = GameData.Situation.GetPlayerMatch();
        if(Utils.NotNull(_uiManager, matchInfo, GameData)) {
            _uiManager.SetTeam(
                homeTeamName:matchInfo.HomeTeam.Name, 
                homeTeamSprite:GameData.Teams.GetLogo(matchInfo.HomeTeam.ID),
                awayTeamName:matchInfo.AwayTeam.Name, 
                awayTeamSprite:GameData.Teams.GetLogo(matchInfo.AwayTeam.ID)
            );
        }

        if(matchInfo != null) {
            runMatchManager.RegisterData(matchInfo);
            Mode = NormalMatchModes.MATCH_START;
            
            if(_teamPlayerView != null && runMatchManager.MatchManager != null) {
                _teamPlayerView.SetPlayerStatus(runMatchManager.MatchManager.PlayerStatus);
            }
        }

        /*
        if(_teamPlayerView != null && runMatchManager != null && _fullPanelManager != null) {
            _teamPlayerView.SetData(
                matchStatus : runMatchManager.Status,
                isMyTeam    : true
            );
        }*/
    }

    public void Progress() {
        switch(Mode) {
            case NormalMatchModes.MATCH_START:
                Mode = NormalMatchModes.NONE;
                StartMatch();
                break;
            case NormalMatchModes.MATCH_WAIT:
                // Countdown이 존재하고 0이 아닐 때, Count를 센다.
                if(isCountdown) {
                    if(countDownTime > 0) {
                        countDownTime -= Time.deltaTime;
                    }
                    else {
                        ProgressMatch();
                    }
                }
                break;
            case NormalMatchModes.MATCH_ANIMATION_START:
                Mode = NormalMatchModes.MATCH_ANIMATION_WORKING;
                if(isAnimation) {
                    _matchAnimationManager.DoAnimation(
                        situationsList : runMatchManager.MatchManager.SituationsList,
                        endAction      : delegate{ Mode = NormalMatchModes.MATCH_ANIMATION_FINISH; }
                    );
                }
                else {
                    Mode = NormalMatchModes.MATCH_ANIMATION_FINISH;
                }
                break;
            case NormalMatchModes.MATCH_ANIMATION_FINISH:
                Mode = NormalMatchModes.NONE;
                EndTurn();
                break;
            // 경기 스킵
            case NormalMatchModes.FINISH_ALL:
                Mode = NormalMatchModes.NONE;
                if(runMatchManager != null) {
                    runMatchManager.ProgressEntireMatchThread(endAction:() => {
                        Mode = NormalMatchModes.MATCH_END;
                    });
                }
                break;
            // 경기가 끝나고, 그 결과가 DB에 등록되지 않은 경우
            case NormalMatchModes.MATCH_END:
                Mode = NormalMatchModes.NONE;
                runMatchManager.RegisterMatchResult(endAction:delegate{ Mode = NormalMatchModes.OTHERS_START; });
                
                if(_uiManager != null) {
                    _uiManager.SetDataByStatus(runMatchManager.MatchManager.Status);
                }
                SetResultPanel();
                break;
            // 다른 경기들을 돌린다.
            case NormalMatchModes.OTHERS_START:
                Mode = NormalMatchModes.NONE;
                if(_scene != null) {
                    _scene.TransitionSlide(
                        workAction : () => {
                            OpenResultPanel();
                        },
                        threadAction: () => {
                            runMatchManager.RunOtherMatches(endAction:delegate{ Mode = NormalMatchModes.ALL_END; });
                            GameData.Seasons.ApplyCurrentSeason();
                        },
                        mode : TransitionModes.THREAD
                    );
                }                
                break;
            // 모든 경기가 끝나고 결과를 UI로 보여준다.
            case NormalMatchModes.ALL_END:
                Mode = NormalMatchModes.NONE;
                break;
        }
    }

    void ProgressMatch() {
        Mode = NormalMatchModes.NONE;
        _uiManager.EndReadyAnimation();
        
        SetButtonClickable(false);
        runMatchManager.Progress(endAction:delegate{ Mode = NormalMatchModes.MATCH_ANIMATION_START; });
    }

    void ProgressButtonClicked() {
        switch(Mode) {
            case NormalMatchModes.MATCH_WAIT:
                ProgressMatch();
                break;
            case NormalMatchModes.MATCH_ANIMATION_WORKING:
                if(_matchAnimationManager != null) {
                    _matchAnimationManager.StopAnimation();
                }
                break;
        }
    }

    void StartMatch() {
        if(_uiManager != null) {
            _uiManager.SetDataByStatus(runMatchManager.MatchManager.Status);
        }
        
        ReadyNext();
    }

    void ReadyNext() {
        _uiManager.ReadyNextSituation(
            pitcher: runMatchManager.MatchManager.Status.CurrentPitcher,
            batter: runMatchManager.MatchManager.Status.CurrentBatter,
            batterOrder: runMatchManager.MatchManager.Status.BatterOrder
        );
        
        if(_outfitManager != null) {
            _outfitManager.UpdateBase(runMatchManager.MatchManager.Status);
        }
        ResetCountDown();

        SetButtonClickable(true);
        Mode = NormalMatchModes.MATCH_WAIT;
    }

    void EndTurn() {
        SimulatorResult resultItem = runMatchManager.MatchManager.GetResult();
        bool isMatchEnd = runMatchManager.MatchManager.UpdateResult(resultItem);

        if(_uiManager != null) {
            _uiManager.SetDataByStatus(runMatchManager.MatchManager.Status);
        }

        if(isMatchEnd) {
            Mode = NormalMatchModes.MATCH_END;
        }
        else {
            ReadyNext();
        }
    }

    void ResetCountDown() {
        countDownTime = 3f;
    }

    #region UI
    void SetResultPanel() {
        if(_resultPanel != null && runMatchManager != null) {
            _resultPanel.SetData(runMatchManager.Status);
        }
    }

    void OpenResultPanel() {
        if(_resultPanel != null) {
            _resultPanel.gameObject.SetActive(true);
        }
    }

    public void OpenAskExitPanel() {
        if (_askExitPanel != null && !_askExitPanel.activeSelf) {
            _askExitPanel.SetActive(true);
        }
    }
    #endregion UI

    public void SetIsCount(bool value) {
        isCountdown = value;
        if(_progressButton) {
            _progressButton.interactable = !isCountdown;
        }
    }
    
    #region PlayerActions
    public void OpenTeamPlayer() {
        StopMatch();

        if(_teamPlayerView != null && runMatchManager != null && _fullPanelManager != null) {
            _teamPlayerView.SetData(
                matchStatus : runMatchManager.Status,
                isMyTeam    : true
            );
            _fullPanelManager.NextPanel(mode:InGameScenePanelModes.TeamPlayer);
        }
    }

    public void OpenTeamOther() {
        StopMatch();
        if(_teamPlayerView != null && runMatchManager != null && _fullPanelManager != null) {
            _teamPlayerView.SetData(
                matchStatus  : runMatchManager.Status,
                isMyTeam     : false
            );
            _fullPanelManager.NextPanel(mode:InGameScenePanelModes.TeamOther);
        }
    }

    public void ReturnToGame() {
        if(GameData != null) {
            // ResetCountDown();
            // Mode = NormalMatchModes.MATCH_WAIT;    
            ReadyNext();
        }
    }

    public void OpenPlayer(PlayerBase player, bool isBatter) {
        if(player != null && player.Base != null && _fullPanelManager != null) {
            if(isBatter) {
                if(_batterIndividualView) {
                    _batterIndividualView.SetData((Batter)player, isRecord:true);
                }
            }
            else if(_pitcherIndividualView) {
                _pitcherIndividualView.SetData((Pitcher)player, isRecord:true);
            }

            _fullPanelManager.NextPanel(
                mode : isBatter ? InGameScenePanelModes.BatterInvididual : InGameScenePanelModes.PitcherInvididual, 
                id   : player.Base.ID
            );
        }
    }
    #endregion

    public void AskSkipMatch() {
        if(_askDialog != null) {
            StopMatch();
            _askDialog.Open(
                text : LanguageSingleton.Instance.GetMatchScene(7),
                okAction: () => {
                    Mode = NormalMatchModes.FINISH_ALL;
                },
                closeAction: () => {
                    ReturnToGame();
                }
            );
        }
    }

    void SetButtonClickable(bool value) {
        if(_changeButton) {
            _changeButton.interactable = value;
        }
        if(_settingsButton) {
            _settingsButton.interactable = value;
        }
        if(_skipButton) {
            _skipButton.interactable = value;
        }
        if(_oppButton) {
            _oppButton.interactable = value;
        }
    }

    public void StopMatch() {
        ResetCountDown();
        Mode = NormalMatchModes.MATCH_STOP;
    }
}
