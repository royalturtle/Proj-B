using UnityEngine;
using TMPro;

public class DateView : MonoBehaviour {
    [SerializeField] TextMeshProUGUI _text;

    public void SetData(DateObj date) {
        if (_text != null) { _text.text = date.YYYYAMADDString; }
    }
}
