using UnityEngine;
using TMPro;

public class StatTextView : MonoBehaviour {
    [SerializeField] TextMeshProUGUI _statText;

    public void SetData(int value, bool isColor = false) {
        if(_statText) {
            _statText.text = value.ToString();

            if(isColor) {
                _statText.color = ColorConstants.Instance.GetColorOfStat(value);
            }
        }
    }

    public void SetData(float value, bool isColor = false) {
        if(_statText) {
            _statText.text = ((int)value).ToString();
            
            if(isColor) {
                _statText.color = ColorConstants.Instance.GetColorOfStat(value);
            }
        }
    }

    public void SetData(double value, bool isColor = false) {
        if(_statText) {
            _statText.text = ((int)value).ToString();
            
            if(isColor) {
                _statText.color = ColorConstants.Instance.GetColorOfStat(value);
            }
        }
    }

    public void SetData(Hands hand) {
        if(_statText) {
            _statText.text = hand.ToString();
        }
    }
}
