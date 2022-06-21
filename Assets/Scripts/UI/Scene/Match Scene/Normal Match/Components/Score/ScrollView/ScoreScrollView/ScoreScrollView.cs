using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreScrollView : MonoBehaviour {
    [SerializeField] GameObject _itemPrefab;
    [SerializeField] GameObject _contentLayout;
    [SerializeField] RectTransform _contentRect;

    private int itemCount = 0;
    private float itemHeight, itemWidth;
    int currentInning = 9;

    void Start() {
        Canvas.ForceUpdateCanvases();
        itemWidth = this.transform.GetComponent<RectTransform>().rect.width / 9;
        itemHeight = this.transform.GetComponent<RectTransform>().rect.height;

        SetItemSize();
    }

    void SetItemSize() {
        for (int i = 0; i < currentInning; i++) {
            AddItem(i + 1);
        }
    }

    private void AddItem(int inning) {
        GameObject newObj = Instantiate(_itemPrefab);
        newObj.transform.SetParent(_contentLayout.transform);
        newObj.GetComponent<RectTransform>().sizeDelta = new Vector2(itemWidth, itemHeight);
        newObj.transform.localScale = new Vector3(1, 1, 1);

        ScoreScrollItemScript item = newObj.GetComponent<ScoreScrollItemScript>();
        item.SetInning(inning);
        itemCount++;
    }

    private ScoreScrollItemScript GetInningObject(int index) {
        Transform contents = _contentLayout.transform;
        int contentsCount = contents.childCount;

        if (contentsCount < 1) return null;

        // Check the unable small index value
        int _index = (index < 0) ? 0 : index;

        // Check the unable big index value
        _index = (_index >= contents.childCount) ? contents.childCount - 1 : _index;

        return contents.GetChild(_index).gameObject.GetComponent<ScoreScrollItemScript>();
    }

    public void SetScore(int inning, int score, MatchPosition matchPosition) {
        if(inning > currentInning) {
            AddItem(inning);
            currentInning = inning;
            if(_contentRect != null) {
                Vector3 former = _contentRect.localPosition;
                _contentRect.localPosition = new Vector3(
                    -1000.0f,former.y,former.z
                );
            }
        }
        
        if (matchPosition == MatchPosition.HOME) GetInningObject(inning - 1).SetScoreHome(score);
        else GetInningObject(inning - 1).SetScoreAway(score);
    }
}
