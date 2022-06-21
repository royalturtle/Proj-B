using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SaveView : OptionView {
    [SerializeField] GameObject _emptyObj;
    [SerializeField] TextMeshProUGUI _indexLabel, _managerNameText, _dateText;

    public Toggle m_toggle {get; private set;}
    [field:SerializeField] public int Index { get; private set; }

    void Awake() {
        if (_indexLabel != null) {
            _indexLabel.text = Index.ToString();
        }

        _layoutImage = GetComponent<Image>();

        // Toggle Event
        m_toggle = GetComponent<Toggle>();
        m_toggle.onValueChanged.AddListener(delegate{
            SetColor(isOn: m_toggle.isOn);
        });
    }

    public void SetData(SaveData saveData) {
        if(saveData != null) {
            if(_managerNameText != null) { _managerNameText.text = saveData.ManagerName; }

            if(_dateText) {
                DateObj date = new DateObj(year:saveData.Year, turn:saveData.Turn);
                _dateText.text = date.YYYYMMDDString;
            }

            _emptyObj.SetActive(true);
            // if(m_toggle) { m_toggle.interactable = true; }
        }
        else {
            _emptyObj.SetActive(false);
            // if(m_toggle) { m_toggle.interactable = false; }
        }
    }

    void SetColor(bool isOn) {
        _layoutImage.color = isOn ? new Color32(0, 147, 123, 255) : new Color32(82, 82, 82, 255);
    }

    public void SetIndex(int index) {
        Index = index;

        if(_indexLabel != null) {
            _indexLabel.text = (Index == 0) ? "ÃÖ±Ù" : " ¹ø";
        }
    }

    public bool IsEmpty {
        get { return (_emptyObj == null) ? true : !_emptyObj.activeSelf; }
    }
}
