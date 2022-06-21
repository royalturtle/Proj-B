using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FullPanel : MonoBehaviour {
    [SerializeField] protected Button _backButton, _closeButton;
    [SerializeField] protected TextMeshProUGUI _titleText;

    public void SetTopBarActions(Action backAction, Action closeAction) {
        if(_backButton) {
            _backButton.onClick.RemoveAllListeners();
            _backButton.onClick.AddListener(() => {
                BeforeClose();
                if(backAction != null) {
                    backAction();
                }
            });
        }

        if(_closeButton) {
            _closeButton.onClick.RemoveAllListeners();
            _closeButton.onClick.AddListener(() => {
                BeforeClose();
                if(backAction != null) {
                    closeAction();
                }
            });
        }
    }

    public virtual void BeforeClose() {}

    public void SetBackButton(bool value) {
        /*
        if(_backButton) {
            _backButton.gameObject.SetActive(value);
        }*/
    }

}
