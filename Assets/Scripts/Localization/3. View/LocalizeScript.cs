using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LocalizeScript : MonoBehaviour {
    [SerializeField] LocalizationTypes Type;

    public int textKey;      //0번째 열(영어 데이터)을 기준으로 key를 담을 문자열 선언 
    // public int[] dropdownKey;    //만약 text가 아닌 드랍다운일 경우
    public bool IsNewLine = true;

    private void Start() {
        LocalizeChanged();
        LanguageSingleton.Instance.LocalizeChanged += LocalizeChanged;
    }

    private void OnDestroy() {
        LanguageSingleton.Instance.LocalizeChanged -= LocalizeChanged;
    }

    //어떤 문자열이 필요하진 key를 매개변수로 받는다 
    // string Localize(string key)
    protected string Localize(int key) {
        // int keyIndex = LanguageSingleton.Instance.Langs[0].value.FindIndex(x => x.ToLower() == key.ToLower()); //기준이 되는 0번 기준으로 value 탐색 후 keyIndex에 문자열 선언      
        return LanguageSingleton.Instance.Data[Type][key];      //현재 언어 인덱스, value의 key를 기준으로 문자열을 반환한다. 
    }

    public void SetData(int value) {
        textKey = value;
        LocalizeChanged();
    }

    public void SetData(LocalizationTypes type, int value) {
        Type = type;
        textKey = value;
        LocalizeChanged();
    }

    protected virtual void LocalizeChanged() {
        if (GetComponent<TextMeshProUGUI>() != null) {
            GetComponent<TextMeshProUGUI>().text = IsNewLine ? Localize(textKey).Replace("\\n", "\n") : Localize(textKey);
        }
        /*
        else if (GetComponent<TMP_Dropdown>() != null) {
            TMP_Dropdown dropdown = GetComponent<TMP_Dropdown>();
            dropdown.captionText.text = Localize(dropdownKey[dropdown.value]);

            for (int i = 0; i < dropdown.options.Count; i++) {
                dropdown.options[i].text = Localize(dropdownKey[i]);
            }
        }
        */
    }
}