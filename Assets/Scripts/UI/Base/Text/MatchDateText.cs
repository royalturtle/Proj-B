using UnityEngine;
using TMPro;

public class MatchDateText : MonoBehaviour {
    TextMeshProUGUI _textObject;

    void Awake() {
        _textObject = GetComponent<TextMeshProUGUI>();
    }
    
    public void SetData(string date, DaysEnum dayName) {
        if(_textObject) {
            _textObject.text = date + "(" + LanguageSingleton.Instance.GetDayName(dayName) + ")";
        }
    }
}
