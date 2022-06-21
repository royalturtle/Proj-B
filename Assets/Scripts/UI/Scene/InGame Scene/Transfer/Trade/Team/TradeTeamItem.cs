using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TradeTeamItem : MonoBehaviour {
    [SerializeField] bool _isChangeAble;
    [SerializeField] Button _teamChangeButton;
    [SerializeField] List<TradePlayerItem> _playerList;
    [SerializeField] TradeMoneyItem _money;
    [SerializeField] Image _teamIcon;
    [SerializeField] TextMeshProUGUI _teamText;
    [SerializeField] TeamSelectDialog _selectDialog;
    [SerializeField] InGameSceneManager _inGameSceneManager;

    Team _team;

    void Awake() {
        if(_teamChangeButton) {
            _teamChangeButton.interactable = _isChangeAble;
            _teamChangeButton.onClick.RemoveAllListeners();
            _teamChangeButton.onClick.AddListener(() => {
                if(_selectDialog != null) {
                    _selectDialog.OpenButtonMode(clickAction:SetData, isMyTeamSelectable:false);
                }
            });
        }

        for(int i = 0; i < _playerList.Count; i++) {
            _playerList[i].SetAction(clickAction:SetPlayer);
        }
    }

    void SetPlayer(TradePlayerItem tradeObj) {
        if(tradeObj != null && _inGameSceneManager != null) {
            _inGameSceneManager.OpenGetTradePlayer(_team, tradeObj);
        }
    }

    public void SetData(Team team) {
        _team = team;
        bool isSelectAble = _team != null;

        for(int i = 0; i < _playerList.Count; i++) {
            _playerList[i].SetInteractable(isSelectAble);
        }
        if(_money) {
            _money.SetInteractable(isSelectAble);
        }

        if(_teamIcon) {
            _teamIcon.gameObject.SetActive(isSelectAble);
        }

        if(_teamText) {
            _teamText.gameObject.SetActive(isSelectAble);
            if(isSelectAble) {_teamText.text = _team.Name;}
        }

        if(!isSelectAble) {ClearItemData();}
    }

    public void ClearAllData() {
        SetData(null);
    }

    public void ClearItemData() {     
        for(int i = 0; i < _playerList.Count; i++) {
            _playerList[i].SetData(null);
        }
        if(_money) {
            _money.SetData(0);
        }
    }
}
