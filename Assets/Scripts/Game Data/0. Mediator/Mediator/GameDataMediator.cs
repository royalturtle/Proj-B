using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using UnityEngine;

public class GameDataMediator {
    public GameDataService GameDB { get; private set;}
    
    public TupleGameInfo GameInfo { get; private set; }
    public TurnManager Situation  { get; private set; }

    public TeamDataMediator Teams { get; private set;}

    public BatterDataMediator Batters { get; private set;}
    public PitcherDataMediator Pitchers { get; private set;}
    public LineupDataMediator Lineup { get; private set; }
    public Group2DataMediator Group2 { get; private set;}

    public SeasonDataMediator Seasons { get; private set; }

    public GameDataMediator(bool isCreate = false, string teamName="", string ownerName="") {
        GameDB = new GameDataService(isClear:isCreate);
        
        if(isCreate) {
            GameDataLoader gameDataLoader = new GameDataLoader();
            gameDataLoader.InitGameData(this, teamName, ownerName);
        }
        else {
            SetGameInfo();
            SetTeamsData();
            SetPlayersData();
            SetLineup();
            SetSeasonData();
            SetSituationData();
            SetGroup2Data();
        }
    }
    
    public void SetGameInfo() {
        GameInfo = GameDB.GetGameInfo();
    }

    public void SetTeamsData() {
        Teams = new TeamDataMediator(this);
    }

    public void SetPlayersData() {
        Batters = new BatterDataMediator(this);
        Pitchers = new PitcherDataMediator(this);
    }

    public void SetSituationData() {
        Situation = new TurnManager(this);
    }

    public void SetLineup() {
        Lineup = new LineupDataMediator(this);
    }

    public void SetSeasonData() {
        Seasons = new SeasonDataMediator(this);
    }

    public void SetGroup2Data() {
        Group2 = new Group2DataMediator(this);
    }

    public int MyTeamIndex 
    { get { return (GameInfo == null) ? 0 : GameInfo.MyTeamIndex; } }

    public int CurrentYear
    { get { return (GameInfo == null) ? 0 : GameInfo.Year; } }

    public int CurrentTurn
    { get { return (GameInfo == null) ? 0 : GameInfo.Turn; } }

    public NationTypes LeagueNation
    { get { return (GameInfo == null) ? NationTypes.NONE : GameInfo.LeagueNation; } }

    public int SituationIndex
    { get { return (GameInfo == null) ? 0 : GameInfo.SituationIndex; } }

    public string ManagerName
    { get { return (GameInfo == null) ? "" : GameInfo.OwnerName; }}

    public string MyTeamName
    {get { return (GameInfo == null || Teams == null)  ? "" : Teams.Name(MyTeamIndex); }}

    public int IsAutoSave
    {get { return (GameInfo == null)  ? 0 : GameInfo.IsAutoSave; }}

    public int SaveId
    {get { return (GameInfo == null)  ? 0 : GameInfo.SaveId;}}

    public int Money
    {get { return (GameInfo == null)  ? 0 : GameInfo.Money;}}

    public void UpdateMatchResult(TupleMatch matchData) {
        // 경기 결과만 UPDATE
        GameDB.UpdateData<TupleMatch>(matchData);

        // 경기 결과 팀 순위에 UPDATE
        // HOME과 AWAY팀의 올해 리그 성적 tuple을 가져옴
        TupleLeagueSeason homeTeamSeason = Seasons.GetLeagueSeason(
            teamId : matchData.HomeTeamID, 
            year   : CurrentYear
        );
        TupleLeagueSeason awayTeamSeason = Seasons.GetLeagueSeason(
            teamId : matchData.AwayTeamID,
            year   : CurrentYear
        );

        if(matchData.IsFinished == GameConstants.TRUE_INT) {
            // Home team wins
            if (matchData.HomeScore > matchData.AwayScore) {
                homeTeamSeason.WinCount++;
                awayTeamSeason.LoseCount++;
            }
            // Away team wins
            else if (matchData.HomeScore < matchData.AwayScore) {
                homeTeamSeason.LoseCount++;
                awayTeamSeason.WinCount++;
            }
            // Draw
            else {
                homeTeamSeason.DrawCount++;
                awayTeamSeason.DrawCount++;
            }

            // 최종 Update
            GameDB.UpdateData<TupleLeagueSeason>(homeTeamSeason);
            GameDB.UpdateData<TupleLeagueSeason>(awayTeamSeason);
        }
    }

    public void ReadyNewYear() {
        TupleGameInfo gameInfo = GameDB.GetGameInfo();

        int _currentYear = gameInfo.Year;
        NationTypes leagueType = gameInfo.LeagueNation;
        GameDB.ReadyMatchCalendar(year: _currentYear, leagueType: leagueType);

        // FactoryManager factoryManager = new FactoryManager(leagueType);
        // factoryManager.CreateYoungPlayers(GameDB);
        // factoryManager.CreateMercenaryPlayers(GameDB);
    }

}
