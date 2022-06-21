using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManagePitcherMatchView : ManagePlayerMatchView<Pitcher> {
    LineupPitcher _lineup;

    [SerializeField] Button _changeButton;

    Pitcher _selectedPitcher;

    protected override void AwakeAfter() {
        if(_changeButton) {
            _changeButton.onClick.AddListener(() => {
                if(_lineup != null && _selectedPitcher != null) {
                    _lineup.ChangePitcher(_selectedPitcher);
                }
                SetData(_lineup, isUpdate:true);
            });
        }

        if(_itemList != null) {
            for(int i = 0; i < _itemList.Count; i++) {
                if(_itemList[i] != null) {
                    _itemList[i].Ready(clickAction:ItemClickAction);
                }
            }
        }
    }

    void ItemClickAction(PlayerListItemView player) {
        _selectedPitcher = (player != null) ? (Pitcher)player.Player : null;

        CheckSelected();
    }

    void SetButtonInteractable() {
        bool isSelectedExist = _selectedPitcher != null;
        if(_infoButtonList != null) {
            for(int i = 0; i < _infoButtonList.Count; i++) {
                if(_infoButtonList[i] != null) {
                    _infoButtonList[i].interactable = isSelectedExist;
                }
            }
        }

        if(_changeButton) {
            _changeButton.interactable = isSelectedExist;
        }
    }

    public override void CheckSelected() {
        SetButtonInteractable();
        if(_itemList != null && _lineup != null) {
            for(int i = 0; i < _itemList.Count; i++) {
                if(_itemList[i] != null) {
                    _itemList[i].SetSelected(_itemList[i].IsEqual(player:_selectedPitcher));
                    _itemList[i].SetSelectedFrame(_itemList[i].IsEqual(player:_lineup.CurrentPitcher));
                }
            }
        }
    }

    public void ClearSelected() {
        _selectedPitcher = null;
    }

    public void SetData(LineupPitcher lineup, bool isUpdate = false) {
        _lineup = lineup;

        if(_lineup != null) {
            _viewList = new List<Pitcher>();
            _positionList = new List<int>();
            _orderList = new List<int>();

            for(int i = 0; i < _lineup.StartingPitchers.Count; i++) {
                _viewList.Add(_lineup.StartingPitchers[i]);
                _positionList.Add((int)PitcherPositionEnum.STARTING);
                _orderList.Add(_lineup.StartingOrders[i]);
            }

            if(_lineup.CloserPitcher != null) {
                _viewList.Add(_lineup.CloserPitcher);
                _positionList.Add((int)PitcherPositionEnum.CLOSER);
                _orderList.Add(0);
            }

            if(_lineup.SetupPitcher != null) {
                _viewList.Add(_lineup.SetupPitcher);
                _positionList.Add((int)PitcherPositionEnum.SETUP);
                _orderList.Add(0);
            }

            for(int i = 0; i < _lineup.ReliefPitchers.Count; i++) {
                _viewList.Add(_lineup.ReliefPitchers[i]);
                _positionList.Add((int)PitcherPositionEnum.RELIEF);
                _orderList.Add(_lineup.ReliefOrders[i]);
            }
            
            // 등록된 부분을 UI에 표현 
            if(_pagePager != null) {
                _pagePager.InitObject(totalPage: (_viewList.Count - 1) / ItemCount + 1, isUpdate:isUpdate);
            }

            if(_categoryPager != null) {
                _categoryPager.InitObject(totalPage: _itemList[0].CategoryCount, isUpdate:isUpdate);
            }
        }
        ClearSelected();
        CheckSelected();
    }

    protected override PlayerBase GetSelectedPlayer() {
        return _selectedPitcher;
    }
}