using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallShadowObject : MonoBehaviour {
    [SerializeField] List<Sprite> spriteList;

    List<float>  thresholdList;
    SpriteRenderer m_SpriteRenderer;

    void Start() {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Ready(float heightMax) {
        thresholdList = new List<float>();

        if(SpritesCount > 0) {
            float heightForThreshold = heightMax / SpritesCount;
            for(int i = 0; i < SpritesCount - 1; i++) {
                thresholdList.Add(heightForThreshold * (i + 1));
            }
        }
    }

    public void SetShadow(float height) {
        gameObject.SetActive(true);
        if(thresholdList != null) {
            int index = SpritesCount - 1;

            for(int i = 0; i < thresholdList.Count; i++) {
                if(height <= thresholdList[i]) {
                    index = i;
                    break;
                }
            }

            Sprite sprite = spriteList[index];
            if(m_SpriteRenderer != null) {
                m_SpriteRenderer.sprite = sprite;
            }
        }
    }

    public void SetActive(bool value) {
        gameObject.SetActive(value);
    }

    public int SpritesCount { get { return spriteList == null ? 0 : spriteList.Count;}}
}
