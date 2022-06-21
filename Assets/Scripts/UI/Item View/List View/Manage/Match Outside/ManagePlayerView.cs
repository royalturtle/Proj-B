using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ManagePlayerView<T> : PlayerReferenceView<T> where T : PlayerBase {
    [SerializeField] protected List<GameObject> _headerList;
    [SerializeField] protected List<PlayerListItemView> _itemList;
    protected List<int> _positionList;
    protected List<int> _orderList;

    public int ItemCount {
        get { return _itemList == null ? 0 : _itemList.Count; }
    }

    protected PlayerBase _selectedPlayer, _prevSelectedPlayer;
    protected int _selectedPositionIndex;
    [SerializeField] protected Button _infoButton, _changePositionButton, _changePlayerButton, _changeGroupButton, _selectButton;    
    [SerializeField] protected Button _infoButton2, _yesButton, _cancelButton;
    [SerializeField] protected GameObject _actionManageLayer, _actionCheckLayer;

    protected Action _yesAction, _cancelAction;

    protected virtual bool IsBatter() { return true; }

    void Awake() {
        RegisterAction(_yesButton, () => {
            if(_yesAction != null) { _yesAction(); }
        });

        RegisterAction(_cancelButton, () => {
            if(_cancelAction != null) { _cancelAction(); }
        });
    }

    void RegisterAction(Button button, Action clickAction) {
        if(button != null) {
            button.onClick.AddListener(() => {
                if(clickAction != null) {
                    clickAction();
                }
            });
        }
    }

    // public void Ready() {}

    public void Ready(
        Action<PlayerBase, bool> viewDetailAction, 
        Action<PlayerBase, bool> changeGroupAction, 
        Action<PlayerBase, int, bool> changePositionAction, 
        Action<PlayerBase, PlayerBase, bool> changePlayerAction, 
        Action<PlayerBase> getTradePlayerAction
    ) {
        if(_infoButton) {
            _infoButton.onClick.AddListener(() => {
                if(Utils.NotNull(viewDetailAction, _selectedPlayer)) {
                    viewDetailAction(_selectedPlayer, IsBatter());
                }
            });
        }

        if(_changePositionButton) {
            _changePositionButton.onClick.RemoveAllListeners();
            _changePositionButton.onClick.AddListener(() => {
                if(changePositionAction != null) {
                    changePositionAction(_selectedPlayer, _selectedPositionIndex, IsBatter());
                }
                ItemClickAction(null);
            });
        }

        if(_changePlayerButton) {
            _changePlayerButton.onClick.AddListener(() => {
                _yesAction = () => {
                    if(Utils.NotNull(changePlayerAction, _selectedPlayer, _prevSelectedPlayer)) {
                        if(_selectedPlayer.Base.ID != _prevSelectedPlayer.Base.ID) {
                            changePlayerAction(_selectedPlayer, _prevSelectedPlayer, IsBatter());
                        }
                    }
                    SetCheckMode(false);
                    _prevSelectedPlayer = null;
                    ItemClickAction(null);
                };

                _cancelAction = () => {
                    SetCheckMode(false);
                    _prevSelectedPlayer = null;
                    ItemClickAction(null);
                };

                _prevSelectedPlayer = _selectedPlayer;
                CheckSelected();
                SetCheckMode(true);
            });
        }

        if(_changeGroupButton) {
            _changeGroupButton.onClick.AddListener(() => {
                if(Utils.NotNull(changeGroupAction, _selectedPlayer)) {
                    changeGroupAction(_selectedPlayer, IsBatter());
                }
                ItemClickAction(null);
            });
        }

        if(_selectButton) {
            _selectButton.onClick.AddListener(() => {
                if(Utils.NotNull(getTradePlayerAction, _selectedPlayer)) {
                    getTradePlayerAction(_selectedPlayer);
                }
            });
        }

        RegisterAction(_infoButton2, () => {
            if(Utils.NotNull(viewDetailAction, _selectedPlayer)) {
                viewDetailAction(_selectedPlayer, IsBatter());
            }
        });

        for(int i = 0; i < _itemList.Count; i++) {
            _itemList[i].Ready(clickAction:ItemClickAction);
        }

        if(_pagePager != null) {
            _pagePager.PageChanged += (int page) => {
                SetPage(page);
                CheckSelected();
            };
        }

        if(_categoryPager != null) {
            _categoryPager.PageChanged += (int page) => {
                SetDetailPage(page);
            };
        }
    }

    bool IsItemAlreadySelected(PlayerListItemView player, PlayerBase selected) {
        return player != null && 
                player.Player != null && 
                selected != null && 
                player.Player.Base.ID == selected.Base.ID;
    }

    void ItemClickAction(PlayerListItemView player) {
        _selectedPlayer = (player == null) || IsItemAlreadySelected(player:player, selected:_selectedPlayer) ? null : player.Player;
        _selectedPositionIndex = _selectedPlayer != null && player != null ? player.positionIndex : 0;
        
        SetButtonInteractable(_selectedPlayer != null);
        CheckSelected();
    }

    void CheckSelected() {
        for(int i = 0; i < _itemList.Count; i++) {
            bool isEqual = IsItemAlreadySelected(player:_itemList[i], selected:_selectedPlayer);
            _itemList[i].SetSelected(isEqual);

            isEqual = IsItemAlreadySelected(player:_itemList[i], selected:_prevSelectedPlayer);
            _itemList[i].SetSelectedFrame(isEqual);
        }
    }

    public void ClearSelected() {
        ItemClickAction(null);
    }

    void SetButtonInteractable(bool value) {
        if(_infoButton) {_infoButton.interactable = value;}
        if(_changePositionButton) {_changePositionButton.interactable = value && IsPositionChangeable(_selectedPositionIndex);}
        if(_changePlayerButton) {_changePlayerButton.interactable = value;}
        if(_changeGroupButton) {_changeGroupButton.interactable = value;}
        if(_selectButton) {_selectButton.interactable = value;}
        if(_infoButton2) {_infoButton2.interactable = value;}
        if(_yesButton) {_yesButton.interactable = value;}
    }

    protected void SetCheckMode(bool value) {
        if(_actionManageLayer) {
            _actionManageLayer.SetActive(!value);
        }
        if(_actionCheckLayer) {
            _actionCheckLayer.SetActive(value);
        }
    }

    void SetButtonActive(
        bool isInfo, bool isPosition, bool isPlayer, bool isGroup, bool isSelect
    ) {
        if(_infoButton) {_infoButton.gameObject.SetActive(isInfo);}
        if(_changePositionButton) {_changePositionButton.gameObject.SetActive(isPosition);}
        if(_changePlayerButton) {_changePlayerButton.gameObject.SetActive(isPlayer);}
        if(_changeGroupButton) {_changeGroupButton.gameObject.SetActive(isGroup);}
        if(_selectButton) {_selectButton.gameObject.SetActive(isSelect);}
    }

    protected virtual bool IsPositionChangeable(int position) {
        return true;
    }

    public void SetDataTrade(GameDataMediator gameData, int teamId, bool isUpdate = false) {
        SetCheckMode(true);
        SetButtonActive(true, false, false, false, true);
        SetData(gameData, teamId, isUpdate);
    }

    public void SetDataManage(GameDataMediator gameData, int teamId, bool isUpdate = false) {
        SetCheckMode(false);
        SetButtonActive(true, true, true, true, false);
        SetData(gameData, teamId, isUpdate);
    }

    public void SetDataView(GameDataMediator gameData, int teamId, bool isUpdate = false) {
        SetCheckMode(false);
        SetButtonActive(true, false, false, false, false);
        SetData(gameData, teamId, isUpdate);
    }

    public virtual void SetData(GameDataMediator gameData, int teamId, bool isUpdate = false) { }
    void SetPage(int page) {
        if(_pagePager != null) {
            List<T> players;
            List<int> positions;
            List<int> orders;

            // 모든 결과
            int itemCount = ItemCount;
            int inputItemCount = (_viewList.Count >= (page + 1) * itemCount) ? itemCount : _viewList.Count - page * itemCount;
            
            players = _viewList.GetRange(page * itemCount, inputItemCount);
            positions = _positionList.GetRange(page * itemCount, inputItemCount);
            orders = _orderList.GetRange(page * itemCount, inputItemCount);

            int i = 0;
            for (; i < players.Count; i++) { 
                _itemList[i].SetData(players[i], (int)positions[i], orders[i]); 
            }
            for (; i < ItemCount; i++) {
                _itemList[i].SetData(null); 
            }
        }
    }
    
    void SetDetailPage(int index) {
        for (int i = 0; i < ItemCount;i++) {
            _itemList[i].SetDetailPage(index);
        }

        if(_headerList != null) {
            for(int i = 0; i < _headerList.Count; i++) {
                _headerList[i].SetActive(i == index);
            }
        }
    }
}
