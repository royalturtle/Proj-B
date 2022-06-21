using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MatchCurrentBatterView : MatchCurrentPlayerView {
    [SerializeField] TextMeshProUGUI _name;    
    [SerializeField] List<ExternalText> _statList;
    [SerializeField] List<ExternalText> _seasonList;

    public void SetData(Batter batter) {
        if(batter != null) {
            if(batter.Base != null) {
                if(_name) { _name.text = batter.Base.Name; }
            }
            if(batter.Stats != null) {
                _statList[0].SetData(((int)batter.Stats.Energy).ToString());
                _statList[1].SetData(batter.Base.HitHand.ToString());
                _statList[2].SetData(((int)batter.Stats.Hit).ToString());
                _statList[3].SetData(((int)batter.Stats.Eye).ToString());
                _statList[4].SetData(((int)batter.Stats.Power).ToString());
                _statList[5].SetData(((int)batter.Stats.GapPower).ToString());
                _statList[6].SetData(((int)batter.Stats.Speed).ToString());
                // _statList[7].SetData(batter.Stats.Energy.ToString());
                // _statList[8].SetData(batter.Stats.Energy.ToString());
            }
            if(batter.Season != null) {

                _seasonList[0].SetData(Utils.floatToString(batter.Season.AVG, 3));
                _seasonList[1].SetData(batter.Season.HR.ToString());
                _seasonList[2].SetData(Utils.floatToString(batter.Season.OBP, 3));
                _seasonList[3].SetData(Utils.floatToString(batter.Season.SLG, 3));
                _seasonList[4].SetData(Utils.floatToString(batter.Season.OPS, 3));
                // _seasonList[5].SetData(batter.Stats.GapPower.ToString());
                // _seasonList[6].SetData(batter.Stats.Speed.ToString());
                // _seasonList[7].SetData(batter.Stats.Energy.ToString());
                // _seasonList[8].SetData(batter.Stats.Energy.ToString());
                // _seasonList[9].SetData(batter.Stats.Energy.ToString());
            }
        }
    }
}
