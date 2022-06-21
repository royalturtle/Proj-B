using UnityEngine;
using TMPro;

public class CheckDialog : DialogBase {
    [SerializeField] TextMeshProUGUI _text;
    
    public void Open(ErrorType errorType) {
        if(errorType != ErrorType.None) {
            if(_text) {
                _text.text = LanguageSingleton.Instance.GetErrorString(errorType);
            }

            TurnActive(true);
        }
        
    }
}
