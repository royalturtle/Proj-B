using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MatchCurrentPitcherView : MatchCurrentPlayerView {
    [SerializeField] TextMeshProUGUI _name;    
    [SerializeField] List<ExternalText> _statList;
    [SerializeField] List<ExternalText> _seasonList;

    public void SetData(Pitcher pitcher) {
        if(pitcher != null) {
            if(pitcher.Base != null) {
                if(_name) { _name.text = pitcher.Base.Name; }
            }
            if(pitcher.Stats != null) {
                _statList[0].SetData(((int)pitcher.Stats.Energy).ToString());
                _statList[1].SetData(pitcher.Base.ThrowHand.ToString());
                _statList[2].SetData(((int)pitcher.Stats.Stamina).ToString());
                _statList[3].SetData(((int)pitcher.Stats.Velocity).ToString());
                _statList[4].SetData(((int)pitcher.Stats.KMov).ToString());
                _statList[5].SetData(((int)pitcher.Stats.Stuff).ToString());
                _statList[6].SetData(((int)pitcher.Stats.Control).ToString());
                _statList[7].SetData(((int)pitcher.Stats.GMov).ToString());
                _statList[8].SetData(((int)pitcher.Stats.Composure).ToString());
                _statList[9].SetData(((int)pitcher.Stats.LOpp).ToString());
                _statList[10].SetData(((int)pitcher.Stats.ROpp).ToString());
            }
            if(pitcher.Season != null) {
                _seasonList[0].SetData(Utils.doubleToString(pitcher.Season.ERA, 3));
                _seasonList[1].SetData(Utils.doubleToString(pitcher.Season.WHIP, 3));
                // _seasonList[5].SetData(pitcher.Stats.GapPower.ToString());
                // _seasonList[6].SetData(pitcher.Stats.Speed.ToString());
                // _seasonList[7].SetData(pitcher.Stats.Energy.ToString());
                // _seasonList[8].SetData(pitcher.Stats.Energy.ToString());
                // _seasonList[9].SetData(pitcher.Stats.Energy.ToString());
            }
        }
    }
}
