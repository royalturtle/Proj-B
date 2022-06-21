using UnityEngine;
using TMPro;

public class ScoutingStatView : StatViewBase {
    [SerializeField] TextMeshProUGUI _salary, _fa, _externalEvaluation, _internalEvaluation, _scouted;

    public override void SetData(PlayerBase player) {
        if(player != null) {
            TupleLivePlayer liveTuple = null;
            if(player is Batter) {
                liveTuple = ((Batter)player).Stats;
            }
            else if(player is Pitcher) {
                liveTuple = ((Pitcher)player).Stats;
            }

            if(liveTuple != null) {
                SetText(_salary, liveTuple.Salary);
                SetText(_fa, liveTuple.FAYear);
            }
        }
        
        
    }
}
