using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitcherIndividualView : PlayerIndividualView<Pitcher> {
    [SerializeField] StatTextView _staminaStat, _velocityStat, _stuffStat, _strikeStat, _controlStat, _groundStat, _composureStat, _lOppStat, _rOppStat;
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
                _staminaStat.SetData(_player.Stats.Stamina, isColor:true);
                _velocityStat.SetData(_player.Stats.Velocity, isColor:true);
                _stuffStat.SetData(_player.Stats.Stuff, isColor:true);
                _strikeStat.SetData(_player.Stats.KMov, isColor:true);
                _controlStat.SetData(_player.Stats.GMov, isColor:true);
                _groundStat.SetData(_player.Stats.Control, isColor:true);
                _composureStat.SetData(_player.Stats.Composure, isColor:true);
                _lOppStat.SetData(_player.Stats.ROpp);
                _rOppStat.SetData(_player.Stats.LOpp);
            }
            if(_player.Season != null) {
                _baseSeasonList[0].SetData(_player.Season.G.ToString());
                _baseSeasonList[1].SetData(Utils.doubleToString(_player.Season.IP, round:1));
                _baseSeasonList[2].SetData(_player.Season.GS.ToString());
                _baseSeasonList[3].SetData(_player.Season.W.ToString());
                _baseSeasonList[4].SetData(_player.Season.L.ToString());
                _baseSeasonList[5].SetData(_player.Season.ER.ToString());
                _baseSeasonList[6].SetData(_player.Season.SV.ToString());
                _baseSeasonList[7].SetData(_player.Season.SO.ToString());
                _baseSeasonList[8].SetData(_player.Season.H.ToString());
                _baseSeasonList[9].SetData(_player.Season.HR.ToString());
                _baseSeasonList[10].SetData(_player.Season.BB.ToString());
                _baseSeasonList[11].SetData(Utils.doubleToString(_player.Season.ERA, 2));
                _baseSeasonList[12].SetData(Utils.doubleToString(_player.Season.WHIP, 2));
                _baseSeasonList[13].SetData(Utils.doubleToString(_player.Season.FIP, 2));
            }
        }

    }
}
