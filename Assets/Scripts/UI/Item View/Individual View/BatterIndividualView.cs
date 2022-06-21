using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatterIndividualView : PlayerIndividualView<Batter> {
    [SerializeField] StatTextView _hitStat, _eyeStat, _powerStat, _gapPowerStat, _avoidKStat, _speedStat;
    [SerializeField] StatTextView _defStatC, _defStatB1, _defStatB2, _defStatB3, _defStatSS, _defStatOF;
    [SerializeField] List<ExternalText> _baseSeasonList;

    public override void SetData(int index = -1) {
        if(_player != null) {
            _playerIndex = (index == GameConstants.NULL_INT) ? GetIndexOfPlayerId(_player.Base.ID) : index;
            if(_player.Base != null) {
                if(_titleText) {
                    _titleText.text = _player.Base.Name;
                }
            }
            if(_basicView != null) {
                _basicView.SetData(liveData:_player.Stats, baseData:_player.Base);
            }
            if(_player.Stats != null) {
                SetTeamData(_player.Stats.TeamID);
                _hitStat.SetData(_player.Stats.Hit, isColor:true);
                _eyeStat.SetData(_player.Stats.Eye, isColor:true);
                _powerStat.SetData(_player.Stats.Power, isColor:true);
                _gapPowerStat.SetData(_player.Stats.GapPower, isColor:true);
                _avoidKStat.SetData(_player.Stats.AvoidK, isColor:true);
                _speedStat.SetData(_player.Stats.Speed, isColor:true);

                _defStatC.SetData(_player.Stats.DefenseC, isColor:true);
                _defStatB1.SetData(_player.Stats.Defense1B, isColor:true);
                _defStatB2.SetData(_player.Stats.Defense2B, isColor:true);
                _defStatB3.SetData(_player.Stats.Defense3B, isColor:true);
                _defStatSS.SetData(_player.Stats.DefenseSS, isColor:true);
                _defStatOF.SetData(_player.Stats.DefenseOF, isColor:true);
            }
            if(_player.Season != null) {
                _baseSeasonList[0].SetData(_player.Season.G.ToString());
                _baseSeasonList[1].SetData(_player.Season.PA.ToString());
                _baseSeasonList[2].SetData(_player.Season.AB.ToString());
                _baseSeasonList[3].SetData(_player.Season.H.ToString());
                _baseSeasonList[4].SetData(_player.Season.R.ToString());
                _baseSeasonList[5].SetData(_player.Season.RBI.ToString());
                _baseSeasonList[6].SetData(_player.Season.BB.ToString());
                _baseSeasonList[7].SetData(_player.Season.SO.ToString());
                _baseSeasonList[8].SetData(_player.Season.HR.ToString());
                _baseSeasonList[9].SetData(_player.Season.SB.ToString());
                _baseSeasonList[10].SetData(Utils.floatToString(_player.Season.AVG, 3));
                _baseSeasonList[11].SetData(Utils.floatToString(_player.Season.OBP, 3));
                _baseSeasonList[12].SetData(Utils.floatToString(_player.Season.SLG, 3));
                _baseSeasonList[13].SetData(Utils.floatToString(_player.Season.OPS, 3));
            }
        }

    }
}
