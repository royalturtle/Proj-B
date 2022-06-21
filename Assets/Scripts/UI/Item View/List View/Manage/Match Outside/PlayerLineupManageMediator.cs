using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLineupManageMediator{
    GameDataMediator GameData;
    ManagePlayerFullView _teamPlayerView;
    AskDialog _askDialog;
    CheckDialog _checkDialog;
    SelectBatterPositionDialog _selectBatterPositionDialog;
    SelectPitcherPositionDialog _selectPitcherPositionDialog;

    Action<string, Action, Action> _openAskDialog;
    Action<GameDataMediator, int>  _updateTeamView;
    
    public PlayerLineupManageMediator(
        GameDataMediator gameData,
        ManagePlayerFullView teamPlayerView,
        AskDialog askDialog,
        CheckDialog checkDialog,
        SelectBatterPositionDialog selectBatterPositionDialog,
        SelectPitcherPositionDialog selectPitcherPositionDialog,
        Action<string, Action, Action> openAskDialog,
        Action<GameDataMediator, int>  updateTeamView
    ) {
        GameData = gameData;
        _teamPlayerView = teamPlayerView;
        _askDialog = askDialog;
        _checkDialog = checkDialog;
        _selectBatterPositionDialog = selectBatterPositionDialog;
        _selectPitcherPositionDialog = selectPitcherPositionDialog;
        _openAskDialog = openAskDialog;
        _updateTeamView = updateTeamView;
    }

    public void ChangePlayer(PlayerBase player1, PlayerBase player2, bool isBatter) {
        if(player1 != null && player2 != null && player1.Base.ID != player2.Base.ID) {
            if(isBatter) { 
                ChangeBatter((Batter)player1, (Batter)player2); 
            }
            else {
                ChangePitcher((Pitcher)player1, (Pitcher)player2); 
            }
        }
        else {
            Debug.Log("ERROR");
        }
    }

    void ChangeBatter(Batter player1, Batter player2) {
        GameDataMediator gameData = GameData;
        if(player1.Stats.Group == 1 && player2.Stats.Group == 1) {
            ChangeBatterAction(player1, player2);
        }
        else if(player1.Stats.Group == 1 && player2.Stats.Group == 2) {
            if(_openAskDialog != null) {
                _openAskDialog(
                    "OK?", 
                    () => ChangeBatterAction(player1, player2),
                    null
                );
            }
            
        }
        else if(player1.Stats.Group == 2 && player2.Stats.Group == 1) {
            if(_openAskDialog != null) {
                _openAskDialog(
                    "OK?", 
                    () => ChangeBatterAction(player1, player2),
                    null
                );
            }
        }
        else if(player1.Stats.Group == 2 && player2.Stats.Group == 2) {
            if(_checkDialog) {
                _checkDialog.Open(ErrorType.Group2Change);
            }
        }
    }

    void ChangeBatterAction(Batter player1, Batter player2) {
        GameDataMediator gameData = GameData;
        gameData.Lineup.ChangeBatter(player1, player2);

        if(_updateTeamView != null) {
            _updateTeamView(gameData, player1.Stats.TeamID);
        }
    }

    void ChangePitcher(Pitcher player1, Pitcher player2) {
        bool isPlayer1Group1 = player1.Stats.Group == 1;
        bool isPlayer2Group1 = player2.Stats.Group == 1;
        if(isPlayer1Group1 && isPlayer2Group1) {
            ChangePitcherAction(player1, player2);
        }
        else if(!isPlayer1Group1 && !isPlayer2Group1) {
            if(_checkDialog) {
                _checkDialog.Open(ErrorType.Group2Change);
            }
        }
        else if(!isPlayer1Group1 || !isPlayer2Group1) {
            if(!GameData.Group2.IsChangeable(player1.Base.ID) || !GameData.Group2.IsChangeable(player2.Base.ID)) {
                if(_checkDialog) {
                    _checkDialog.Open(ErrorType.Group2DayRemains);
                }
            }
            else if(_openAskDialog != null) {
                _openAskDialog(
                    "OK?", 
                    () => ChangePitcherAction(player1, player2),
                    null
                );
            }
        }
    }

    public void ChangePosition(PlayerBase player, int position, bool isBatter) {
        GameDataMediator gameData = GameData;
        if(Utils.NotNull(player, gameData, gameData.Lineup)) {
            if(isBatter) {
                if(_selectBatterPositionDialog) {
                    Batter batter = (Batter)player;
                    _selectBatterPositionDialog.Open(
                        position      : (BatterPositionEnum)position,
                        confirmAction : (BatterPositionEnum newPosition) => {
                            gameData.Lineup.ChangeBatterPosition(batter, newPosition);
                            
                            if(_updateTeamView != null) {
                                _updateTeamView(gameData, batter.Stats.TeamID);
                            }
                        }
                    );
                }
            }
            else {
                if(_selectPitcherPositionDialog) {
                    Pitcher pitcher = (Pitcher)player;
                    _selectPitcherPositionDialog.Open( 
                        position      : (PitcherPositionEnum)position,
                        confirmAction : (PitcherPositionEnum newPosition) => {
                            ErrorType errorType = gameData.Lineup.ChangePitcherPosition(pitcher, newPosition);
                            if(errorType == ErrorType.None) {
                                if(_updateTeamView != null) {
                                    _updateTeamView(gameData, pitcher.Stats.TeamID);
                                }
                            }
                            else if(_checkDialog) {
                                _checkDialog.Open(errorType);
                            }
                        }
                    );
                }
            }
        }
    }

    void ChangePitcherAction(Pitcher player1, Pitcher player2) {
        GameDataMediator gameData = GameData;
        gameData.Lineup.ChangePitcher(player1, player2);
        
        if(_updateTeamView != null) {
            _updateTeamView(gameData, player1.Stats.TeamID);
        }
    }
}
