using System;
using UnityEngine;
using UnityEngine.UI;

public class RecordItemBase : MonoBehaviour {
    protected PlayerBase _player;
    protected bool _isBatter;
    protected Button _button;
    protected CanvasGroup _canvasGroup;

    [SerializeField] protected Image _teamLogoImage;
    protected Func<int, Sprite> _getLogoAction;

    void Awake() {
        _button = GetComponent<Button>();
        _canvasGroup = GetComponent<CanvasGroup>();
        AwakeAfter();
    }
    protected virtual void AwakeAfter() {}

    public void Ready(Action<PlayerBase> clickAction, Func<int, Sprite> getLogoAction) {
        _getLogoAction = getLogoAction;
        _button = GetComponent<Button>();
        if(_button) {
            _button.onClick.AddListener(() => {
                if(clickAction != null) { clickAction(_player); }
            });
        }
    }
    public virtual void SetData(PlayerBase player, int rank = 1) {}
    protected void OpenSingleDialog() {}

    public void SetActive(bool value) {
        if(_canvasGroup != null) {
            _canvasGroup.alpha = value ? 1 : 0;
        }
    }
}
