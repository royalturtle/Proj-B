using UnityEngine;
using TMPro;

public class BatterSeasonView : StatViewBase {
    [SerializeField] TextMeshProUGUI _pa, _hr, _sb, _avg, _obp, _slg, _ops, _e;

    public override void SetData(PlayerBase player){
        if(player != null && player is Batter) {
            TupleBatterSeason season = ((Batter)player).Season;

            if(season != null) {
                SetText(_pa, season.PA);
                SetText(_hr, season.HR);
                SetText(_sb, season.SB);
                SetText(_avg, Utils.doubleToString(season.AVG, round:3));
                SetText(_obp, Utils.doubleToString(season.OBP, round:3));
                SetText(_slg, Utils.doubleToString(season.SLG, round:3));
                SetText(_ops, Utils.doubleToString(season.OPS, round:3));
                SetText(_e, season.E);
            }
        }
    }
}
