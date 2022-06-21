using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LocalizeExternalScript : MonoBehaviour {
    [SerializeField] LocalizationTypes Type;
    [SerializeField] TextMeshProUGUI _text;

    public int textKey;      //0번째 열(영어 데이터)을 기준으로 key를 담을 문자열 선언 
    public bool IsNewLine = true;

    private void Start() {
        LocalizeChanged();
        LanguageSingleton.Instance.LocalizeChanged += LocalizeChanged;
    }

    private void OnDestroy() {
        LanguageSingleton.Instance.LocalizeChanged -= LocalizeChanged;
    }

    //어떤 문자열이 필요하진 key를 매개변수로 받는다 
    protected string Localize(int key) {
        return LanguageSingleton.Instance.Data[Type][key];      //현재 언어 인덱스, value의 key를 기준으로 문자열을 반환한다. 
    }

    protected virtual void LocalizeChanged() {
        if (_text != null) {
            _text.text = IsNewLine ? Localize(textKey).Replace("\\n", "\n") : Localize(textKey);
        }
    }
}
