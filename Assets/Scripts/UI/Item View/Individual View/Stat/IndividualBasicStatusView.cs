using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IndividualBasicStatusView : MonoBehaviour {
    [SerializeField] List<StatTextView> _textList;
    [SerializeField] LocalizeScript _throwHandText, _hitHandText;
    [SerializeField] ConditionImage _conditionImage;

    public void SetData(TupleLivePlayer liveData, TuplePlayerBase baseData) {
        if(liveData != null && baseData != null) {
            _textList[0].SetData(liveData.Age);
            _textList[1].SetData(liveData.Energy);
            _textList[2].SetData(liveData.FAYear);
            _textList[3].SetData(liveData.Salary);

            if(_throwHandText != null) {
                _throwHandText.SetData(value:(int)baseData.ThrowHand);
            }

            if(_hitHandText != null) {
                _hitHandText.SetData(value:(int)baseData.HitHand);
            }

            if(_conditionImage) {
                _conditionImage.SetData(condition:liveData.Condition);
            }
        }
    }
}
