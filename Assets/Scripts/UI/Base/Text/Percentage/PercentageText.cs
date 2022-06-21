using UnityEngine;
using TMPro;

public class PercentageText : MonoBehaviour {
    [SerializeField] TextMeshProUGUI _percentageText;
    public void SetActive(bool value) {
        gameObject.SetActive(value);
    }

    public void SetValue(double value) {
        if(_percentageText != null) {
            _percentageText.text = ((int)value).ToString();
        }
    }
}
