
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuickMatchOptionToggle : MonoBehaviour {
    [SerializeField] TextMeshProUGUI _dateText;
    Toggle _toggle;
    
    public int TurnCount { get; private set; }
    public string Date { get; private set; }

    void Awake() {
        _toggle = GetComponent<Toggle>();
    }

    public void SetData(int turnCount, string date) {
        if(_toggle) {
            _toggle.interactable = true;
        }

        TurnCount = turnCount;

        Date = date;
        if(_dateText) {
            _dateText.text = Date;
        }
    }
    
    public void SetDiabled() {
        if(_toggle) {
            _toggle.interactable = false;
        }
        if(_dateText) {
            _dateText.text = "";
        }
    }

}
