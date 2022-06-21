// https://goodear.tistory.com/14
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using UnityEngine;
using SQLite4Unity3d;

public class GameDataLoader {
    public void InitGameData(
        GameDataMediator gameData, 
        string teamName, 
        string ownerName, 
        NationTypes leagueType = NationTypes.KOREA
    ) {
        // Create Teams
        int teamIndex = CreateTeams(gameData, leagueType, teamName);

        // Create Basic Info
        CreateGameInfo(gameData, teamIndex: teamIndex, leagueType:leagueType, ownerName:ownerName);
        TransitionObject.SetPercentage(1.0);
        gameData.SetGameInfo();
        gameData.SetTeamsData();

        // Create Players
        CreatePitcherSeason(gameData);
        TransitionObject.SetPercentage(2.0);
        CreateBatterSeason(gameData);
        TransitionObject.SetPercentage(3.0);
        CreateLineupGroup2(gameData);
        TransitionObject.SetPercentage(4.0);
        CreateStartSeason(gameData, isCreate:true);
        TransitionObject.SetPercentage(6.0);
        CreatePlayers(gameData, leagueType, startPercentage:6.0, endPercentage:50.0);
        TransitionObject.SetPercentage(50.0);
        ReadyPlayersNewSeaon(gameData);
        gameData.SetPlayersData();
        TransitionObject.SetPercentage(55.0);

        // Create Lineup
        CreateLineup(gameData);
        TransitionObject.SetPercentage(65.0);
        gameData.SetLineup();
        TransitionObject.SetPercentage(70.0);

        // Create Situations
        CreateMatch(gameData);
        TransitionObject.SetPercentage(72.0);
        CreateTurn(gameData);
        TransitionObject.SetPercentage(74.0);
        CreateSituation(gameData);
        TransitionObject.SetPercentage(76.0);
        CreateSpecialPlayerInfo(gameData);
        TransitionObject.SetPercentage(80.0);
        gameData.SetSituationData();
        TransitionObject.SetPercentage(82.0);
        gameData.SetGroup2Data();
        TransitionObject.SetPercentage(84.0);
        // CreateViews(gameData);

        // [TODO]
        gameData.ReadyNewYear();
        TransitionObject.SetPercentage(97.0);

        // Create Season Records
        gameData.SetSeasonData();
        TransitionObject.SetPercentage(100.0);
    }

    public bool NewYear(GameDataMediator gameData) {
        if(gameData == null) { return false;}

        TupleGameInfo gameInfo = gameData.GameInfo;

        if(gameInfo == null) { return false;}

        DateObj startDate = new DateObj(year: gameInfo.Year + 1, month: 2, day: 1);
        gameInfo.Year = gameInfo.Year + 1;
        gameInfo.Turn = startDate.Turn;
        gameInfo.SituationIndex = 0;
        gameData.GameDB.UpdateData(gameInfo);

        gameData.SetGameInfo();
        
        CreateStartSeason(gameData, isCreate:false);
        ReadyPlayersNewSeaon(gameData);
        gameData.SetPlayersData();

        // 라인업
        CreateLineup(gameData);
        gameData.SetLineup();
        CreateMatch(gameData);
        CreateTurn(gameData);
        CreateSituation(gameData);
        // CreateSpecialPlayerInfo
        gameData.SetSituationData();
        CreateLineupGroup2(gameData);
        gameData.SetGroup2Data();
        gameData.ReadyNewYear();
        gameData.SetSeasonData();
        return true;
    }

    public void ReadyPlayersNewSeaon(GameDataMediator gameData) {
        int year = gameData.CurrentYear;
        List<TupleBatterStats> batterList = gameData.GameDB.GetAllBattersStat();
        for(int i = 0; i < batterList.Count; i++) {
            gameData.GameDB.Insert(new TupleBatterSeason {Year = year, TeamID = batterList[i].TeamID, PlayerID = batterList[i].PlayerID});
        }

        List<TuplePitcherStats> pitcherList = gameData.GameDB.GetAllPitchersStat();
        for(int i = 0; i < pitcherList.Count; i++) {
            gameData.GameDB.Insert(new TuplePitcherSeason {Year = year, TeamID = pitcherList[i].TeamID, PlayerID = pitcherList[i].PlayerID});
        }
    }

    void CreateGameInfo(
        GameDataMediator gameData, 
        int teamIndex, 
        NationTypes leagueType, 
        string ownerName
    ) {
        // Create a table.
        gameData.GameDB.DropTable<TupleGameInfo>();
        gameData.GameDB.CreateTable<TupleGameInfo>();

        // Get the start date.
        DateObj startDate = new DateObj(year: GameConstants.GAME_START_YEAR, month: 2, day: 1);

        TupleGameInfo result = new TupleGameInfo {
            GameMajorVersion = 1,
            Year = GameConstants.GAME_START_YEAR,
            Turn = startDate.Turn,
            OwnerName = ownerName,
            LeagueNation = leagueType,
            MyTeamIndex = teamIndex,
            Money = GameConstants.START_MONEY,
            IsAutoSave = 0,
            SituationIndex = 0
        };

        // Create new tuple.
        gameData.GameDB.Insert(result);
    }

    // Create Teams
    // Return : index of player's team.
    int CreateTeams(GameDataMediator gameData, NationTypes leagueType, string teamName) {
        System.Random randomCreator = new System.Random();
        // Create a table.
        gameData.GameDB.DropTable<Team>();
        gameData.GameDB.CreateTable<Team>();

        // Ready Variables
        List<string> teamNames;
        int nameIndex = 0;

        // Get Team Logo ID List
        List<int> teamLogo = new List<int>();
        for(int i = 1; i <= GameConstants.RESOURCE_LOGO_COUNT; i++) {
            teamLogo.Add(i);
        }
        teamLogo = Utils.ShuffleList(teamLogo);

        // Set the league rule by the nation of the league.
        int div1 = 1, div2 = 1, eachTeamCount;

        if(leagueType == NationTypes.USA) {
            div1 = 2; div2 = 3; eachTeamCount = 5; 
        }
        else if(leagueType == NationTypes.JAPAN) {
            div1 = 2; div2 = 6; eachTeamCount = 6; 
        }
        else {
            div1 = 1; div2 = 1; eachTeamCount = 10; 
        }
        int teamCount = div1 * div2 * eachTeamCount;

        // Get the list of teams' name.
        teamNames = GetTeamNames(teamCount - 1);
        int random = randomCreator.Next(0, div1 * div2);
        teamNames.Insert(random * 5, teamName);

        int teamLogoCounter = 0;
        // Create all teams.
        for(int i = 0; i < div1; i++) {
            for(int j = 0; j < div2; j++) {
                for(int k = 0; k < eachTeamCount; k++) {
                    gameData.GameDB.Insert(new Team {
                            Name = teamNames[nameIndex++],
                            LogoName = "b" + teamLogo[teamLogoCounter++].ToString(),
                            Div1 = i,
                            Div2 = j,
                            StartingOrder = 1
                        });
                }
            }
        }
        // Return : index of player's team.
        return random + 1;
    }

    List<string> GetTeamNames(int count) {
        System.Random randomCreator = new System.Random();
        string[] teamNames = new string[] { "Team1", "Team2", "Team3", "Team4", "Team5", "Team6", "Team7", "Team8", "Team9", "Team10", 
                "Team11", "Team12", "Team13", "Team14", "Team15", "Team16", "Team17", "Team18", "Team19", "Team20", 
                "Team21", "Team22", "Team23", "Team24", "Team25", "Team26", "Team27", "Team28", "Team29", "Team30" };

        List<int> indexList = new List<int>();
        List<string> result = new List<string>();

        for (int i = 0; i < teamNames.Length; i++) {
            indexList.Add(i);
        }

        for(int i = 0; i < count; i++) {
            int random = randomCreator.Next(0, indexList.Count);
            result.Add(teamNames[indexList[random]]);
            indexList.RemoveAt(random);
        }

        return result;
    }

    void CreateStartSeason(GameDataMediator gameData, bool isCreate = true) {
        if(gameData == null || gameData.GameInfo == null) { return; }
        
        if(isCreate) {
            gameData.GameDB.DropTable<TupleLeagueSeason>();
            gameData.GameDB.CreateTable<TupleLeagueSeason>();
        }

        var teams = gameData.GameDB.Select<Team>();

        foreach (var team in teams) {
            gameData.GameDB.Insert(new TupleLeagueSeason {
                Year = gameData.GameInfo.Year,
                TeamID = team.ID,
                WinCount = 0,
                DrawCount = 0,
                LoseCount = 0
            });
        }
    }

    void CreatePlayers(GameDataMediator gameData, NationTypes leagueType, double startPercentage = 0.0, double endPercentage = 0.0) {
        gameData.GameDB.DropTable<TuplePlayerBase>();
        gameData.GameDB.CreateTable<TuplePlayerBase>();

        gameData.GameDB.DropTable<TupleBatterStats>();
        gameData.GameDB.CreateTable<TupleBatterStats>();

        gameData.GameDB.DropTable<TuplePitcherStats>();
        gameData.GameDB.CreateTable<TuplePitcherStats>();

        // Create Player Tuples
        FactoryManager factoryManager = new FactoryManager(leagueType);
        factoryManager.CreateFirstPlayers(
            dataService     : gameData.GameDB,
            startPercentage : startPercentage,
            endPercentage   : endPercentage
        );

        // 이름 지정
        gameData.GameDB.ChangeNamesLocalize();
    }

    void CreatePitcherSeason(GameDataMediator gameData) {
        gameData.GameDB.DropTable<TuplePitcherSeason>();
        gameData.GameDB.CreateTable<TuplePitcherSeason>();
    }

    void CreateBatterSeason(GameDataMediator gameData) {
        gameData.GameDB.DropTable<TupleBatterSeason>();
        gameData.GameDB.CreateTable<TupleBatterSeason>();
    }

    void CreateTurn(GameDataMediator gameData) {
        gameData.GameDB.DropTable<TupleTurn>();
        gameData.GameDB.CreateTable<TupleTurn>();
    }

    void CreateSituation(GameDataMediator gameData) {
        gameData.GameDB.DropTable<TupleSituation>();
        gameData.GameDB.CreateTable<TupleSituation>();
    }

    void CreateLineup(GameDataMediator gameData) {
        gameData.GameDB.DropTable<TupleLineupBatter>();
        gameData.GameDB.CreateTable<TupleLineupBatter>();

        gameData.GameDB.DropTable<TupleLineupPitcher>();
        gameData.GameDB.CreateTable<TupleLineupPitcher>();

    }

    void CreateLineupGroup2(GameDataMediator gameData) {
        gameData.GameDB.DropTable<TupleLineupGroup2>();
        gameData.GameDB.CreateTable<TupleLineupGroup2>();
    }

    void CreateMatch(GameDataMediator gameData) {
        gameData.GameDB.DropTable<TupleMatch>();
        gameData.GameDB.CreateTable<TupleMatch>();
    }

    void CreateSpecialPlayerInfo(GameDataMediator gameData) {
        gameData.GameDB.DropTable<TupleNewPlayer>();
        gameData.GameDB.CreateTable<TupleNewPlayer>();

        gameData.GameDB.DropTable<TupleMercenary>();
        gameData.GameDB.CreateTable<TupleMercenary>();
    }

    void CreateViews(GameDataMediator gameData) {
        // Create "Pitcher" View
        gameData.GameDB.Execute("CREATE VIEW IF NOT EXISTS '" + typeof(Pitcher).Name +"' AS SELECT " +
            "TuplePitcherStats.PlayerID, " +

            "TuplePlayerBase.NameIndex, " +
            "TuplePlayerBase.Name, " +
            "TuplePlayerBase.Nation, " +
            "TuplePlayerBase.Nation, " +
            "TuplePlayerBase.BirthYear, " +
            "TuplePlayerBase.Potential, " +
            "TuplePlayerBase.THrowHand, " +
            "TuplePlayerBase.HitHand, " +

            "TuplePitcherStats.Stamina, " +
            "TuplePitcherStats.Velocity, " +
            "TuplePitcherStats.Stuff, " +
            "TuplePitcherStats.KMov, " +
            "TuplePitcherStats.GMov, " +
            "TuplePitcherStats.Control, " +
            "TuplePitcherStats.Location, " +
            "TuplePitcherStats.Composure, " +
            "TuplePitcherStats.ROpp, " +
            "TuplePitcherStats.LOpp, " +

            "TuplePitcherStats.TeamID, " +
            "TuplePitcherStats.FAYear, " +
            "TuplePitcherStats.Health, " +
            "TuplePitcherStats.Condition, " +
            "TuplePitcherStats.Fatigue, " +
            "TuplePitcherStats.Group, " +

            "TupleLineupPitcher.Position " +
            // "TupleLineupPitcher.Order " +

            "FROM TuplePitcherStats " +
            "INNER JOIN " + typeof(TuplePlayerBase).Name + " ON TuplePitcherStats.PlayerID=TuplePlayerBase.ID " +
            "INNER JOIN TupleLineupPitcher ON TuplePitcherStats.PlayerID=TupleLineupPitcher.PlayerID;");

        // Create "Batter" View
        gameData.GameDB.Execute("CREATE VIEW IF NOT EXISTS '" + typeof(Batter).Name + "' AS SELECT " +
            "TupleBatterStats.PlayerID, " +

            "TuplePlayerBase.NameIndex, " +
            "TuplePlayerBase.Name, " +
            "TuplePlayerBase.Nation, " +
            "TuplePlayerBase.Nation, " +
            "TuplePlayerBase.BirthYear, " +
            "TuplePlayerBase.Potential, " +
            "TuplePlayerBase.THrowHand, " +
            "TuplePlayerBase.HitHand, " +

            "TupleBatterStats.Hit, " +
            "TupleBatterStats.GapPower, " +
            "TupleBatterStats.Eye, " +
            "TupleBatterStats.Speed, " +
            "TupleBatterStats.DefenseC, " +
            "TupleBatterStats.Defense1B, " +
            "TupleBatterStats.Defense2B, " +
            "TupleBatterStats.Defense3B, " +
            "TupleBatterStats.DefenseSS, " +
            "TupleBatterStats.DefenseOF, " +

            "TupleBatterStats.TeamID, " +
            "TupleBatterStats.FAYear, " +
            "TupleBatterStats.Health, " +
            "TupleBatterStats.Condition, " +
            "TupleBatterStats.Fatigue, " +
            // "TupleBatterStats.Group, " +

            "TupleLineupBatter.Position " +
            // "TupleLineupBatter.Order " +

            "FROM TupleBatterStats " +
            "INNER JOIN " + typeof(TuplePlayerBase).Name + " ON TupleBatterStats.PlayerID=TuplePlayerBase.ID " +
            "INNER JOIN TupleLineupBatter ON TupleBatterStats.PlayerID=TupleLineupBatter.PlayerID;");
    }
}
