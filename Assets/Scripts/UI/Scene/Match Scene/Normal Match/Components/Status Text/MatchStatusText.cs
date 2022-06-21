using UnityEngine;
using TMPro;

public class MatchStatusText : MonoBehaviour {
    Animator _animator;
    TextMeshProUGUI _text;

    void Awake() {
        _animator = GetComponent<Animator>();
        _text = GetComponent<TextMeshProUGUI>();
    }

    public void Open(string text) {
        if(_text) { _text.text = text; }
        if(_animator) { 
            _animator.SetTrigger("TurnOn"); 
        }
    }

    public void Close() {
        if(_animator) { _animator.SetTrigger("TurnOff"); }
    }

    public void CloseImmediate() {
        if(_animator) { _animator.SetTrigger("Off"); }
    }

}
