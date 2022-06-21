using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OtherMatchListView : MonoBehaviour {
    List<MatchInfo> _matchList;
    int PageCount {
        get { return _matchList == null ? 0 : ((_matchList.Count - 1) / ItemCount) + 1; }
    }

    [SerializeField] List<OtherMatchView> _viewList;
    int ItemCount {
        get { return _viewList == null ? 0 : _viewList.Count; }
    }

    int _page = 0;
    [SerializeField] Button _prevButton, _nextButton;

    void Awake() {
        if(_prevButton) {
            _prevButton.onClick.AddListener(() =>{
                PrevPage();
            });
        }

        if(_nextButton) {
            _nextButton.onClick.AddListener(() =>{
                NextPage();
            });
        }
    }

    public void SetEmpty() {
        SetData(null);
    }

    public void SetData(List<MatchInfo> matchList) {
        _matchList = matchList;
        if(matchList != null) {
            bool isChangeAble = PageCount > 1;
            if(_prevButton) {
                _prevButton.interactable = isChangeAble;
            }
            if(_nextButton) {
                _nextButton.interactable = isChangeAble;
            }
            
        }
        else {
            if(_prevButton) {
                _prevButton.interactable = false;
            }
            if(_nextButton) {
                _nextButton.interactable = false;
            }
        }
        SetPage(0);
    }

    void PrevPage() {
        SetPage(--_page < 0 ? PageCount - 1 : _page);
    }

    void NextPage() {
        SetPage(++_page >= PageCount ? 0 : _page);
    }

    void SetPage(int page) {
        _page = page;
        int i = 0;
        if(_matchList != null) {
            int itemCount = _matchList.Count - _page * ItemCount;
            if(itemCount > ItemCount) {
                itemCount = ItemCount;
            }
            
            List<MatchInfo> tmpList = _matchList.GetRange(_page * ItemCount, itemCount);
            for(; i < tmpList.Count; i++) {
                _viewList[i].SetData(tmpList[i]);
            }
        }

        for(; i < _viewList.Count; i++) {
            _viewList[i].SetData(null);
        }
    }



}
