using UnityEngine;
using TMPro;

public class ExternalText : MonoBehaviour {
    [SerializeField] TextMeshProUGUI _text;

    public void SetData(string value) {
        if(_text) {
            _text.text = value;
        }
    }
}
