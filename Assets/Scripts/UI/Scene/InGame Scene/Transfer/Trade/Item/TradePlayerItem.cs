using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TradePlayerItem : MonoBehaviour {
    [SerializeField] TextMeshProUGUI _nameText;
    Button _button;

    Action<Team> _clickAction;

    public PlayerBase Player {get; private set;}

    void CheckButton() {
        if(_button == null) { _button = GetComponent<Button>();}
    }

    void Awake() {
        CheckButton();
    }

    public void SetAction(Action<TradePlayerItem> clickAction) {
        CheckButton();
        if(_button) {
            _button.onClick.RemoveAllListeners();
            _button.onClick.AddListener(() => {
                if(clickAction != null) { clickAction(this); }
            });
        }
    }

    public void SetData(PlayerBase player) {
        Player = player;
        if(Player != null) {
            if(_nameText) {_nameText.text = Player.Base.Name;}
        }
        else {
            if(_nameText) {_nameText.text = "";}
        }
    }

    public void SetInteractable(bool value) {
        CheckButton();
        if(_button) {_button.interactable = value;}
    }
}
