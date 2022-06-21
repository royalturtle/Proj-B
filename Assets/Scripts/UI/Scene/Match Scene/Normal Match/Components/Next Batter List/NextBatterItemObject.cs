using UnityEngine;
using TMPro;

public class NextBatterItemObject : MonoBehaviour {
    [SerializeField] TextMeshProUGUI _rankText;
    [SerializeField] LocalizeScript _handText;
    [SerializeField] Animator _changeAnimator;

    public void SetData(string rank, Hands hand) {
        if(_rankText != null) { _rankText.text = rank; }
        if(_handText != null) { _handText.SetData(value:(int)hand); }
    }

    public void ActivateSelected() {
        _changeAnimator.SetBool("isSelected", true);
    }

    public void DisactivateSelected() {
        _changeAnimator.SetBool("isSelected", false);
    }
}
