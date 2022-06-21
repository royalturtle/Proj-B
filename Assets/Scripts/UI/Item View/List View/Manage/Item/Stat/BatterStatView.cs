using UnityEngine;
using TMPro;

public class BatterStatView : StatViewBase {
    [SerializeField] TextMeshProUGUI _hit, _eye, _power, _gapPower, _avoidK, _speed, _defense, _defensePosition;

    public override void SetData(PlayerBase player) {
        if(player != null && player is Batter) {
            Batter batter = (Batter)player;
            TupleBatterStats stats = ((Batter)player).Stats;

            if(stats != null) {
                SetText(_hit, stats.Hit, isColor:true);
                SetText(_eye, stats.Eye, isColor:true);
                SetText(_power, stats.Power, isColor:true);
                SetText(_gapPower, stats.GapPower, isColor:true);
                SetText(_avoidK, stats.AvoidK, isColor:true);
                SetText(_speed, stats.Speed, isColor:true);
                // SetText(_defense, stats.);
                // SetText(_defensePosition, stats.);
            }
        }
    }
}
