using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

public class BatterDataMediator {
    GameDataMediator GameData;
    public Dictionary<int, Batter> Data {get; private set;}
    public List<Batter> DataList {
        get {
            return Data != null ? Data.Values.ToList() : new List<Batter>();
        }
    }

    public Batter GetBatter(int id) {
        return Data != null && Data.ContainsKey(id) ? Data[id] : null;
    }

    public BatterDataMediator(GameDataMediator gameData) {
        GameData = gameData;
        Data = GameData.GameDB.GetAllCurrentBatters(year:GameData.CurrentYear);
    }

    public List<Batter> GetInTeam(int teamID, bool isOnlyGroup1=false, bool isOnlyGroup2=false) {
        if(isOnlyGroup1) {
            return Data.Values.ToList().Where(b => (b.Stats.TeamID == teamID) && (b.Stats.Group == 1)).ToList();
        }
        else if (isOnlyGroup2) {
            return Data.Values.ToList().Where(b => (b.Stats.TeamID == teamID) && (b.Stats.Group == 2)).ToList();
        }
        else {
            return Data.Values.ToList().Where(b => b.Stats.TeamID == teamID).ToList();
        }
    }

    public List<Batter> GetMyTeam(bool isOnlyGroup1=false, bool isOnlyGroup2=false) {
        return GetInTeam(GameData.MyTeamIndex, isOnlyGroup1, isOnlyGroup2);
    }
    
    public void UpdateStats(TupleBatterStats stat) {
        int id = stat.PlayerID;

        if(Data.ContainsKey(id)) {
            Data[id].Stats = stat;
            GameData.GameDB.UpdateData(stat);
        }
    }
    
    public Dictionary<int, int> GetSalary() {
        Dictionary<int, int> result = new Dictionary<int, int>();
        List<Batter> dataList = DataList;
		if(dataList != null) {
			for(int i = 0; i < dataList.Count; i++) {
				Batter batter = dataList[i];
				if(batter != null) {
					int teamId = batter.Stats.TeamID;
					if(result.ContainsKey(teamId)) {
						result[teamId] += batter.Stats.Salary;
					}
					else {
						result.Add(teamId, batter.Stats.Salary);
					}
				}
			}
		}
        return result;
    }
}
