using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerReferenceView<T> : MonoBehaviour where T : PlayerBase {
    protected List<T> _viewList;
    [SerializeField] protected BasePager _pagePager, _categoryPager;

    public int FindPlayer(int id) {
        int result = GameConstants.NULL_INT;
        for(int i = 0; i < _viewList.Count; i++) {
            if(_viewList[i].Base.ID == id) {
                result = i;
                break;
            }
        }
        return result;
    }

    public (int, PlayerBase) GetNextPlayer(int index, bool isNext) {
        PlayerBase result = null;
        int newIndex = GameConstants.NULL_INT;
        if(_viewList != null && _viewList.Count > 0) {
            newIndex = index + ((isNext) ? 1 : -1);
            newIndex = Utils.GetLoopableIndex(_viewList, newIndex);

            if(newIndex >= 0 && newIndex < _viewList.Count) {
                result = _viewList[newIndex];
            }
        }

        return (newIndex, result);
    }

    public void CheckScroll(BaseEventData eventData) {
        if(eventData != null && eventData is PointerEventData) {
            Vector2 vec = ((PointerEventData)eventData).delta;
            bool isVertical = (Mathf.Abs(vec.y) >= Mathf.Abs(vec.x));
            BasePager pageManager = isVertical ? _pagePager : _categoryPager;

            if(pageManager != null) {
                float value = isVertical ? vec.y : vec.x;
                if(value < 0.0f) {
                    pageManager.PreviousPage();
                }
                else if(value > 0.0f) {
                    pageManager.NextPage();
                }
            }
        }
    }
}
