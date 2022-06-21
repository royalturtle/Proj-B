using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextBatterListObject : MonoBehaviour {
    [SerializeField] List<NextBatterItemObject> _contentsList;
    [SerializeField] Animator _clearAnimationObj;

    InningOrder registeredInningOrder;
    int BatterOrder;

    void Awake() {
        registeredInningOrder = InningOrder.POST;
    }

    public void SetData(List<Batter> batters) {
        ClearAnimationAppear();
        for (int i = 0; i < batters.Count; i++) {
            _contentsList[i].SetData(
                rank : Utils.GetStringOfGrade(batters[i].Stats.Potential), 
                hand : batters[i].Base.HitHand
            );
        }
        ClearAnimationDisappear();
    }

    void ClearAnimationAppear() {
        if(_clearAnimationObj != null) { _clearAnimationObj.SetBool("clearAppear", true); }
    }

    void ClearAnimationDisappear() {
        if (_clearAnimationObj != null) { _clearAnimationObj.SetBool("clearAppear", false); }
    }

    public void SetBatterOrder(int batterOrder) {
        if(BatterOrder != batterOrder) {
            if(BatterOrder > 0) { _contentsList[BatterOrder - 1].DisactivateSelected(); }
            BatterOrder = batterOrder;
            _contentsList[BatterOrder - 1].ActivateSelected();
        }
    }

    public bool InningOrderShouldChange(InningOrder inningOrder) {
        if (inningOrder != registeredInningOrder) {
            registeredInningOrder = inningOrder;
            return true;
        }
        else return false;
    }

}
