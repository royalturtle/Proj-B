using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerListItemView : MonoBehaviour {
    protected PlayerBase _player;
    public PlayerBase Player {get{return _player;}}
    Button _button;

    [SerializeField] StatViewBase _basicStat;
    [SerializeField] List<StatViewBase> _statList;
    Image _backImage;

    [SerializeField] GameObject _selectedFrame;
    public int positionIndex {get; protected set;}
    public int Order {get; protected set;}

    [SerializeField] ExternalText _positionText, _subInfoText;
    [SerializeField] GameObject _hideObject;
    [SerializeField] protected Image _mainLabelImage;

    public int CategoryCount { get { return _statList == null ? 0 : _statList.Count;}}

    void Awake() {
        _backImage = GetComponent<Image>();
    }

    public bool IsEqual(PlayerListItemView playerObject) {
        return Utils.NotNull(playerObject, _player) && playerObject.Player != null && (_player.Base.ID == playerObject.Player.Base.ID);
    }

    public void Ready(Action<PlayerListItemView> clickAction) {
        CheckButton();
        if(_button) {
            _button.onClick.RemoveAllListeners();
            _button.onClick.AddListener(() => { 
                if(clickAction != null) { 
                    clickAction(this); 
                } 
            });
        }
    }

    public void SetData(PlayerBase player, int position = 0, int order = 0) {
        _player = player;
        positionIndex = position;
        Order = order;
        if(_player != null) {
            if(_basicStat != null) {
                _basicStat.SetData(player);
            }
            for(int i = 0; i < _statList.Count; i++) {
                _statList[i].SetData(player);
            }

            if(_positionText) {
                _positionText.SetData(PositionToString(position));
            }

            if(_mainLabelImage) {
                _mainLabelImage.color = GetMainLabelColor();
            }
        }

        if(_hideObject) {
            _hideObject.SetActive(player == null);
        }

        if(_positionText) {
            _positionText.gameObject.SetActive(player != null);
        }

        if(_subInfoText) {
            _subInfoText.gameObject.SetActive(order != 0);
            _subInfoText.SetData(((order < 0) ? "D" : "") + order.ToString());
        }

        CheckButton();
        if(_button) {
            _button.interactable = _player != null;
        }
    }

    protected virtual Color32 GetMainLabelColor() {
        return ColorConstants.Instance.MainLabelColorGroup2;
    }
    protected virtual string PositionToString(int position) { return position.ToString(); }

    void CheckButton() {
        if(_button == null) {
            _button = GetComponent<Button>();
        }
    }

    public void SetSelected(bool value) {
        if(_backImage) {
            _backImage.color = value ? 
                ColorConstants.Instance.PlayerBackgroundSelected :
                ColorConstants.Instance.PlayerBackgroundNormal;
        }
    }
    
    public void SetSelectedFrame(bool value) {
        if(_selectedFrame != null) {
            _selectedFrame.SetActive(value);
        }
    }

    public void SetDetailPage(int index) {
        int _index = (index + 1 >= CategoryCount) ? 0 : index + 1;
        _index = (_index - 1 < 0) ? CategoryCount - 1 : _index - 1;

        for(int i = 0; i < _statList.Count; i++) {
            GameObject currentObject = _statList[i].gameObject;
            if(i == index) {
                currentObject.SetActive(true);
            }
            else if(currentObject.activeSelf) {
                currentObject.SetActive(false);
            }
        }
    }

    public bool IsEqual(PlayerBase player) {
        return _player != null && _player.IsEqual(player);
    }

    public bool IsEqual(int position, int order) {
        return positionIndex == position && Order == order;
    }
}
