using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TradeResult : MonoBehaviour {
    [SerializeField] Button _button;
    [SerializeField] TextMeshProUGUI _text;
    [SerializeField] Image _image;

    public void Ready(System.Action clickAction) {
        if(_button) {
            _button.onClick.RemoveAllListeners();
            _button.onClick.AddListener(() => {
                if(clickAction != null) {
                    clickAction();
                }
            });
        }
    }
}
