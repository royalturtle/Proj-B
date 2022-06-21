using UnityEngine;
using TMPro;

public class CurrentTotalText : MonoBehaviour {
    public bool IsActive {
        get {
            return gameObject.activeSelf;
        }
    }
    [SerializeField] TextMeshProUGUI _currentText, _totalText;

    public void SetData(int value, bool isTotal) {
        if(isTotal) {
            gameObject.SetActive(value > 0);
        }

        TextMeshProUGUI textObj = isTotal ? _totalText : _currentText;
        if(textObj) {
            textObj.text = value.ToString();
        }
    }
}
