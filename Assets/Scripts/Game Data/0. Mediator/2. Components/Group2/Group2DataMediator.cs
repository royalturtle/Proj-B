using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

public class Group2DataMediator {
    GameDataMediator GameData;

    Dictionary<int, TupleLineupGroup2> Data;

    public Group2DataMediator(GameDataMediator gameData) {
        GameData = gameData;
        SetData();
    }

    void SetData() {
        if(GameData != null) {
            Data = GameData.GameDB.Select<TupleLineupGroup2>().ToDictionary(g => g.PlayerID);
        }
    }

    public TupleLineupGroup2 Get(int id) {
        return Data != null && Data.ContainsKey(id) ? Data[id] : null;
    }

    public void DeleteData(int id) {
        if(Data.ContainsKey(id)) {
            GameData.GameDB.Delete(Data[id]);
            Data.Remove(id);
        }
    }

    public void AddData(int id) {
        if(!Data.ContainsKey(id)) {
            TupleLineupGroup2 newTuple = new TupleLineupGroup2{
                PlayerID=id,
                Year = GameData.CurrentYear,
                Turn = GameData.CurrentTurn
            };
            GameData.GameDB.Insert<TupleLineupGroup2>(newTuple);
            Data[id] = newTuple;
        }
    }

    public bool IsGroup2(int id) {
        return Data != null && Data.ContainsKey(id) ? RemainDays(id) > 0 : false;
    }

    public bool IsChangeable(int id) {
        return Data != null && Data.ContainsKey(id) ? RemainDays(id) <= 0 : true;
    }

    public int RemainDays(int id) {
        return Data != null && Data.ContainsKey(id) ?
            Data[id].RemainDays( year:GameData.CurrentYear, turn:GameData.CurrentTurn) : 0;
    }
}
