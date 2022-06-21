using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextCopyFontSize : MonoBehaviour {
    [SerializeField] List<TextMeshProUGUI> _otherTextList;
    void Start() {
        TextMeshProUGUI textObject = GetComponent<TextMeshProUGUI>();
        if(textObject != null && _otherTextList != null) {
            float fontSize = textObject.fontSize;
            Debug.Log(fontSize);
            for(int i = 0; i < _otherTextList.Count; i++) {
                _otherTextList[i].fontSize = fontSize;
            }
        }
    }
}
