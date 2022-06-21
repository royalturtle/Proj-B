using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager {
    GameDataMediator GameData;
    Dictionary<int, List<SituationBase>> _situationDict;

    public List<TupleMatch> MatchAllList {get; private set;}

    MatchInfo PlayerMatch;
    List<MatchInfo> otherMatchList;

    List<SituationBase> _preMatchScheduleList, _postMatchScheduleList;
    int _situationIndex;

    public TurnManager(GameDataMediator gameDataMediator) {
        GameData = gameDataMediator;

        TupleGameInfo gameInfo = GameData.GameInfo;

        SetEventList(GameData.LeagueNation);
        _situationIndex = GameData.SituationIndex;

        PlayerMatch = null;
        otherMatchList = new List<MatchInfo>();

        _preMatchScheduleList = new List<SituationBase>();
        _postMatchScheduleList = new List<SituationBase>();

        _situationDict = new Dictionary<int, List<SituationBase>>();

        InitTurn();
    }

    void SetEventList(NationTypes leagueType) {
        if(leagueType == NationTypes.USA) {

        }
        else if(leagueType == NationTypes.JAPAN) {

        }
        else {

        }
    }

    int GetTurn() {
        return GameData.CurrentTurn;
    }

    int SetTurn(int turn) {
        TupleGameInfo gameInfo = GameData.GameInfo;
        gameInfo.Turn = turn;
        GameData.GameDB.UpdateData<TupleGameInfo>(gameInfo);
        return turn;
    }

    void InitTurn() {
        TupleGameInfo gameInfo = GameData.GameInfo;
        SetSchedule(
            myTeamIndex: gameInfo.MyTeamIndex, 
            turn: gameInfo.Turn,
            situationIndex : gameInfo.SituationIndex
        );
    }

    public void NextTurn() {
        int nextTurn = GetNextTurn();
        if (nextTurn != 0) {
            SetTurn(turn:nextTurn);
            // gameInfo.Turn = nextTurn;
            // _dataService.UpdateData<TupleGameInfo>(gameInfo);

            SetSchedule(myTeamIndex: GameData.MyTeamIndex, turn: nextTurn);
            Debug.Log("NEXT DAY 1");
        }
        else if(GameData != null) {
            Debug.Log("NEXT DAY 2");
            // GameData.ReadyNewYear();
            GameDataLoader gameDataLoader = new GameDataLoader();
            gameDataLoader.NewYear(GameData);
        }
    }

    int GetNextTurn() {
        int result = 0;
        if(GameData != null) {
            int currentTurn = GetTurn();

            Expression<Func<TupleTurn, bool>> turnRestrict = m => (m.Turn > currentTurn);
            List<TupleTurn> turnList = GameData.GameDB.Select<TupleTurn>(turnRestrict).ToList();

            if(turnList != null && turnList.Count > 0) {
                turnList.Sort((t1, t2) => t1.Turn.CompareTo(t2.Turn));
                result = turnList[0].Turn;
            }
        }
        return result;
    }

    public bool IsMatchExist {
        get {
            bool result = false;
            result = (PlayerMatch != null || (otherMatchList != null && otherMatchList.Count > 1));
            return result;
        }
    }
    
    void SetSchedule(int myTeamIndex, int turn, int situationIndex = 0) {
        // Set Schedule
        _situationIndex = situationIndex;

        PlayerMatch = null;
        _preMatchScheduleList.Clear();
        _postMatchScheduleList.Clear();
        otherMatchList.Clear();

        // Check is Match Exist?
        Expression<Func<TupleMatch, bool>> matchRestrict = m => (m.Turn == turn);
        List<TupleMatch> matchList = GameData.GameDB.Select<TupleMatch>(matchRestrict).ToList();

        foreach (TupleMatch match in matchList) {
            PlayerStatusInMatch playerStatus = match.IsTeamIDExist(myTeamIndex);
            if (playerStatus != PlayerStatusInMatch.NONE) {
                PlayerMatch = GetMatchInfoByTuple(
                    tuple:match,
                    status:playerStatus 
                );
            } 
            else {
                otherMatchList.Add(
                    GetMatchInfoByTuple(
                        tuple:match,
                        status:playerStatus 
                    )
                );
            }
        }

        // Check is Situation Exist
        if(_situationDict.ContainsKey(turn)) {
            List<SituationBase> todaySitutations = _situationDict[turn];

            foreach(SituationBase situation in todaySitutations) {
                if(situation.IsBeforeMatch) {
                    _preMatchScheduleList.Add(situation);
                }
                else {
                    _postMatchScheduleList.Add(situation);
                }
            }
        }
    }

    // public List<QuickMatchData> GetPlayerMatchAfter(int turnAfter, int teamIndex = -1)
    public List<QuickMatchData> GetPlayerMatchAfter(int turnAfter) {
        int teamIndex = GameData.MyTeamIndex;
        int year = GameData.CurrentYear;

        List<QuickMatchData> result = new List<QuickMatchData>();
        for (int turn = GetTurn(); turn < GetTurn() + turnAfter; turn++) {
            Debug.Log("Turn Ready" + turn.ToString());
            Expression<Func<TupleMatch, bool>> matchRestrict = m => (m.Turn == turn);

            IEnumerable<TupleMatch> searchResult = GameData.GameDB.Select<TupleMatch>(matchRestrict);
            int myTeamIndex = GameData.MyTeamIndex;

            bool isFound = false;
            foreach (TupleMatch match in searchResult) {
                PlayerStatusInMatch playerStatus = match.IsTeamIDExist(teamIndex);
                if (playerStatus != PlayerStatusInMatch.NONE) {
                    result.Add(new QuickMatchData(
                        isMatchRegistered: true,
                        year: match.Year,
                        turn: match.Turn,
                        homeTeam: GameData.Teams.TeamDict[match.HomeTeamID],
                        homeRank: GameData.Teams.Rank(id: match.HomeTeamID, year: match.Year),
                        awayTeam: GameData.Teams.TeamDict[match.AwayTeamID],
                        awayRank: GameData.Teams.Rank(id: match.AwayTeamID, year: match.Year),
                        playerStatus: match.IsTeamIDExist(id: myTeamIndex)
                    ));
                    isFound = true;
                    break;
                }
            }
            // if(!isFound) result.Add(null);
            if(!isFound) {
                result.Add(new QuickMatchData(
                    isMatchRegistered: false,
                    year:year, 
                    turn:turn
                ));
            }

        }
        return result;
    }

    /*
    public SituationBase GetSituation()
    {
        return _todayScheduleList[_situationIndex];
    }*/

    public MatchInfo GetPlayerMatch() {
        return PlayerMatch;
    }

    public List<MatchInfo> GetOthersMatches() {
        return otherMatchList;
    }

    MatchInfo GetMatchInfoByTuple(TupleMatch tuple, PlayerStatusInMatch status) {
        MatchInfo result = null;

        if(GameData != null) {
            result = new MatchInfo(
                playerStatus:status,
                matchData:tuple,
                homeTeam:GameData.Teams.TeamDict[tuple.HomeTeamID],
                awayTeam:GameData.Teams.TeamDict[tuple.AwayTeamID]
            );
        }

        return result;
    }

    public Dictionary<int, MatchInfo> GetMyMatchDict(int year = -1) {
        Dictionary<int, MatchInfo> result = new Dictionary<int, MatchInfo>();
        if(GameData != null) {
            if(year == -1) {
                year = GameData.CurrentYear;
            }
            List<TupleMatch> search = GameData.GameDB.GetMyMatchList(year:year);
            int myTeamIndex = GameData.MyTeamIndex;
            for(int i = 0; i < search.Count; i++) {
                
                PlayerStatusInMatch playerStatus = search[i].IsTeamIDExist(myTeamIndex);
                result.Add(
                    search[i].Turn, 
                    GetMatchInfoByTuple(
                        status:playerStatus,
                        tuple:search[i]
                    )
                );
            }
        }
        return result;
    }

    public List<MatchInfo> GetMatchInfoList(int turn, int year = -1) {
        List<MatchInfo> result = new List<MatchInfo>();
        if(GameData != null) {
            if(year == -1) {
                year = GameData.CurrentYear;
            }
            int myTeamIndex = GameData.MyTeamIndex;

            List<TupleMatch> matchTupleList = GameData.GameDB.GetMatchListByTurn(year:year, turn:turn);
            for(int i = 0; i < matchTupleList.Count; i++) {
                PlayerStatusInMatch status = matchTupleList[i].IsTeamIDExist(myTeamIndex);
                result.Add(
                    GetMatchInfoByTuple(
                        status:status,
                        tuple:matchTupleList[i]
                    )
                );
            }
        }
        return result;
    }

    List<TupleMatch> GetOtherMatchList(int turn, int year = -1) {
        List<TupleMatch> result = null;
        if(GameData != null) {
            if(year == -1) {
                year = GameData.CurrentYear;
            }
            result = GameData.GameDB.GetMatchListByTurn(year:year, turn:turn);
            int myTeamIndex = GameData.MyTeamIndex;
            for(int i =0 ; i < result.Count; i++) {
                if(result[i].IsTeamContained(myTeamIndex)) {
                    result.RemoveAt(i);
                    break;
                }
            }
        }
        return result;
        
    }

}
