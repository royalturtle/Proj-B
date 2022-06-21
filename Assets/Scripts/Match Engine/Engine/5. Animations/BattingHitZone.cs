using System;
using UnityEngine;

public class BattingHitZone : MonoBehaviour {
    Action _battingAction;
    public bool IsBatting {get; private set;}
    bool _isContacted;
    bool ActionOccured;

    [SerializeField] BattingTimingTypes _timing;

    public void Ready(Action battingAction) {
        _battingAction = battingAction;
    }

    public void SetIsBatting(bool value, bool isContacted = true) {
        ActionOccured = false;
        _isContacted = isContacted;
        IsBatting = value;
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(IsBatting && other.gameObject.tag == GameConstants.TAG_BALL && _battingAction != null && !ActionOccured) {
            ActionOccured = true;
            if(_isContacted) {
                _battingAction();
            }

        }
    }
}
