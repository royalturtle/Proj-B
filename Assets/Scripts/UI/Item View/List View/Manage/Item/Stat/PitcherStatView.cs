using UnityEngine;
using TMPro;

public class PitcherStatView : StatViewBase {
    [SerializeField] TextMeshProUGUI _health, _velocity, _stuff, _k, _control, _groundBall, _composure, _oppL, _oppR;

    public override void SetData(PlayerBase player) {
        if(player != null && player is Pitcher) {
            Pitcher pitcher = (Pitcher)player;
            SetText(_health, pitcher.Stats.Stamina, isColor:true);
            SetText(_velocity, pitcher.Stats.Velocity, isColor:true);
            SetText(_stuff, pitcher.Stats.Stuff, isColor:true);
            SetText(_k, pitcher.Stats.KMov, isColor:true);
            SetText(_control, pitcher.Stats.Control, isColor:true);
            SetText(_groundBall, pitcher.Stats.GMov, isColor:true);
            SetText(_composure, pitcher.Stats.Composure, isColor:true);
            SetText(_oppL, pitcher.Stats.LOpp);
            SetText(_oppR, pitcher.Stats.ROpp);
        }
    }
}
