using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundAnimManager : MonoBehaviour {
    [SerializeField] CameraScript mainCamera;
    [SerializeField] MatchStatusText _statusText;
    [SerializeField] OutfitManager _outfitManager;
    Action _endAction;
    
    IEnumerator _coroutine, _moveCoroutine;

    GroundCameraSet CameraSet;
    void Start() {
        CameraSet = new GroundCameraSet();
    }

    public IEnumerator DoAnimation(
        List<MatchSituationBase> situationsList, 
        Action endAction
    ) {
        if(_outfitManager) {
            _outfitManager.SetStatus(value:true);
        }

        _endAction = endAction;
        if(_outfitManager != null && situationsList != null && situationsList.Count > 0) {
            MatchSituationBase firstSituation = situationsList[0];
            if(firstSituation is WildPitchingSituation) {
                mainCamera.SetPosition(CameraSet.Pitching);
                OpenText("폭투");
                StartMoveCoroutine(_outfitManager.WildPitchingAnimation());
                yield return new WaitForSeconds(2.0f);
            }
            else if(firstSituation is HBPSituation) {
                mainCamera.SetPosition(CameraSet.Pitching);
                OpenText("사구");
                StartMoveCoroutine(_outfitManager.HitByPitchAnimation());

                yield return new WaitForSeconds(2.0f);
            }
            else if(firstSituation is KBBSituation) {
                KBBSituation kbb = (KBBSituation)firstSituation;
                mainCamera.SetPosition(CameraSet.Pitching);
                switch(kbb.Result) {
                    case BaseballResultTypes.STRIKE_OUT:
                        OpenText("삼진");
                        StartMoveCoroutine(_outfitManager.StrikeOutAnimation(isSwing:false));
                        break;
                    case BaseballResultTypes.STRIKE_OUT_SWING:
                        OpenText("삼진");
                        StartMoveCoroutine(_outfitManager.StrikeOutAnimation(isSwing:true));
                        break;
                    case BaseballResultTypes.BB:
                        OpenText("볼넷");
                        StartMoveCoroutine(_outfitManager.BaseOnBallsAnimation());
                        break;
                }
                yield return new WaitForSeconds(2.0f);
            }
            else {
                DefenseCatchSituation defenseCatch = (DefenseCatchSituation)situationsList[1];
                
                mainCamera.SetPosition(CameraSet.Pitching);
                switch(defenseCatch.NewResult) {
                    case BaseballResultTypes.HOMERUN:
                        StartMoveCoroutine(_outfitManager.HomerunAnimation(catchSituation:defenseCatch));
                        break;
                    default:
                        StartMoveCoroutine(_outfitManager.HitBallAnimation(catchSituation:defenseCatch));
                        break;
                }
                yield return new WaitForSeconds(0.5f);
            
                mainCamera.SetPosition(CameraSet.GetPosition(defenseCatch.Position));
                switch(defenseCatch.NewResult) {
                    case BaseballResultTypes.HIT1:
                        OpenText("1루타");
                        break;
                    case BaseballResultTypes.HIT1_LONG:
                        OpenText("1루타");
                        break;
                    case BaseballResultTypes.HIT2:
                        OpenText("2루타");
                        
                        yield return new WaitForSeconds(1.5f);
                        break;
                    case BaseballResultTypes.HIT2_LONG:
                        OpenText("2루타");

                        yield return new WaitForSeconds(1.5f);
                        break;
                    case BaseballResultTypes.HIT3:
                        OpenText("3루타");

                        yield return new WaitForSeconds(1.5f);
                        break;
                    case BaseballResultTypes.HOMERUN:
                        OpenText("홈런");
                        break;
                    case BaseballResultTypes.FLY_INNER:
                        OpenText("아웃");
                        break;
                    case BaseballResultTypes.FLY_OUTSIDE:
                        OpenText("아웃");
                        break;
                    case BaseballResultTypes.INFIELD_FLY:
                        OpenText("인필드 플라이");
                        break;
                }
                // StartMoveCoroutine(_outfitManager.CatchAnimation(catchSituation:defenseCatch));

                yield return new WaitForSeconds(0.5f);

                if(situationsList.Count >= 3) {
                    MatchSituationBase thirdSituation = situationsList[2];
                    if(thirdSituation is TagUpSituation) {
                        TagUpSituation tagUp = (TagUpSituation)thirdSituation;

                        if(tagUp.Defense2 != BatterPositionEnum.NONE) {
                            mainCamera.SetPosition(CameraSet.GetPosition(tagUp.Defense2));
                            StartMoveCoroutine(_outfitManager.TagUpAnimation(tagUpSituation:tagUp, isRelay:true));
                            yield return new WaitForSeconds(0.5f);
                        }
                        
                        mainCamera.SetPosition(CameraSet.GetBase(tagUp.ThrowBase));
                        StartMoveCoroutine(_outfitManager.TagUpAnimation(tagUpSituation:tagUp, isRelay:false));
                        yield return new WaitForSeconds(0.5f);
                        if(tagUp.IsOut) {
                            OpenText("아웃");
                        }
                        else {
                            OpenText("세이프");
                        }
                        yield return new WaitForSeconds(0.5f);
                    }
                    else if(thirdSituation is ThrowGroundSituation) {
                        ThrowGroundSituation throwGround = (ThrowGroundSituation)thirdSituation;

                        if(throwGround.BaseList.Count <= 1) {
                            StartMoveCoroutine(_outfitManager.ThrowGroundAnimation(throwGroundSituation:throwGround, index:0));
                            OpenText("아웃");
                            yield return new WaitForSeconds(0.5f);
                        }
                        else {
                            for(int i = 0; i < throwGround.IsCatchList.Count; i++) {
                                StartMoveCoroutine(_outfitManager.ThrowGroundAnimation(throwGroundSituation:throwGround, index:i));
                                // mainCamera.SetPosition(CameraSet.GetBase(throwGround.BaseList[i + 1]));
                                mainCamera.SetPosition(CameraSet.GetBase(throwGround.BaseList[i]));
                                OpenText((throwGround.IsCatchList[i]) ? "아웃" : "세이프");
                                yield return new WaitForSeconds(0.5f);
                            }
                        }
                        
                    }
                    else if(thirdSituation is ThrowHitSituation) {
                        ThrowHitSituation throwHit = (ThrowHitSituation)thirdSituation;

                        if(throwHit.Defense2 != BatterPositionEnum.NONE) {
                            StartMoveCoroutine(_outfitManager.ThrowHitAnimation(throwHitSituation:throwHit, isRelay:true));
                            mainCamera.SetPosition(CameraSet.GetPosition(throwHit.Defense2));
                            yield return new WaitForSeconds(0.5f);
                        }
                        
                        StartMoveCoroutine(_outfitManager.ThrowHitAnimation(throwHitSituation:throwHit, isRelay:false));
                        mainCamera.SetPosition(CameraSet.GetBase(throwHit.ThrowBase));
                        if(throwHit.IsOut) {
                            OpenText("아웃");
                        }
                        else {
                            OpenText("세이프");
                        }
                        yield return new WaitForSeconds(0.5f);
                    }
                }
                // ResultType = defenseCatch.NewResult;
            }
        }
        
        Reset();
        if(_endAction != null) {
            _endAction();
        }
        yield return null;
    }

    void StartMoveCoroutine(IEnumerator coroutine) {
        _moveCoroutine = coroutine;
        StartCoroutine(_moveCoroutine);
    }

    void OpenText(string text) {
        _coroutine = IOpenText(text:text);
        StartCoroutine(_coroutine);
    }

    IEnumerator IOpenText(string text) {
        if(_statusText) {
            _statusText.Open(text);
        }
        yield return new WaitForSeconds(1.0f);
    }

    public void StopAnimation() {
        // 텍스트 제우기
        if(_coroutine != null) {
            StopCoroutine(_coroutine);
        }

        if(_moveCoroutine != null) {
            StopCoroutine(_moveCoroutine);
        }
        
        if(_statusText) {
            _statusText.CloseImmediate();
        }
        
        Reset();
        if(_endAction != null) {
            _endAction();
        }
    }

    void Reset() {
        if(_outfitManager) {
            _outfitManager.SetStatus(value:false);
        }
        mainCamera.SetPosition(CameraSet.Default);
    }
}
