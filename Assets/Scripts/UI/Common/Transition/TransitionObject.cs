using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;
using TMPro;

public class TransitionObject : MonoBehaviour {
    Animator _animator;
    bool IsFinished, _isThreadFinished;
    TransitionModes _mode;
    Action _workingAction, _threadAction;
    Action _loadingFinishAction;

    [SerializeField] Image _slideObject, _gradientObject;
    Image _image;

    [SerializeField] TextMeshProUGUI _tipText, _dateText;
    [SerializeField] PercentageText _percentageText;    

    static double Percentage;
    public static bool IsPencetageChanging;
    public static void SetPercentage(double value) {
        Percentage = value;
    }

    public static string DateString;
    public static bool IsDateChanging;
    
    static string TipString;
    public static bool IsTipChanging;

    void Awake() {
        _animator = GetComponent<Animator>();
        _image = GetComponent<Image>(); 
    }

    void Update() {
        if(IsPencetageChanging && _percentageText != null) {
            _percentageText.SetValue(Percentage);
        }

        if(IsDateChanging && _dateText != null) {
            _dateText.text = DateString;
        }
    }

    public void SetIsFinished(bool value) {
        IsFinished = value;        
    }

    void SetLoading(bool value) {
        if(_animator) {
            _animator.SetTrigger("LoadingTurn" + (value ? "On" : "Off"));
        }
    }

    public void SetSlide(
        bool value, 
        TransitionModes mode, 
        bool isPercentage = false,
        Action workingAction = null, 
        Action threadAction = null,
        bool isDate = false,
        byte alpha = 255
    ) {
        if(value && _image != null) {
            _image.raycastTarget = true;
        }

        if(_slideObject != null) {
            _slideObject.color = new Color32(0, 0, 0, alpha);
        }

        _mode = mode;

        if(value) {
            _threadAction = threadAction;
            _workingAction = workingAction;
            _loadingFinishAction = () => {
                SetSlide(false, mode:TransitionModes.NONE); 
            };
            SetPercentage(0.0);
            IsPencetageChanging = isPercentage;
            IsDateChanging = isDate;
        }
        else {
            IsPencetageChanging = false;
            IsDateChanging = false;

            if(_dateText) {
                _dateText.text = "";
            }
        }

        if(_percentageText != null) {
            _percentageText.gameObject.SetActive(IsPencetageChanging);
        }

        if(_animator) {
            _animator.SetTrigger("SlideTurn" + (value ? "On" : "Off"));
        }
    }

    public void SetGradient(
        bool value, 
        TransitionModes mode, 
        bool isPercentage = false,
        Action workingAction = null, 
        Action threadAction = null,
        bool isDate = false,
        byte alpha = 255
    ) {
        if(value && _image != null) {
            _image.raycastTarget = true;
        }
        
        if(_gradientObject != null) {
            _gradientObject.color = new Color32(0, 0, 0, alpha);
        }

        _mode = mode;

        if(value) {
            _threadAction = threadAction;
            _workingAction = workingAction;
            _loadingFinishAction = () => {
                SetGradient(false, mode:TransitionModes.NONE); 
            };
            SetPercentage(0.0);
            IsPencetageChanging = isPercentage;
            IsDateChanging = isDate;
        }
        else {
            IsPencetageChanging = false;
            IsDateChanging = false;

            if(_dateText) {
                _dateText.text = "";
            }
        }

        if(_percentageText != null) {
            _percentageText.gameObject.SetActive(IsPencetageChanging);
        }
        
        if(_animator) {
            _animator.SetTrigger("GradientTurn" + (value ? "On" : "Off"));
        }
    }

    public void ReleaseNonClick() {
        if(_image != null) {
            _image.raycastTarget = false;
        }
    }

    public void TurnOnFinished() {
        switch(_mode) {
            case TransitionModes.NORMAL:
                StartCoroutine(Working());
                break;
            case TransitionModes.COROUTINE:
                SetLoading(value:true);
                StartCoroutine(Working());
                break;
            case TransitionModes.THREAD:
                SetLoading(value:true);
                ThreadWorking();
                break;
        }
    }

    public void CheckExit() {
        if(IsFinished) {
            IsFinished = false;

            if(_mode == TransitionModes.THREAD) {
                _mode = TransitionModes.COROUTINE;
                StartCoroutine(Working());
            }
            else {
                SetLoading(value:false);
            }
        }
    }

    public void LoadingFinished() {
        if(_loadingFinishAction != null) {
            _loadingFinishAction();
        }
    }

    IEnumerator Working() {
        if(_workingAction != null) {
            _workingAction();
        }

        switch(_mode) {
            case TransitionModes.NORMAL:
                WaitForRealSeconds(0.5f);
                LoadingFinished();
                break;
            case TransitionModes.COROUTINE:
                IsFinished = true;
                break;
        }

        yield return null;
    }

    void ThreadWorking() {
        Thread thread = new Thread(delegate() {
            _threadAction();
            SetIsFinished(true);
        });
        thread.Start();
    }

    IEnumerator WaitForRealSeconds (float seconds) {
        float startTime = Time.realtimeSinceStartup;
        
        while (Time.realtimeSinceStartup-startTime < seconds) {
            yield return null;
        }
    }
}
