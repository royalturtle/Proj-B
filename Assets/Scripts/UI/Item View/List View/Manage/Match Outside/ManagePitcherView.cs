using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagePitcherView : ManagePlayerView<Pitcher> {
    LineupPitcher _lineupPitcher;

    protected override bool IsBatter() { return false; }

    public override void SetData(GameDataMediator gameData, int teamId, bool isUpdate = false) {
        if(gameData != null) {
            SetData(
                lineUp: gameData.Lineup.GetLineupPitcher(teamId),
                group2 : gameData.Group2,
                isUpdate:isUpdate
            );
        }
    }

    public void SetData(LineupPitcher lineUp, Group2DataMediator group2 = null, bool isUpdate = false) {
        _lineupPitcher = lineUp;
        if(_lineupPitcher != null) {    
            // 필요한 List 생성
            _viewList = new List<Pitcher>();
            _positionList = new List<int>();
            _orderList = new List<int>();

            // 선발 등록
            int orderIndex = 1;
            foreach(Pitcher pitcher in _lineupPitcher.StartingPitchers) {
                _viewList.Add(pitcher);
                _positionList.Add((int)PitcherPositionEnum.STARTING);
                _orderList.Add(orderIndex++);
            }

            // 마무리 등록
            if(_lineupPitcher.CloserPitcher != null) {
                _viewList.Add(_lineupPitcher.CloserPitcher);
                _positionList.Add((int)PitcherPositionEnum.CLOSER);
                _orderList.Add(0);
            }

            // 셋업 등록
            if(_lineupPitcher.SetupPitcher != null) {
                _viewList.Add(_lineupPitcher.SetupPitcher);
                _positionList.Add((int)PitcherPositionEnum.SETUP);
                _orderList.Add(0);
            }

            // 그외 중계 등록
            orderIndex = 1;
            foreach(Pitcher pitcher in _lineupPitcher.ReliefPitchers) {
                _viewList.Add(pitcher);
                _positionList.Add((int)PitcherPositionEnum.RELIEF);
                _orderList.Add(orderIndex++);
            }

            if(group2 != null) {
                // 2군 등록
                foreach(Pitcher pitcher in _lineupPitcher.Group2) {
                    _viewList.Add(pitcher);
                    _positionList.Add((int)PitcherPositionEnum.GROUP2);
                    // _orderList.Add(0);
                    _orderList.Add(-1 * group2.RemainDays(pitcher.Base.ID));
                }
            }

            // Page Manager
            if(_pagePager != null) {
                _pagePager.InitObject(totalPage: (_viewList.Count - 1) / ItemCount + 1, isUpdate:isUpdate);
            }
        
            if(_categoryPager != null) {
                _categoryPager.InitObject(totalPage: _itemList[0].CategoryCount, isUpdate:isUpdate);
            }
        }
    }

    protected override bool IsPositionChangeable(int position) {
        return (int)PitcherPositionEnum.STARTING <= position && position <= (int)PitcherPositionEnum.CLOSER;
    }
}
