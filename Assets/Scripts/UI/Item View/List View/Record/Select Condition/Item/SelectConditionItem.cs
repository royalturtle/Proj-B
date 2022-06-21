using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SelectConditionItem : MonoBehaviour {
    Button _button;

    void Awake() {
        _button = GetComponent<Button>();
    }

    [SerializeField] LocalizeScript _labelText;
    public int Value {get; private set;}


    public void SetLabel(LocalizationTypes key, int value) {
        bool isActive = value >= 0 && key >= 0;
        Value = value;
        if(isActive && _labelText != null) {
            _labelText.SetData(type:key, value:value);
        }

        gameObject.SetActive(isActive);
    }
}
