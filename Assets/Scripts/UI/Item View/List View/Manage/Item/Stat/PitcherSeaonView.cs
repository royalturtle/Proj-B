using UnityEngine;
using TMPro;

public class PitcherSeaonView : StatViewBase {
    [SerializeField] TextMeshProUGUI _game, _inning, _win, _lose, _hold, _save, _k, _era, _whip;

    public override void SetData(PlayerBase player) {
        if(player != null && player is Pitcher) {
            Pitcher pitcher = (Pitcher)player;
            SetText(_game, pitcher.Season.G);
            SetText(_inning, Utils.doubleToString(pitcher.Season.IP, round:1));
            SetText(_win, pitcher.Season.W);
            SetText(_lose, pitcher.Season.L);
            // SetText(_hold, pitcher.Season.Holds);
            SetText(_save, pitcher.Season.SV);
            SetText(_k, pitcher.Season.SO);
            SetText(_era, Utils.doubleToString(pitcher.Season.ERA, round:2));
            SetText(_whip, Utils.doubleToString(pitcher.Season.WHIP, round:2));
        }
    }
}
