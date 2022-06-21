using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BasicStatView : StatViewBase {
    [SerializeField] TextMeshProUGUI _grade, _name, _age, _energy, _fatigue;
    [SerializeField] LocalizeScript  _throwHand, _hitHand;
    [SerializeField] ConditionImage _condition;

    public override void SetData(PlayerBase player) {
        if(player != null && player.Base != null) {
            SetText(_name, player.Base.Name);

            if(_throwHand) {
                _throwHand.SetData((int)player.Base.ThrowHand);
            }

            if(_hitHand) {
                _hitHand.SetData((int)player.Base.HitHand);
            }

            TupleLivePlayer live = null;
            
            if(player is Batter) {live = ((Batter)player).Stats;}
            else if(player is Pitcher) {live = ((Pitcher)player).Stats;}

            if(live != null) {
                // SetText(_grade, live.Potential);
                if(_grade) {
                    _grade.text = Utils.GetStringOfGrade(grade:live.Potential);
                    _grade.color = ColorConstants.Instance.GetColorOfGrade(live.Potential);
                }
                SetText(_age, live.Age);
                SetText(_energy, live.Energy);
                SetText(_fatigue, live.Fatigue);

                if(_condition) {
                    _condition.SetData(live.Condition);
                }
            }
        }
    }
}
