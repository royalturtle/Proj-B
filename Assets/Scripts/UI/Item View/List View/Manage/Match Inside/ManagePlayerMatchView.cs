using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManagePlayerMatchView<T> : PlayerReferenceView<T> where T : PlayerBase { 
    [SerializeField] protected List<GameObject> _headerList;
    [SerializeField] protected List<PlayerListItemView> _itemList;
    protected List<int> _positionList;
    protected List<int> _orderList;

    public int ItemCount {
        get { return _itemList == null ? 0 : _itemList.Count; }
    }

    [SerializeField] protected List<Button> _infoButtonList;

    void Awake() {
        AwakeAfter();
    }

    public void Ready(Action<PlayerBase> viewDetailAction) {
        if(_infoButtonList != null) {
            for(int i = 0; i < _infoButtonList.Count; i++) {
                if(_infoButtonList[i] != null) {
                    _infoButtonList[i].onClick.AddListener(() => {
                        if(viewDetailAction != null) {
                            viewDetailAction(GetSelectedPlayer());
                        }
                    });
                }
            }
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

    protected virtual void AwakeAfter() {}
    public virtual void CheckSelected() {}

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

    protected virtual PlayerBase GetSelectedPlayer() {
        return null;
    }
}
