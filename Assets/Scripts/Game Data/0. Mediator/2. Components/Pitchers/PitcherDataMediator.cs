using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

public class PitcherDataMediator {
    GameDataMediator GameData;
    public Dictionary<int, Pitcher> Data {get; private set;}
    public List<Pitcher> DataList {
        get {
            return Data != null ? Data.Values.ToList() : new List<Pitcher>();
        }
    }

    public PitcherDataMediator(GameDataMediator gameData) {
        GameData = gameData;
        Data = GameData.GameDB.GetAllCurrentPitchers(year:GameData.CurrentYear);
    }
        
    public List<Pitcher> GetMyTeam(bool isOnlyGroup1=false, bool isOnlyGroup2=false) {
        return GetInTeam(GameData.MyTeamIndex, isOnlyGroup1, isOnlyGroup2);
    }

    public List<Pitcher> GetInTeam(int teamID, bool isOnlyGroup1=false, bool isOnlyGroup2=false) {
        if(isOnlyGroup1) {
            return Data.Values.ToList().Where(p => (p.Stats.TeamID == teamID) && (p.Stats.Group == 2)).ToList();
        }
        else if(isOnlyGroup2) {
            return Data.Values.ToList().Where(p => (p.Stats.TeamID == teamID) && (p.Stats.Group == 2)).ToList();
        }
        else {
            return Data.Values.ToList().Where(p => p.Stats.TeamID == teamID).ToList();
        }
    }

    public void UpdateStats(TuplePitcherStats stat) {
        int id = stat.PlayerID;

        if(Data.ContainsKey(id)) {
            Data[id].Stats = stat;
            GameData.GameDB.UpdateData(stat);
        }
    }

    public Dictionary<int, int> GetSalary() {
        Dictionary<int, int> result = new Dictionary<int, int>();
        List<Pitcher> dataList = DataList;
		if(dataList != null) {
			for(int i = 0; i < dataList.Count; i++) {
				Pitcher pitcher = dataList[i];
				if(pitcher != null) {
					int teamId = pitcher.Stats.TeamID;
					if(result.ContainsKey(teamId)) {
						result[teamId] += pitcher.Stats.Salary;
					}
					else {
						result.Add(teamId, pitcher.Stats.Salary);
					}
				}
			}
		}
        return result;
    }
}
