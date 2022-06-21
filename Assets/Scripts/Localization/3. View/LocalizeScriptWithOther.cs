using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LocalizeScriptWithOther : LocalizeScript {
    public string Prefix;
    public string Postfix;

    protected override void LocalizeChanged() {
        if (GetComponent<TextMeshProUGUI>() != null) {
            // GetComponent<TextMeshProUGUI>().text = Prefix + Localize(textKey) + Postfix;
            GetComponent<TextMeshProUGUI>().text = Prefix + Localize(textKey);
        }
    }

    public void SetData(int key, string prefix, string postfix) {
        textKey = key;
        Prefix = prefix;
        Postfix = postfix;

        if (GetComponent<TextMeshProUGUI>() != null) {
            // GetComponent<TextMeshProUGUI>().text = Prefix + Localize(textKey) + Postfix;
            GetComponent<TextMeshProUGUI>().text = Prefix + Localize(textKey);
        }
    }
}
