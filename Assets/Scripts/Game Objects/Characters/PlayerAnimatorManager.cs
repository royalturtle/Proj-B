using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorManager : MoveObject {
    public bool IsRightHand {get; private set;}
    public bool IsDefense {get; private set;}
    CharacterDirection Direction = CharacterDirection.FRONT;

    [SerializeField] Animator BodyAnimator;
    [SerializeField] Animator HatAnimator;

    public void SetData(
        RuntimeAnimatorController bodyAnimator,
        RuntimeAnimatorController hatAnimator,
        bool isRightHand = true
    ) {
        IsRightHand = isRightHand;
        gameObject.SetActive(true);
        if(BodyAnimator!= null) {
            BodyAnimator.runtimeAnimatorController = bodyAnimator;
        }
        if(HatAnimator != null) {
            HatAnimator .runtimeAnimatorController = hatAnimator;
        }
    }

    public void ActiveOff() {
        gameObject.SetActive(false);
    }

    public void SetDirection(CharacterDirection direction) {
        Direction = direction;
        if(BodyAnimator!= null) {
            BodyAnimator.SetInteger(CharacterAnimParams.Direction, (int)Direction);
        }
        if(HatAnimator != null) {
            HatAnimator .SetInteger(CharacterAnimParams.Direction, (int)Direction);
        }
    }

    public void SetIsDefense(bool value) {
        IsDefense = value;
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == GameConstants.TAG_BALL && IsDefense) {
            BallObject ball = other.GetComponent<BallObject>();
            if(ball != null && ball.Height <= 1.0f) {
                // ball.SetIsFinished(true);
            }
        }
    }
}
