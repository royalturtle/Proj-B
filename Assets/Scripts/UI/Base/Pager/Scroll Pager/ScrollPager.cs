using UnityEngine;
using UnityEngine.UI;

public class ScrollPager : BasePager {
    public bool _isVertical;
    [SerializeField] RectTransform _frontImage;

    float BarLength {
        get { return 1.0f / (float)_totalPage; }
    }

    float BarStart {
        get { return (float)CurrentPage * BarLength; }
    }
    
    protected override void ChangeUI() {
        if(_frontImage != null) {
            float start = BarStart;
            if(start < 0.0f) {
                start = 0.0f;
            }

            float end = start + BarLength;
            if(end > 1.0f) {
                end = 1.0f;
            }

            if(_isVertical) {
                _frontImage.anchorMin = new Vector2(0.0f, 1.0f - end);
                _frontImage.anchorMax = new Vector2(1.0f, 1.0f - start);
            }
            else {
                _frontImage.anchorMin = new Vector2(start, 0.0f);
                _frontImage.anchorMax = new Vector2(end, 1.0f);
            }
        }
    }

    protected override void ChangeTotalUI() {
        ChangeUI();
    }
}
