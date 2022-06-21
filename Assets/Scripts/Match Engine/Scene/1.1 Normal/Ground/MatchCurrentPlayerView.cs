using UnityEngine;

public class MatchCurrentPlayerView : MonoBehaviour {
    Animator _animator;

    void Awake() {
        _animator = GetComponent<Animator>();
    }

    public void SetTurn(bool value) {
        if(_animator) {
            Debug.Log("TESTANIMATOR" + value.ToString());
            _animator.SetTrigger("Turn" + (value ? "On" : "Off"));
        }
    }
}
