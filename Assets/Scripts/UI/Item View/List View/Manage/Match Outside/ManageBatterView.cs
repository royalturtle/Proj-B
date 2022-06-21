using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageBatterView : ManagePlayerView<Batter> {
    LineupBatter _lineupBatter;
    Action<Batter, BatterPositionEnum> _changePositionAction;

    public override void SetData(GameDataMediator gameData, int teamId, bool isUpdate = false) {
        if(gameData != null) {
            SetData(
                lineUp   : gameData.Lineup.GetLineupBatter(teamId), 
                group2   : gameData.Group2,
                isUpdate : isUpdate
            );
        }
    } 
    
    public void SetData(LineupBatter lineUp, Group2DataMediator group2 = null, bool isUpdate = false) {
        _lineupBatter = lineUp;

        if(_lineupBatter != null) {
            // 필요한 List 생성
            _viewList = new List<Batter>();
            _positionList = new List<int>();
            _orderList = new List<int>();

            // 선발 등록
            int orderIndex = 0;
            for(int i = 0; i < _lineupBatter.Playings.Count; i++) {
                _viewList.Add(_lineupBatter.Playings[i]);
                _positionList.Add((int)_lineupBatter.DefenseList[orderIndex]);
                _orderList.Add(++orderIndex);
            }

            // 후보 등록
            orderIndex = 0;
            for(int i = 0; i < _lineupBatter.Candidates.Count; i++) {
                _viewList.Add(_lineupBatter.Candidates[i]);
                _positionList.Add((int)BatterPositionEnum.CANDIDATE);
                _orderList.Add(++orderIndex);
            }

            // 2군 등록
            if(group2 != null) {
                foreach(Batter batter in _lineupBatter.Group2) {
                    _viewList.Add(batter);
                    _positionList.Add((int)BatterPositionEnum.GROUP2);
                    _orderList.Add((-1 * group2.RemainDays(batter.Base.ID)));
                }
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

    protected override bool IsPositionChangeable(int position) {
        return (int)BatterPositionEnum.C <= position && position <= (int)BatterPositionEnum.DH;
    }
}
