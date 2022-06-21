using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManagePlayerBaseFullView : FullPanel {
    [SerializeField] protected Button _toBatterButton, _toPitcherButton;
    [SerializeField] protected Image _iconImage;
    [SerializeField] protected Group1CountText _countText;

    public void SetCurrentTotalText(int count, int pitcherCount, bool isTotal) {
        if(_countText) {
            _countText.SetCurrentTotalText(count:count, pitcherCount:pitcherCount, isTotal:isTotal);
        }
    }
}
