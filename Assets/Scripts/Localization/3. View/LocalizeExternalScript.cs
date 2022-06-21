using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LocalizeExternalScript : MonoBehaviour {
    [SerializeField] LocalizationTypes Type;
    [SerializeField] TextMeshProUGUI _text;

    public int textKey;      //0��° ��(���� ������)�� �������� key�� ���� ���ڿ� ���� 
    public bool IsNewLine = true;

    private void Start() {
        LocalizeChanged();
        LanguageSingleton.Instance.LocalizeChanged += LocalizeChanged;
    }

    private void OnDestroy() {
        LanguageSingleton.Instance.LocalizeChanged -= LocalizeChanged;
    }

    //� ���ڿ��� �ʿ����� key�� �Ű������� �޴´� 
    protected string Localize(int key) {
        return LanguageSingleton.Instance.Data[Type][key];      //���� ��� �ε���, value�� key�� �������� ���ڿ��� ��ȯ�Ѵ�. 
    }

    protected virtual void LocalizeChanged() {
        if (_text != null) {
            _text.text = IsNewLine ? Localize(textKey).Replace("\\n", "\n") : Localize(textKey);
        }
    }
}
