using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectConditionManager<T> where T : PlayerBase {
    string searchName;
    bool isRegular;
    Dictionary<int, RecordCondition> _conditionDict;

    public SelectConditionManager() {
        
        // _conditionDict =    
    }

    public void Init() {
        searchName = "";
        isRegular = true;
    }

    public List<T> GetValidPlayers(List<T> players) {
        List<T> result = new List<T>();

        if(players != null) {
            for(int i = 0; i < players.Count; i++) {
                T player = players[i];
                if(player != null && player.Base != null) {
                    bool isValid = true;

                    if(player.Base != null) {
                        if(searchName != "") {
                            isValid = player.Base.Name.Contains(searchName);
                        }

                        if(isValid) {
                
                        }
                    }
                }
            }
        }

        return result;
    }

    protected virtual bool CheckRegular(T player, int gameCount) { return true; }

    public void AddCondition(RecordCondition condition) {
        if(condition != null && _conditionDict != null) {
            if(_conditionDict.ContainsKey(condition.Type)) {
                _conditionDict[condition.Type] = condition;
            }
            else {
                _conditionDict.Add(condition.Type, condition);
            }
        }
    }

    public void DeleteCondition(int index) {
        if(_conditionDict != null && _conditionDict.ContainsKey(index)) {
            _conditionDict.Remove(index);
        }
    }
}
