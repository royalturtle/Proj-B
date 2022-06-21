using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AskDialog : MonoBehaviour {
    Animator _animator;
    [SerializeField] TextMeshProUGUI Txt;
    [SerializeField] Button OkBtn;
    [SerializeField] List<Button> _closeButtonList;

    void Awake() {
        _animator = GetComponent<Animator>();
    }

    public void Open(string text, Action okAction = null, Action closeAction = null) {
        if(Txt != null) {
            Txt.text = text;
        }
        
        if(OkBtn != null) {
            OkBtn.onClick.RemoveAllListeners();
            OkBtn.onClick.AddListener(() => { 
                if(okAction != null) { okAction(); } 
                if(_animator) { _animator.SetTrigger("TurnOff"); }
            });
            OkBtn.gameObject.SetActive(okAction != null);
        }

        RegisterCloseAction(closeAction:closeAction);

        if(_animator != null) {
            ResetTrigger();
            _animator.SetTrigger("TurnOn");
        }
    }

    public void Close() {
        if(_animator) {
            ResetTrigger();
            _animator.SetTrigger("TurnOff");
        }
    }

    void ResetTrigger() {
        if(_animator) {
            _animator.ResetTrigger("TurnOn");
            _animator.ResetTrigger("TurnOff");
            _animator.ResetTrigger("On");
            _animator.ResetTrigger("Off");
        }
    }

    public void RegisterCloseAction(Action closeAction) {
        if(_closeButtonList != null) {
            for(int i = 0; i < _closeButtonList.Count; i++) {
                _closeButtonList[i].onClick.RemoveAllListeners();
                _closeButtonList[i].onClick.AddListener(() => {
                    if(closeAction != null) {
                        closeAction();
                    }
                    Close();
                });
            }
        }
    }
}
