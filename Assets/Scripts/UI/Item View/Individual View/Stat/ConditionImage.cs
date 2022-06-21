using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConditionImage : MonoBehaviour {
    [SerializeField] List<Sprite> _imageList;
    Image _image;

    void Awake() {
        _image = GetComponent<Image>();
    }

    public void SetData(double condition) {
        Sprite sprite = null;

        if(_imageList != null && _imageList.Count > 0) {
            int index = 0;
            for(; index < _imageList.Count - 1; index++) {
                if(condition <= (index + 1) * 0.2) {
                    break;
                }
            }
            
            sprite = _imageList[index];
        }
        
        if(_image) {
            _image.sprite = sprite;
        }
    }
}
