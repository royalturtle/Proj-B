using UnityEngine;
using UnityEngine.UI;

public class OptionView : MonoBehaviour {
    protected Image _layoutImage;
    Toggle _toggle;

    public bool isOn {
        get {
            return (_toggle != null) && (_toggle.isOn);
        }
    }

    void Awake() {
        _layoutImage = GetComponent<Image>();
        _toggle = GetComponent<Toggle>();
        _toggle.onValueChanged.AddListener(delegate{
            SetColor(isOn: _toggle.isOn);
        });
    }

    void SetColor(bool isOn) {
        if(_layoutImage) {
            _layoutImage.color = isOn ? new Color32(0, 147, 123, 255) : new Color32(51, 51, 51, 255);
        }
    }
}
