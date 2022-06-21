using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchAnimationManager : MonoBehaviour {
    IEnumerator _coroutine;
    [SerializeField] GroundAnimManager groundAnimManager;

    public MatchUIManager _matchUIManager = null;
    public MatchUIManager MatchUIManager {
        get { return _matchUIManager; }
        set { _matchUIManager = value; }
    }

    public void DoAnimation(
        List<MatchSituationBase> situationsList, 
        Action endAction
    ) {
        _coroutine = groundAnimManager.DoAnimation(
            situationsList : situationsList,
            endAction      : endAction
        );
        StartCoroutine(_coroutine);
    }

    public void StopAnimation() {
        if(groundAnimManager != null) {
            groundAnimManager.StopAnimation();
        }
        if(_coroutine != null) {
            StopCoroutine(_coroutine);
        }
        
    }
}
