using UnityEngine;
using TMPro;

public class LocalizeLangScript : MonoBehaviour {
    void Start() {
        LocalizeChanged();
        LanguageSingleton.Instance.LocalizeChanged += LocalizeChanged;
    }

    void OnDestroy() {
        LanguageSingleton.Instance.LocalizeChanged -= LocalizeChanged;
    }

    void LocalizeChanged() {
        TextMeshProUGUI text = GetComponent<TextMeshProUGUI>();
        if(text != null) {
            text.text = LanguageSingleton.Instance.CurrentLanguage;
        }
    }
}
