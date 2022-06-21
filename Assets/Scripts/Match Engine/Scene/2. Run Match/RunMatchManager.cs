using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunMatchManager {
    GameDataMediator GameData;
    public MatchSimulator MatchManager {get; private set;}
    
    public MatchStatus Status {
        get { return MatchManager == null ? null : MatchManager.Status; } 
    }

    public RunMatchManager(GameDataMediator gameData) {
        GameData = gameData;
    }

    public void RegisterData(MatchInfo matchInfo) {
        TupleGameInfo gameInfo = GameData.GameInfo;

        // Information about HOME Team
        TeamDataInMatch homeTeam = new TeamDataInMatch(
            team          : matchInfo.HomeTeam,
            batterLineup  : GameData.Lineup.GetLineupBatter(matchInfo.MatchData.HomeTeamID),
            pitcherLineup : GameData.Lineup.GetLineupPitcher(matchInfo.MatchData.HomeTeamID),
            teamLogo      : GameData.Teams.GetLogo(teamId:matchInfo.HomeTeam.ID)
        );

        // Information about AWAY Team
        TeamDataInMatch awayTeam = new TeamDataInMatch(
            team          : matchInfo.AwayTeam,
            batterLineup  : GameData.Lineup.GetLineupBatter(matchInfo.MatchData.AwayTeamID),
            pitcherLineup : GameData.Lineup.GetLineupPitcher(matchInfo.MatchData.AwayTeamID),
            teamLogo      : GameData.Teams.GetLogo(teamId:matchInfo.AwayTeam.ID)
        );

        MatchManager = new MatchSimulator(
            leagueType   : gameInfo.LeagueNation,
            matchData    : matchInfo.MatchData,
            playerStatus : matchInfo.PlayerStatus,
            homeTeamInfo : homeTeam,
            awayTeamInfo : awayTeam
        );
    }

    public void Progress(Action endAction=null) {
        MatchManager.Progress();
        if(endAction != null) { 
            endAction(); 
        }
    }

    public void ProgressEntireMatch() {
        bool isMatchEnd = false;
        while(!isMatchEnd) {
            MatchManager.Progress();
            SimulatorResult resultItem = MatchManager.GetResult();
            isMatchEnd = MatchManager.UpdateResult(result: resultItem);
        }

        isMatchEnd = true;
        RegisterMatchResult();
    }

    public void ProgressEntireMatchThread(Action endAction) {
        Thread thread = new Thread(delegate() {
            bool isMatchEnd = false;
            while(!isMatchEnd) {
                MatchManager.Progress();
                SimulatorResult resultItem = MatchManager.GetResult();
                isMatchEnd = MatchManager.UpdateResult(result: resultItem);
            }
            if(endAction != null) {
                endAction();
            }
        });
        thread.Start();
    }

    public void RunOtherMatches(Action endAction=null) {
        // Get the information about the others matches
        List<MatchInfo> matchInfoList = GameData.Situation.GetOthersMatches();
        
        // Run all matches.
        for(int i = 0; i < matchInfoList.Count; i++) {
            // Register data.
            RegisterData(matchInfoList[i]);

            // Progress the match.
            ProgressEntireMatch();
        }
        if(endAction != null) { 
            endAction(); 
        }
    }

    public void RegisterMatchResult(Action endAction=null) {
        // Get the result of the match.
        TupleMatch matchData = MatchManager.GetFinishedMatch();
        GameData.UpdateMatchResult(matchData);

        // Change the starting order of the matches.
        GameData.Lineup.SetStartingOrder(teamId: matchData.HomeTeamID, isNext: true);
        GameData.Lineup.SetStartingOrder(teamId: matchData.AwayTeamID, isNext: true);

        // Get all batters who played the match, and register the new record.
        List<Batter> batterUpdateList = MatchManager.Status.UpdateNeedBatters;
        for(int i = 0; i < batterUpdateList.Count; i++) {
            GameData.GameDB.UpdateData(batterUpdateList[i].Season);
        }

        // Get all pitchers who played the match, and register the new record.
        List<Pitcher> pitcherUpdateList = MatchManager.Status.UpdateNeedPitchers;
        for(int i = 0; i < pitcherUpdateList.Count; i++) {
            GameData.GameDB.UpdateData(pitcherUpdateList[i].Season);
        }

        if(endAction != null) {
            endAction();
        }
    }

    public void NextTurn() {
        GameData.Situation.NextTurn();
    }
}
