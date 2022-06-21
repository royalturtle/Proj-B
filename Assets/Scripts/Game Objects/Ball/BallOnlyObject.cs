using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallOnlyObject : MonoBehaviour {
    const string ANIM_ISMOVING = "IsMoving";

    const float ScaleMin = 1.0f, ScaleMax = 1.8f, ScaleRange = ScaleMax - ScaleMin;

    Animator animator;

    void Start() {
        animator = GetComponent<Animator>();
    }
    
    public void Move(float height, float maxHeight) {
        gameObject.SetActive(true);
        float scale = GetScale(height, maxHeight);

        transform.localPosition = new Vector3(0.0f, height, 0.0f);
        transform.localScale = new Vector3(scale, scale, 1.0f);

        if(animator != null) {
            animator.SetBool(ANIM_ISMOVING, true);
        }
    }

    float GetScale(float height, float maxHeight) {
        return ScaleMin + (height / maxHeight) * ScaleRange;
    }

    public void SetActive(bool value) {
        gameObject.SetActive(value);
    }
}
