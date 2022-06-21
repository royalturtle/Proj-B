using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TeamRecordView : FullPanel {
    [SerializeField] protected List<TeamRecordListItem> _itemList;
    [SerializeField] protected BasePager _pager, _modePager;

    [SerializeField] List<GameObject> _headerList;

    protected List<List<TeamSeason>> _teamList;
    protected List<List<int>> _rankList;
    protected int ItemCount { get {return _itemList == null ? 0 : _itemList.Count;}}

    TeamStatType _sortType;

    public void Ready(Action<TeamSeason> clickAction) {
        _sortType = TeamStatType.WinRate;

        for(int i = 0; i < _itemList.Count; i++) {
            _itemList[i].Ready(clickAction:clickAction);
        }

        if(_pager != null) {
            _pager.PageChanged += SetPage;
        }

        if(_modePager != null) {
            _modePager.PageChanged += SetDetailPage;
            _modePager.InitObject(totalPage: 3);
        }
    }

    void Sort() {
        _rankList = new List<List<int>>();
        // Check
        if(_teamList != null && _teamList.Count > 0 && _rankList != null) {

            for(int i = 0; i < _teamList.Count; i++) {
                List<int> newRankList = new List<int>();

                if(newRankList != null && _teamList[i] != null && _teamList[i].Count > 0) {
                    bool isBiggerStatGood = TeamSeason.IsBiggerStatGood(_sortType);

                    _teamList[i] = isBiggerStatGood ?
                        _teamList[i].OrderByDescending(team => team.GetStat(_sortType)).ToList()
                        : _teamList[i].OrderBy(team => team.GetStat(_sortType)).ToList();

                    double formerStat = _teamList[i][0].GetStat(_sortType);
                    int formerRank = 1, checkRank = 1;
                    for(int j = 0; j < _teamList[i].Count; j++) {
                        double currentStat = _teamList[i][j].GetStat(_sortType);
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
                        newRankList.Add(formerRank);
                    }
                }

                _rankList.Add(newRankList);
            }
        }
    }

    public void ChangeOrderBy(TeamStatType type) {
        if(_sortType != type) {
            _sortType = type;
            if(_pager != null) {
                SetPage(page:_pager.CurrentPage);
            }
        }
    }

    public void SetData(GameDataMediator gameData, int year = -1) {
        if(gameData != null) {
            _teamList = gameData.Seasons.GetSeaonList(year);
            Sort();

            int count = _teamList == null ? 0 : _teamList.Count;

            if(_pager) {
                _pager.gameObject.SetActive(count > 1);
                _pager.InitObject(totalPage: count);
            }
        }
    }

    void SetPage(int page) {
        if(_pager != null) {
            int itemCount = ItemCount;
            int i = 0;

            List<TeamSeason> _viewList = _teamList[page];

            for(i = 0; i < itemCount && i < _viewList.Count; i++) {
                _itemList[i].SetData(_viewList[i], _rankList[page][i]);
            }

            for(; i < itemCount; i++) {
                _itemList[i].SetData(null);
            }
        }
    }

    void SetDetailPage(int index) {
        int itemCount = ItemCount;
        for(int i = 0; i < itemCount; i++) {
            _itemList[i].SetDetailPage(index);
        }
        if(_headerList != null) {
            for(int i = 0; i < _headerList.Count; i++) {
                _headerList[i].SetActive(i == index);
            }
        }
    }
    
    public void CheckScroll(BaseEventData eventData) {
        if(eventData != null && eventData is PointerEventData &&  _teamList != null && _teamList.Count >= 1) {
            Vector2 vec = ((PointerEventData)eventData).delta;
            bool isVertical = (Mathf.Abs(vec.y) >= Mathf.Abs(vec.x));
            BasePager pageManager = isVertical ? _pager : _modePager;

            if(pageManager != null) {
                float value = isVertical ? vec.y : vec.x;
                if(value < 0.0f) {
                    pageManager.PreviousPage();
                }
                else if(value > 0.0f) {
                    pageManager.NextPage();
                }
            }
        }
    }
}
