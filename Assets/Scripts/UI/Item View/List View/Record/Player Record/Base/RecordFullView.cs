using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecordFullView<T> : PlayerReferenceView<T> where T : PlayerBase{
    protected Func<int, int> _getGameCountAction;
    protected Func<int, Sprite> _getSpriteAction;
    protected NationTypes _nation;

    protected List<T> _originalList;
    protected int _sortType;
    protected List<int> _sortIndexes;
    [SerializeField] protected List<RecordItemBase> _recordObjectList;

    [SerializeField] Button _conditionButton;   
    [SerializeField] protected SelectConditionDialog _selectConditionDialog;

    public int ItemCount { get { return _recordObjectList == null ? 0 : _recordObjectList.Count; } }

    public virtual int StartType() { return 0; }

    protected List<RecordCondition> _conditionList;
    public virtual List<RecordCondition> DefaultCondition() {
        return new List<RecordCondition>();
    }

    protected bool _isRegular;
    protected virtual bool CheckRegular(T player) { return true; }

    protected int GetGameCount(int teamId) {
        return _getGameCountAction == null ? 0 : _getGameCountAction(teamId);
    }

    protected Sprite GetSprite(int teamId) {
        return _getSpriteAction == null ? null : _getSpriteAction(teamId);
    }

    public void Ready(Action<PlayerBase> itemAction, NationTypes nation, Func<int, int> getGameCountAction, Func<int, Sprite> getSpriteAction) {
        _nation = nation;
        _getGameCountAction = getGameCountAction;
        _getSpriteAction = getSpriteAction;

        for(int i = 0; i < _recordObjectList.Count; i++) {
            _recordObjectList[i].Ready(
                clickAction:itemAction,
                getLogoAction:_getSpriteAction
            );
        }

        if(_pagePager != null) {
            _pagePager.PageChanged += PageChanged;
        }
        _sortType = StartType();
        _conditionList = DefaultCondition();
        _isRegular = true;

        if(_conditionButton) {
            _conditionButton.onClick.RemoveAllListeners();
            _conditionButton.onClick.AddListener(() => {
                if(_selectConditionDialog != null) {
                    _selectConditionDialog.TurnActive(true);
                }
            });
        }

        if(_selectConditionDialog != null) {
            _selectConditionDialog.Ready();
        }
    }

    public void SetData(List<T> dataList) {
        _originalList = dataList;
        Sort();
    }

    protected void PageChanged(int page = 0) {
        if(Utils.NotNull(_pagePager, _viewList, _sortIndexes, _recordObjectList)
            && _viewList.Count > 0
            && _sortIndexes.Count > 0
            && _viewList.Count == _sortIndexes.Count
        ) {
            int itemCount = ItemCount;
            int count = _viewList.Count - page * itemCount;

            if(count > itemCount) {
                count = itemCount;
            }

            SetDataList(
                playerList:_viewList.GetRange(page * itemCount, count),
                indexes : _sortIndexes.GetRange(page * itemCount, count)
            );
        }
    }

    void SetDataList(List<T> playerList, List<int> indexes) {
        if(Utils.NotNull(playerList, indexes)) {
            int i = 0;
            for(i = 0; i < ItemCount && i < playerList.Count && i < indexes.Count; i++) {
                _recordObjectList[i].SetData(player:playerList[i], rank:indexes[i]);
            }

            for(; i < ItemCount; i++) {
                _recordObjectList[i].SetData(player:null);
            }
        }
    }

    public void ChangeOrderBy(int statType) {
        if(_sortType != statType) {
            _sortType = statType;
            if(_pagePager != null) {
                PageChanged(page:_pagePager.CurrentPage);
            }
        }
    }

    bool CheckConditions(T player) {
        bool result = player != null;
        if(_conditionList != null && result) {
            for(int i = 0; i < _conditionList.Count; i++) {
                if(_conditionList[i] != null) {
                    result = _conditionList[i].CheckValue(
                        player.GetStat(statType:_conditionList[i].Type)
                    );
                }
            }
        }
        return result;
    }
    
    public void Sort() {
        if(_originalList != null) {
            if(_viewList == null) {
                _viewList = new List<T>();
            }
            else {
                _viewList.Clear();
            }

            for(int i = 0; i < _originalList.Count; i++) {
                if((_isRegular && CheckRegular(_originalList[i])) && CheckConditions(_originalList[i])) {
                    _viewList.Add(_originalList[i]);
                }
            }
            // _viewList = _originalList;
            _sortIndexes = new List<int>();

            if(_sortIndexes != null && _viewList != null && _viewList.Count > 0) {
                bool isBiggerStatGood = IsBiggerStatGood(_sortType);

                _viewList = isBiggerStatGood ?
                    _viewList.OrderByDescending(player => player.GetStat(_sortType)).ToList()
                    : _viewList.OrderBy(player => player.GetStat(_sortType)).ToList();

                double formerStat = _viewList[0].GetStat(_sortType);
                int formerRank = 1, checkRank = 1;
                for(int j = 0; j < _viewList.Count; j++) {
                    double currentStat = _viewList[j].GetStat(_sortType);
                    if(isBiggerStatGood) {
                        if(formerStat > currentStat) {
                            formerStat = currentStat;
                            formerRank = checkRank;
                        }
                    }
                    else if(formerStat < currentStat) {
                        formerStat = currentStat;
                        formerRank = checkRank;
                    }
                    checkRank++;
                    _sortIndexes.Add(formerRank);
                }
            }
            
            if(_pagePager != null) {
                _pagePager.InitObject(totalPage: ((_viewList.Count - 1) / ItemCount) + 1);
            }  
        }
        Debug.Log("Check Player " + _originalList.Count.ToString() + " ? " + _viewList.Count.ToString());
    }

    protected virtual bool IsBiggerStatGood(int statType) {
        return true;
    }
}
