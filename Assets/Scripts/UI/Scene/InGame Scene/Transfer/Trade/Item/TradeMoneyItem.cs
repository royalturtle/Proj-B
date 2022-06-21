using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TradeMoneyItem : MonoBehaviour {
    public int Money {get; private set;}
    Button _button;
    [SerializeField] TextMeshProUGUI _text;
    [SerializeField] MoneyDialog _moneyDialog;

    void CheckButton() {
        if(_button == null) { 
            _button = GetComponent<Button>();
        }
    }

    void Awake() {
        CheckButton();
        if(_button) {
            _button.onClick.RemoveAllListeners();
            _button.onClick.AddListener(() => {
                if(_moneyDialog) {
                    _moneyDialog.Open(money:Money, confirmAction:SetData);
                }
            });
        }
    }

    public void SetData(int money) {
        Money = money;
        if(_text) {_text.text = Money.ToString();}
    }

    public void SetInteractable(bool value) {
        CheckButton();
        if(_button) {_button.interactable = value;}
    }
}
