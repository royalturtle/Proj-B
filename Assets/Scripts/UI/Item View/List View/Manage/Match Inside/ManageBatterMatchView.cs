using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManageBatterMatchView : ManagePlayerMatchView<Batter> {
    LineupBatter _lineup;
    
    [SerializeField] Button _positionButton, _changeStartButton, _changeConfirmButton, _changeCancelButton;
    [SerializeField] SelectBatterPositionDialog _positionDialog;
    [SerializeField] GameObject _changeLayout, _manageLayout;

    BatterPositionEnum _prevSelectedPosition, _selectedPosition;
    int _prevSelectedIndex, _selectedIndex;

    protected override void AwakeAfter() {
        if(_positionButton != null) {
            _positionButton.onClick.AddListener(() => {
                if(_positionDialog != null && _lineup != null && IsPositionGroup1(_selectedPosition)) {
                    _positionDialog.Open(
                        position      : _selectedPosition,
                        confirmAction : (BatterPositionEnum position) => {
                            _lineup.ChangePosition(_selectedPosition, position);
                            SetDefaultMode(true);
                            SetData(_lineup, isUpdate:true);
                        }
                    );
                }
            });
        }

        if(_changeStartButton != null) {
            _changeStartButton.onClick.AddListener(() => {
                if(_lineup != null) {
                    SetDefaultMode(false);
                }
            });
        }

        if(_changeConfirmButton != null) {
            _changeConfirmButton.onClick.AddListener(() => {
                if(_lineup != null) {
                    bool isPrevSub = _prevSelectedPosition == BatterPositionEnum.CANDIDATE;
                    bool isCurrentSub = _selectedPosition == BatterPositionEnum.CANDIDATE;

                    if((!isPrevSub && !isCurrentSub) || (isPrevSub && isCurrentSub)) {
                        SetDefaultMode(true);
                    }
                    else {
                        int subIndex = (isPrevSub ? _prevSelectedIndex : _selectedIndex) - 1;
                        int order = (isPrevSub ? _selectedIndex : _prevSelectedIndex) - 1;

                        _lineup.PutSub(subIndex : subIndex, order : order);
                        SetDefaultMode(true);
                        SetData(_lineup, isUpdate:true);
                    }
                }
            });
        }

        if(_changeCancelButton != null) {
            _changeCancelButton.onClick.AddListener(() => {
                if(_lineup != null) {
                    SetDefaultMode(true);
                }
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

    public void SetData(LineupBatter lineup, bool isUpdate = false) {
        _lineup = lineup;

        SetDefaultMode(true);

        if(_lineup != null) {
            // 필요한 List 생성
            _viewList = new List<Batter>();
            _positionList = new List<int>();
            _orderList = new List<int>();

            // 선발 등록
            int orderIndex = 0;
            for(int i = 0; i < _lineup.Playings.Count; i++) {
                _viewList.Add(_lineup.Playings[i]);
                _positionList.Add((int)_lineup.DefenseList[orderIndex]);
                _orderList.Add(++orderIndex);
            }

            // 후보 등록
            orderIndex = 0;
            for(int i = 0; i < _lineup.Candidates.Count; i++) {
                _viewList.Add(_lineup.Candidates[i]);
                _positionList.Add((int)BatterPositionEnum.CANDIDATE);
                _orderList.Add(++orderIndex);
            }

            // 등록된 부분을 UI에 표현 
            if(_pagePager != null) {
                _pagePager.InitObject(totalPage: (_viewList.Count - 1) / ItemCount + 1, isUpdate:isUpdate);
            }

            if(_categoryPager != null) {
                _categoryPager.InitObject(totalPage: _itemList[0].CategoryCount, isUpdate:isUpdate);
            }
        }
    }

    void ItemClickAction(PlayerListItemView player) {
        _selectedPosition = (player != null) ? (BatterPositionEnum)player.positionIndex : BatterPositionEnum.NONE;
        _selectedIndex = (player != null) ? player.Order : GameConstants.NULL_INT;

        CheckSelected();
    }

    void SetButtonInteractable() {
        bool isSelectedExist = _selectedPosition != BatterPositionEnum.NONE;
        if(_infoButtonList != null) {
            for(int i = 0; i < _infoButtonList.Count; i++) {
                if(_infoButtonList[i] != null) {
                    _infoButtonList[i].interactable = isSelectedExist;
                }
            }
        }
        
        if(_positionButton) {
            _positionButton.interactable = isSelectedExist;
        }

        if(_changeStartButton) {
            _changeStartButton.interactable = isSelectedExist;
        }
        
        if(_changeConfirmButton) {
            _changeConfirmButton.interactable = isSelectedExist;
        }
    }

    void SetDefaultMode(bool value) {
        if(_manageLayout) {
            _manageLayout.SetActive(value);
        }
        if(_changeLayout) {
            _changeLayout.SetActive(!value);
        }

        if(!value) {
            MoveSelected();
        }
        else {
            ClearSelected();
            ClearPrevSelected();
        }
    }

    public override void CheckSelected() {
        SetButtonInteractable();
        if(_itemList != null) {
            for(int i = 0; i < _itemList.Count; i++) {
                if(_itemList[i] != null) {
                    _itemList[i].SetSelected(_itemList[i].IsEqual(position:(int)_selectedPosition, order:_selectedIndex));
                    _itemList[i].SetSelectedFrame(_itemList[i].IsEqual(position:(int)_prevSelectedPosition, order:_prevSelectedIndex));
                }
            }
        }
    }

    public void ClearSelected() {
        _selectedPosition = BatterPositionEnum.NONE;
        _selectedIndex = GameConstants.NULL_INT;
    }

    void ClearPrevSelected() {
        _prevSelectedPosition = BatterPositionEnum.NONE;
        _prevSelectedIndex = GameConstants.NULL_INT;
    }

    void MoveSelected() {
        _prevSelectedPosition = _selectedPosition;
        _prevSelectedIndex = _selectedIndex;
    }

    bool IsPositionGroup1(BatterPositionEnum position) {
        return BatterPositionEnum.C <= position && position <= BatterPositionEnum.DH;
    }

    protected override PlayerBase GetSelectedPlayer() {
        PlayerBase result = null;
        if(_lineup != null) {
            result = _lineup.GetBatter(position:(BatterPositionEnum)_selectedPosition, order:_selectedIndex);
        }
        return result;
    }
}
