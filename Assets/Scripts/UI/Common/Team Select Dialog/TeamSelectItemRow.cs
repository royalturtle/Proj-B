using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamSelectItemRow : MonoBehaviour {
    [field:SerializeField] public TeamSelectItem _team1 {get; private set;}
    [field:SerializeField] public TeamSelectItem _team2 {get; private set;}
    
    public void SetData(Team team1 = null, Team team2 = null) {
        gameObject.SetActive(team1 != null || team2 != null);
        if(_team1) { _team1.SetData(team1); }
        if(_team2) { _team2.SetData(team2); }
    }

    public void SetInteractable(bool value1 = false, bool value2 = false) {
        if(_team1) { _team1.SetInteractable(value1); }
        if(_team2) { _team2.SetInteractable(value2); }
    }
}
