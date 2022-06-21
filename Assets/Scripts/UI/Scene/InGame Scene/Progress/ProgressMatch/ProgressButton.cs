using UnityEngine;
using UnityEngine.UI;

public class ProgressButton : MonoBehaviour {
    [SerializeField] GameObject _matchButton, _nextDayButton;

    public void SetMatchMode() {
        if (_matchButton != null) { _matchButton.gameObject.SetActive(true); }
        if (_nextDayButton != null) { _nextDayButton.gameObject.SetActive(false); }
    }

    public void SetNextMode() {
        if (_matchButton != null) {_matchButton.gameObject.SetActive(false); }
        if (_nextDayButton != null) {_nextDayButton.gameObject.SetActive(true); }
    }
}
