using UnityEngine;

public class DialogBase : MonoBehaviour {
    protected Animator _dialogAnimator; 

    void Awake() {
        _dialogAnimator = GetComponent<Animator>();
        AwakeAfter();
    }

    protected virtual void AwakeAfter() {}

    public void SetActive(bool value) {
        if(_dialogAnimator) {
            ResetAllTrigger();

            if(value) {
                BeforeOn();
            }

            _dialogAnimator.SetTrigger(value ? "On" : "Off");

            if(!value) {
                AfterOff(); 
            }
        }
    }

    public void TurnActive(bool value) {
        if(_dialogAnimator) {
            ResetAllTrigger();

            if(value) {
                BeforeOn(); 
            }

            _dialogAnimator.SetTrigger(value ? "TurnOn" : "TurnOff");

            if(!value) {
                AfterOff(); 
            }
        }
    }

    protected virtual void BeforeOn() {}
    protected virtual void AfterOff() {}

    public void ResetAllTrigger() {
        if(_dialogAnimator) {
            _dialogAnimator.ResetTrigger("On");
            _dialogAnimator.ResetTrigger("Off");
            _dialogAnimator.ResetTrigger("TurnOn");
            _dialogAnimator.ResetTrigger("TurnOff");
        }
    }
}
