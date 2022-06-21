
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TeamSelectItem : MonoBehaviour {
    [SerializeField] TextMeshProUGUI _text;
    [SerializeField] Image _image;

    Action<Team> _clickAction;
    Button _button;

    public Team _team {get; private set;}

    void Awake() {
        _button = GetComponent<Button>();

        if(_button) {
            _button.onClick.RemoveAllListeners();
            _button.onClick.AddListener(() => {
                if(_clickAction != null) {
                    _clickAction(_team);
                }
            });
        }
    }

    public void SetData(Team team) {
        _team = team;
        if(_text) {
            _text.text = team == null ? "" : team.Name;
        }
        if(_image) {
            _image.gameObject.SetActive(team != null);
            // _image.sprite = sprite;
        }
    }

    public void SetInteractable(bool value) {
        if(_button) {
            _button.interactable = value;
        }
    }

    public void SetClickAction(Action<Team> clickAction) {
        _clickAction = clickAction;
    }
}