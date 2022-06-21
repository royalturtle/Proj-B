using System;
using UnityEngine;

public class StadiumWallObject : MonoBehaviour {
    public bool IsOn {get; private set;}
    Action _wallHitAction;

    public void Ready(Action wallHitAction) {
        _wallHitAction = wallHitAction;
    }

    public void Init() {
        IsOn = false;
    }

    public void SetOn() {
        IsOn = true;
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == GameConstants.TAG_BALL && IsOn) {
            if(_wallHitAction != null) {
                _wallHitAction();
            }
        }
    }
}
