using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NewGameNamePanel : SequencePanel{
    [SerializeField] TextInputFilter _teamNameInput, _managerNameInput;

    public string TeamName {get {return _teamNameInput == null ? "" : _teamNameInput.Text;}}
    public string OwnerName {get {return _managerNameInput == null ? "" : _managerNameInput.Text;}}
    
    void Awake() {
        _checkList = new List<bool>{false, false};
        if(_teamNameInput) {
            _teamNameInput.CheckValidAction = (bool value) => {
                Check(0, value);
            };
        }

        if(_managerNameInput) {
            _managerNameInput.CheckValidAction = (bool value) => {
                Check(1, value);
            };
        }
    }

}
