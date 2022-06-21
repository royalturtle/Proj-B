using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PageManager : BasePager {
    [SerializeField] Button _previousBtn, _nextBtn;
    [SerializeField] TextMeshProUGUI _currentText, _totalText, _mainText;

    protected override void AwakeAfter() {
        if(_previousBtn) { _previousBtn.onClick.AddListener(PreviousPage); }
        if(_nextBtn) { _nextBtn.onClick.AddListener(NextPage); }
    }

    protected override void ChangeUI() {
        if(_currentText) {
            _currentText.text = (CurrentPage + 1).ToString();
        }

        _previousBtn.interactable = (CurrentPage <= 0) ? false : true;
        _nextBtn.interactable = (CurrentPage >= _totalPage - 1) ? false : true;
    }

    protected override void ChangeTotalUI() {
        if(_totalText) { 
            _totalText.text = _totalPage.ToString(); 
        }
    }

    protected void SetInteractableAfter() {
        _previousBtn.interactable = Interactable;
        _nextBtn.interactable = Interactable;
    }
}
