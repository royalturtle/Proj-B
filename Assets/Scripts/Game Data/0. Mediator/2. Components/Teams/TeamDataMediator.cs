using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using UnityEngine;

public class TeamDataMediator {
    GameDataMediator GameData;
    
    public Dictionary <int, Team> TeamDict {get; private set;}
    public List<Team> DataList {
        get {
            List<Team> result = TeamDict == null ? new List<Team>() : TeamDict.Values.ToList();
            return result;
        }
    }
    public Team MyTeam {
        get {
            return GameData != null && TeamDict != null && TeamDict.ContainsKey(GameData.MyTeamIndex) ? TeamDict[GameData.MyTeamIndex] : null;
        }
    }

    public string GetName(int teamId) {
        string result = "";
        if(TeamDict != null && TeamDict.ContainsKey(teamId) && TeamDict[teamId] != null) {
            result = TeamDict[teamId].Name;
        }
        return result;
    }

    Dictionary<int, Sprite> TeamLogo;
    public Sprite GetLogo(int teamId) {
        if(TeamLogo == null) {
            List<Team> teamList = DataList;
            TeamLogo = new Dictionary<int, Sprite>();
            for(int i = 0; i < teamList.Count; i++) {
                TeamLogo.Add(teamList[i].ID, ResourcesUtils.GetTeamIconImage(teamList[i].LogoName));
            }
        }
        return TeamLogo != null && TeamLogo.ContainsKey(teamId) ? TeamLogo[teamId] : null;
    }

    public TeamDataMediator(GameDataMediator gameData) {
        GameData = gameData;
        TeamDict = GameData.GameDB.GetTeam();
    }

    public string Name(int id) {
        if(TeamDict.ContainsKey(id)) {
            return TeamDict[id].Name;
        }
        else {
            return id.ToString();
        }
    }

    public int Rank(int id, int year)     {
        Expression<Func<TupleLeagueSeason, bool>> restriction = t => (t.TeamID == id) && (t.Year == year);
        IEnumerable<TupleLeagueSeason> searchResult = GameData.GameDB.Select<TupleLeagueSeason>(restriction);
        return (searchResult != null) ? searchResult.First().Rank : GameConstants.NULL_INT;
    }

    public List<List<Team>> GetTeamList() {
        List<List<Team>> result = new List<List<Team>>();
        List<Team> teamList = DataList;

        int maxDiv1 = 0, maxDiv2 = 0;
        for(int i = 0; i < teamList.Count; i++) {
            if(maxDiv1 < teamList[i].Div1) { maxDiv1 = teamList[i].Div1; }
            if(maxDiv2 < teamList[i].Div2) { maxDiv2 = teamList[i].Div2; }
        }
        for(int i = 0; i <= maxDiv1 * maxDiv2; i++) {
            result.Add(new List<Team>());
        }

        for(int i = 0; i < teamList.Count; i++) {
            int div = teamList[i].Div1 * maxDiv2 + teamList[i].Div2;
            result[div].Add(teamList[i]);
        }
        
        return result;
    }

    public Dictionary<int, int> GetSalary() {
	    Dictionary<int, int> result = null;
	    if(GameData != null && GameData.Batters != null && GameData.Pitchers != null) {
		    result = GameData.Batters.GetSalary();
		    Dictionary<int, int> pitcherSalary = GameData.Pitchers.GetSalary();
		    foreach(KeyValuePair<int, int> item in pitcherSalary) {
    			result[item.Key] += item.Value;
		    }
    	}
    	return result;
    }

}
