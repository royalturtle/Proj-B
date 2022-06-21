using System;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataService : DataService {

    public GameDataService(bool isClear) : base(isClear:isClear) {}
    protected override string FileName {get {return GameConstants.DB_NOWGAME;}}

    public TupleGameInfo GetGameInfo() {
        using (IEnumerator<TupleGameInfo> iter = Select<TupleGameInfo>().GetEnumerator()) {
            iter.MoveNext();
            return iter.Current;
        }
    }
    
    public Dictionary<int, Team> GetTeam() {
        Dictionary<int, Team> result = Select<Team>().ToDictionary(item => item.ID, item => item);
        return result;
    }

    public Dictionary<int, TupleMatch> GetMatch() {
        Dictionary<int, TupleMatch> result = Select<TupleMatch>().ToDictionary(item => item.ID, item => item);
        return result;
    }

    public List<TupleMatch> GetMyMatchList(int year) {
        int myTeamIndex = GetGameInfo().MyTeamIndex;
        Expression<Func<TupleMatch, bool>> restriction = m => m.Year == year && (m.HomeTeamID == myTeamIndex || m.AwayTeamID == myTeamIndex);
        return Select<TupleMatch>(restriction);
    }

    public Dictionary<int, TupleMatch> GetMyMatchDict(int year) {
        return GetMyMatchList(year : year).ToDictionary(item=>item.Turn, item=>item);
    }

    public List<TupleMatch> GetMatchListByTurn(int year, int turn) {
        Expression<Func<TupleMatch, bool>> restriction = m => m.Turn == turn && m.Year == year;
        return Select<TupleMatch>(restriction).ToList();
    }

    public List<TupleBatterStats> GetAllBattersStat() {
        return Select<TupleBatterStats>().ToList();
    }

    public List<TuplePitcherStats> GetAllPitchersStat() {
        return Select<TuplePitcherStats>().ToList();
    }

    public Dictionary<int, Batter> GetAllCurrentBatters(int year) {
        List<TupleBatterStats> statsList = GetAllBattersStat();
        statsList.Sort((playerA, playerB) => playerA.PlayerID.CompareTo(playerB.PlayerID));

        List<int> idList = statsList.Select(p => p.PlayerID).ToList();

        Expression<Func<TuplePlayerBase, bool>> restriction1 = p => (idList.Contains(p.ID));
        List<TuplePlayerBase> baseList = Select<TuplePlayerBase>(restriction1).ToList();
        baseList.Sort((playerA, playerB) => playerA.ID.CompareTo(playerB.ID));

        // Season
        Expression<Func<TupleBatterSeason, bool>> restrictionSeason = p => (idList.Contains(p.PlayerID) && (p.Year == year));
        List<TupleBatterSeason> seasonList = Select<TupleBatterSeason>(restrictionSeason).ToList();
        seasonList.Sort((playerA, playerB) => playerA.ID.CompareTo(playerB.ID));

        Dictionary<int, Batter> result = new Dictionary<int, Batter>();

        for (int i = 0; i < statsList.Count; i++) {
            result.Add(baseList[i].ID, new Batter(player: baseList[i],stats: statsList[i],season: seasonList[i]));
        }
        return result;
    }

    public Dictionary<int, Pitcher> GetAllCurrentPitchers(int year) {
        // Stats
        List<TuplePitcherStats> statsList = Select<TuplePitcherStats>().ToList();
        statsList.Sort((playerA, playerB) => playerA.PlayerID.CompareTo(playerB.PlayerID));

        // Players
        List<int> idList = statsList.Select(p => p.PlayerID).ToList();

        // Base
        Expression<Func<TuplePlayerBase, bool>> restriction1 = p => (idList.Contains(p.ID));
        List<TuplePlayerBase> baseList = Select<TuplePlayerBase>(restriction1).ToList();
        baseList.Sort((playerA, playerB) => playerA.ID.CompareTo(playerB.ID));

        // Season
        Expression<Func<TuplePitcherSeason, bool>> restrictionSeason = p => (idList.Contains(p.PlayerID) && (p.Year == year));
        List<TuplePitcherSeason> seasonList = Select<TuplePitcherSeason>(restrictionSeason).ToList();
        seasonList.Sort((playerA, playerB) => playerA.ID.CompareTo(playerB.ID));

        Dictionary<int, Pitcher> result = new Dictionary<int, Pitcher>();

        for (int i = 0; i < statsList.Count; i++) {
            result[baseList[i].ID] = new Pitcher(
                player: baseList[i],
                stats: statsList[i],
                season: seasonList[i]
                );
        }
        return result;
    }

    public void ChangeNamesLocalize() {
        NameDataService _nameDataService = new NameDataService();

        List<TuplePlayerBase> playerList = Select<TuplePlayerBase>().ToList();

        for(int i = 0; i < playerList.Count; i++) {
            _nameDataService.ChangeNameLocalize(playerList[i]);
            UpdateData<TuplePlayerBase>(playerList[i]);
        }
    }

    public List<TupleBatterSeason> GetBatterRecordsSeason(int year) {
        int _year = year;

        Expression<Func<TupleBatterSeason, bool>> restrictionSeason = s => (s.Year == _year);
        List<TupleBatterSeason> seasonList = Select<TupleBatterSeason>(restrictionSeason).ToList();
        return seasonList;
    }

    public List<TuplePitcherSeason> GetPitcherRecordsSeason(int year) {
        int _year = year;

        Expression<Func<TuplePitcherSeason, bool>> restrictionSeason = s => (s.Year == _year);
        List<TuplePitcherSeason> seasonList = Select<TuplePitcherSeason>(restrictionSeason).ToList();
        return seasonList;
    }

    public void ReadyNewYear(bool isStart = true) {
        TupleGameInfo gameInfo = GetGameInfo();
        if(!isStart) {
            gameInfo.Year++;
        }
        int _currentYear = gameInfo.Year;
        NationTypes leagueType = gameInfo.LeagueNation;
        ReadyMatchCalendar(year: _currentYear, leagueType: leagueType);

        FactoryManager factoryManager = new FactoryManager(leagueType);
        factoryManager.CreateYoungPlayers(this);
        factoryManager.CreateMercenaryPlayers(this);
    }

    public List<TupleLineupBatter> GetBatterLineup() {
        return Select<TupleLineupBatter>().ToList();
    }

    public Dictionary<int, TupleLineupPitcher> GetPitcherLineup() {
        return Select<TupleLineupPitcher>().ToDictionary(item => item.TeamId, item => item);
    }

    public void ReadyMatchCalendar(int year, NationTypes leagueType)  {
        bool[] daysCheck = new bool[366];
        if(leagueType == NationTypes.USA) {
            
        } 
        else if(leagueType == NationTypes.JAPAN) {

        }
        // KOR
        else {
            DateObj startSeasonDate = new DateObj(year:year, month:3, dayName: DaysEnum.FRIDAY, week:5);

            List<int> teamIndexListOrg = Enumerable.Range(1, 10).ToList();
            List<int> teamIndexList = Utils.ShuffleList<int>(teamIndexListOrg);

            FileUtils.DeletePersistentFile(GameConstants.FILE_MATCH_KOREA);
            string matchFilePath = FileUtils.StreammingAssetToPersistent(GameConstants.FILE_MATCH_KOREA);

            FileInfo fileInfo = new FileInfo(matchFilePath);
            string value = "";

            if (fileInfo.Exists) {
                StreamReader reader = new StreamReader(matchFilePath);
                value = reader.ReadToEnd();
                reader.Close();
            }

            string[] row = value.Split('\n'); //스페이스를 기준을 행 분류 
            int rowSize = row.Length;
            int columnSize = row[0].Split('\t').Length; //탭을 기준으로 열 분류
            int maxTurn = startSeasonDate.Turn;

            // 이차원 배열에 데이터 넣어주기
            for (int i = 0; i < rowSize; i++) {
                string[] column = row[i].Split('\t');

                int currentTurn = startSeasonDate.Turn + Int32.Parse(column[0]);
                if (currentTurn > maxTurn) maxTurn = currentTurn;

                Insert(new TupleMatch {
                    Year = year,
                    Turn = currentTurn,
                    HomeTeamID = Int32.Parse(column[1]),
                    AwayTeamID = Int32.Parse(column[2]),
                    IsFinished = 0,
                    HomeScore = 0,
                    AwayScore = 0
                });
            }

            for (int i = startSeasonDate.Turn; i <= maxTurn; i++) {
                Insert(new TupleTurn { Turn = i });
                daysCheck[i] = true;
            }


            // 그 외의 상황들

            // 월말 정산
            for (int i = 1; i <= 12; i++) {
                DateObj lastDay = new DateObj(year: year, month: i, isEndMonth : true);

                // 상황 추가
                Insert(new TupleSituation {
                    Turn = lastDay.Turn,
                    Type = nameof(SituationType.END_OF_MONTH),
                    IsBeforeMatch = false
                });

                // 턴 추가
                if(!daysCheck[i]) { 
                    Insert(new TupleTurn { Turn = lastDay.Turn });
                    daysCheck[i] = true;
                }
            }

            // 첫 턴을 변경

        }
    }
}
