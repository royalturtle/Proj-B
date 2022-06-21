using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Group1CountText : MonoBehaviour {
    [SerializeField] CurrentTotalText _totalText, _pitcherTotalText;

    public void SetCurrentTotalText(
        int count,
        int pitcherCount,
        bool isTotal
    ) {
        if(_totalText) {
            _totalText.SetData(value:count, isTotal:isTotal);
        }
        if(_pitcherTotalText) {
            _pitcherTotalText.SetData(value:count, isTotal:isTotal);
        }
    }
}
