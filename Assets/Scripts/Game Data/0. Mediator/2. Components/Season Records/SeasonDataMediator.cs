using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeasonDataMediator {
    GameDataMediator GameData;
    // Dictionary<int, TupleLeagueSeason> CurrentDictionary;
    public SeasonDataMediator(GameDataMediator gameData) {
        GameData = gameData;
    }

    public Dictionary<int, int> GetGameCountOfCurrentYear() {
        Dictionary<int, int> result = new Dictionary<int, int>();
        List<TupleLeagueSeason> searchList = GetLeagueSeasonList();
        if(result != null && searchList != null) {
            for(int i = 0; i < searchList.Count; i++) {
                if(searchList[i] != null) {
                    result.Add(searchList[i].TeamID, searchList[i].GameCount);
                }
            }
        }
        return result;
    }

    public Dictionary<int, int> GetTeamsGameCountDict(int year = -1) {
        List<TupleLeagueSeason> leagueSeasonList = GameData.Seasons.GetLeagueSeasonList(year: year);

        Dictionary<int, int> result = new Dictionary<int, int>();

        if(leagueSeasonList != null && leagueSeasonList.Count > 0) {
            foreach(TupleLeagueSeason season in leagueSeasonList) {
                result.Add(season.TeamID, season.GameCount);
            }
        }

        return result;
    }

    public List<List<TeamSeason>> GetSeaonList(int year = -1) {
        if (year == -1) { year = GameData.CurrentYear; }
        List<List<TeamSeason>> result = new List<List<TeamSeason>>();
        if(GameData != null && GameData.Teams != null) {
            List<Team> teamList = GameData.Teams.DataList;

            int maxDiv1 = 0, maxDiv2 = 0;
            for(int i = 0; i < teamList.Count; i++) {
                if(maxDiv1 < teamList[i].Div1) { maxDiv1 = teamList[i].Div1; }
                if(maxDiv2 < teamList[i].Div2) { maxDiv2 = teamList[i].Div2; }
            }
            for(int i = 0; i <= maxDiv1 * maxDiv2; i++) {
                result.Add(new List<TeamSeason>());
            }

            for(int i = 0; i < teamList.Count; i++) {
                int teamId = teamList[i].ID;
                TupleLeagueSeason season = GetLeagueSeason(teamId:teamId, year:year);
                
                int div = teamList[i].Div1 * maxDiv2 + teamList[i].Div2;
                result[div].Add(new TeamSeason(
                    team:teamList[i],
                    season:season
                ));
            }
        }
        
        return result;
    }

    public Dictionary<int, TeamSeason> GetSeasonDictionary(int year = -1) {
        if (year == -1) { year = GameData.CurrentYear; }
        Dictionary<int, TeamSeason> result = new Dictionary<int, TeamSeason>();

        if(GameData != null && GameData.Teams != null) {
            List<Team> teamList = GameData.Teams.DataList;
            teamList = teamList.OrderBy(team => team.ID).ToList();
            List<TupleLeagueSeason> tupleList = GetLeagueSeasonList(year:year);
            tupleList = tupleList.OrderBy(tuple => tuple.TeamID).ToList();

            for(int i = 0; i < teamList.Count && i < tupleList.Count; i++) {
                result.Add(
                    teamList[i].ID, 
                    new TeamSeason(
                        team:teamList[i],
                        season:tupleList[i]
                    )
                );
            }
        }
        
        return result;
    }

    public List<TupleLeagueSeason> GetLeagueSeasonList(int year = -1) {
        if (year == -1) { year = GameData.CurrentYear; }
        if(GameData != null && GameData.GameDB != null) {
            Expression<Func<TupleLeagueSeason, bool>> restriction = l => l.Year == year;
            return GameData.GameDB.Select<TupleLeagueSeason>(restriction).ToList();
        }
        return null;
    }

    public Dictionary<int, TupleLeagueSeason> GetLeagueSeasonDictionary(int year) {
        if(GameData != null && GameData.GameDB != null) {
            Expression<Func<TupleLeagueSeason, bool>> restriction = l => l.Year == year;
            return GameData.GameDB.Select<TupleLeagueSeason>(restriction).ToDictionary( team => team.TeamID, team => team);
        }
        return null;
    }

    public TupleLeagueSeason GetLeagueSeason(int teamId, int year = -1) {
        if(year == -1) { year = GameData.CurrentYear; }
        if(GameData != null && GameData.GameDB != null) {
            Expression<Func<TupleLeagueSeason, bool>> restriction = t => (t.Year == year) && (t.TeamID == teamId);
            List<TupleLeagueSeason> result = GameData.GameDB.Select<TupleLeagueSeason>(restriction);
            return result != null && result.Count > 0 ?  result[0] : null;
        }
        return null;
    }

    public void ApplyCurrentSeason() {
        List<List<TeamSeason>> seasonData = GetSeaonList();

        if(seasonData != null) {
            for(int i = 0; i < seasonData.Count; i++) {
                if(seasonData[i] != null) {
                    seasonData[i].Sort((x, y) => y._season.WinRate.CompareTo(x._season.WinRate));
                    double winRate = seasonData[i][0]._season.WinRate;
                    int mostDiff = seasonData[i][0]._season.WinCount - seasonData[i][0]._season.LoseCount;
                    int rank = 1;

                    for(int j = 0; j < seasonData[i].Count; j++) {
                        TupleLeagueSeason season = seasonData[i][j]._season;
                        if(season != null) {
                            if(season.WinRate < winRate) {
                                winRate = season.WinRate;
                                rank = j + 1;
                            }
                        
                            season.Rank = rank;
                            season.WinDiff = (mostDiff - (season.WinCount - season.LoseCount)) / 2.0;
                            GameData.GameDB.UpdateData(season);
                        }
                    }
                }
            }
        }
    }
}
