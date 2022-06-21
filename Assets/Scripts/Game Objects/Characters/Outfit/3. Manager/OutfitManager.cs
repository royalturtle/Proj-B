using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutfitManager : MonoBehaviour {
    [SerializeField] List<RuntimeAnimatorController> BodyAnimatorList;
    [SerializeField] List<RuntimeAnimatorController> CapAnimatorList;

    [SerializeField] List<PlayerAnimatorManager> DefenserList;
    [SerializeField] List<PlayerAnimatorManager> RunnerList;
    [SerializeField] BallObject _ball;
    [SerializeField] List<BattingHitZone> _hitZoneList;
    [SerializeField] StadiumWallManager _wallManager;

    Vector3 playerBigScale, playerSmallScale;
    const float playerBigSize = 0.4f, playerSmallSize = 0.25f;

    OutfitSet outfitSet;

    List<MoveObject> _moveList;
    Vector3 _hitBallGoVector, _wallBallGoVector;
    BatterPositionEnum _catcherPosition;

    PlayerAnimatorManager _catcher;
    List<Vector3> _defensePositionList;
    List<Vector3> _runnerPositionList;
    List<Vector3> _basePositionList;

    bool IsBatterRightHand {
        get {
            return (RunnerList != null && RunnerList.Count > 0 && RunnerList[0] != null) ? RunnerList[0].IsRightHand : true;
        }
    }

    bool IsPitcherRightHand {
        get {
            return (DefenserList != null && DefenserList.Count > 0 && DefenserList[0] != null) ? DefenserList[0].IsRightHand : true;
        }
    }

    void Awake() {
        if(_wallManager != null) {
            _wallManager.Ready(WallHit);
            _wallManager.Init();
        }

        _moveList = new List<MoveObject>();
        playerBigScale = new Vector3(playerBigSize, playerBigSize, playerBigSize);
        playerSmallScale = new Vector3(playerSmallSize, playerSmallSize, playerSmallSize);

        _defensePositionList = new List<Vector3> {
            new Vector3(0.0f, -1.2f, 0.0f),
            new Vector3(0.0f, -3.15f, 0.0f),
            new Vector3(1.6f, -0.6f, 0.0f),
            new Vector3(1.0f,  0.5f, 0.0f),
            new Vector3(-1.6f, -0.6f, 0.0f),
            new Vector3(-1.0f, 0.5f, 0.0f),
            new Vector3(-2.8f, 1.5f, 0.0f),
            new Vector3(0.0f, 2.5f, 0.0f),
            new Vector3(2.8f, 1.5f, 0.0f)
        };

        _runnerPositionList = new List<Vector3> {
            new Vector3( 0.21f, -2.65f, 0.0f),
            new Vector3( 1.20f, -0.90f, 0.0f),
            new Vector3(-0.40f,  0.00f, 0.0f),
            new Vector3(-1.30f, -1.50f, 0.0f)
        };

        _basePositionList = new List<Vector3> {
            new Vector3( 0.00f, -2.70f, 0.0f),
            new Vector3( 1.45f, -1.25f, 0.0f),
            new Vector3( 0.00f,  0.20f, 0.0f),
            new Vector3(-1.45f, -1.25f, 0.0f),
            new Vector3( 0.00f, -2.70f, 0.0f)
        };

        if(_hitZoneList != null) {
            for(int i = 0; i < _hitZoneList.Count; i++) {
                if(_hitZoneList[i]) {
                    _hitZoneList[i].Ready(battingAction:Batting);
                }
            }
        }
    }

    void Start() {
        outfitSet = new OutfitSet();

        for(int i = 0; i < OutfitSet.DefenseCount; i++) {
            SetPlayerOutfit(isDefense:true , index:i);
        }

        for(int i = 0; i < OutfitSet.RunnerCount; i++) {
            SetPlayerOutfit(isDefense:false, index:i);
        }
    }

    void FixedUpdate() {
        for(int i = _moveList.Count - 1; i >= 0; i--) {
            if(_moveList[i] == null || _moveList[i].IsFinished) {
                _moveList.RemoveAt(i);
            }
            else {
                _moveList[i].Walk();
            }
        }
    }

    public void SetStatus(bool value) {
        Vector3 scaleVector = value ? playerSmallScale : playerBigScale;

        if(_ball != null) {
            _ball.Init();
            if(!value){
                _ball.SetActive(false);
            }
        }

        if(DefenserList != null) {
            for(int i = 0; i < DefenserList.Count; i++) {
                if(DefenserList[i] != null) {
                    DefenserList[i].transform.localScale = scaleVector;
                }
            }
        }

        if(RunnerList != null) {
            for(int i = 0; i < RunnerList.Count; i++) {
                if(RunnerList[i] != null) {
                    RunnerList[i].transform.localScale = scaleVector;
                }
            }
        }

        if(_moveList != null) {
            _moveList.Clear();
        }

        if(_hitZoneList != null) {
            for(int i = 0; i < _hitZoneList.Count; i++) {
                if(_hitZoneList[i]) {
                    _hitZoneList[i].SetIsBatting(false);
                }
            }
        }

        if(_wallManager != null && !value) {
            _wallManager.Init();
        }

        if(!value) {
            InitPlayers();
        }
    }

    void InitPlayers() {
        _catcher = null;

        for(int i = 0; i < OutfitSet.DefenseCount; i++) {
            CharacterDirection direction = GetFirstDirection(isDefense:true, index:i);
            PlayerAnimatorManager player = GetPlayer(isDefense:true, index:i);
            if(player != null) {
                player.SetIsDefense(false);
                player.transform.localPosition = _defensePositionList[i];
                player.SetDirection(direction:direction);
            }
        }

        for(int i = 0; i < OutfitSet.RunnerCount; i++) {
            CharacterDirection direction = GetFirstDirection(isDefense:false, index:i);
            PlayerAnimatorManager player = GetPlayer(isDefense:false, index:i);
            if(player != null) {
                player.transform.localPosition = _runnerPositionList[i];
                player.SetDirection(direction:direction);
            }
        }
    }

    public void SetData(MatchStatus status) {
        if(status != null) {
            // 투수 

            // 수비 리스트

            // 타자/주자
            outfitSet.SetBases(baseStatus:status.BaseStatus);
        }
    }

    public void UpdateBase(MatchStatus status) {
        if(status != null) {
            // 타자/주자
            outfitSet.SetBases(baseStatus:status.BaseStatus);

            for(int i = 0; i < OutfitSet.RunnerCount; i++) {
                SetPlayerOutfit(isDefense:false, index:i);
            }
        }   
    }


    CharacterDirection GetFirstDirection(bool isDefense, int index) {
        CharacterDirection result = CharacterDirection.FRONT;
        if(isDefense) { if(index == 1) result = CharacterDirection.BACK; }
        else if(index == 0) result = CharacterDirection.BACK;
        else if(index == 1) result = CharacterDirection.LEFT;
        else if(index == 3) result = CharacterDirection.RIGHT;
        
        return result;
    }

    void SetPlayerOutfit(bool isDefense, int index) {
        PlayerAnimatorManager player = GetPlayer(isDefense:isDefense, index:index);

        if(player != null) {
            OutfitData outfitData = GetOutfit(isDefense, index);

            if(outfitData != null) {
                CharacterDirection direction = GetFirstDirection(isDefense:isDefense, index:index);

                int bodyIndex = outfitData.BodyIndex;
                int hatIndex  = outfitData.HatIndex;

                player.SetData(
                    bodyAnimator : BodyAnimatorList[bodyIndex],
                    hatAnimator  : CapAnimatorList[hatIndex]
                );
                player.SetDirection(direction:direction);
            }
            else {
                player.ActiveOff();
            }
        }
    }

    PlayerAnimatorManager GetPlayer(bool isDefense, int index) {
        return isDefense ? DefenserList[index] : RunnerList[index];
    }

    OutfitData GetOutfit(bool isDefense, int index) {
        return isDefense ? outfitSet.DefenseList[index] : outfitSet.RunnerList[index];
    }

    Vector3 GetPitchingStartVector {
        get {
            return (_ball != null) ? 
                _ball.GetPitchingPosition(isPitcherRightHand:IsPitcherRightHand) :
                new Vector3(0.0f, 0.0f, 0.0f);
        }
    }

    public IEnumerator WildPitchingAnimation() {
        if(_ball) {
            Vector3 startVector = GetPitchingStartVector;
            Vector3 endVector = _ball.GetEndPosition(BaseballResultTypes.WP, isBatterRightHand:IsBatterRightHand);
            _ball.SetActive(true);
            _moveList.Add(_ball.SetBallMove(
                startVector : startVector,
                endVector   : endVector,
                speed       : 10.0f
            ));
        }
        Debug.Log("Animation : Wild Pitching");
        yield return null;
    }

    public IEnumerator HitByPitchAnimation() {
        if(_ball) {
            Vector3 startVector = GetPitchingStartVector;
            Vector3 endVector = _ball.GetEndPosition(BaseballResultTypes.HBP, isBatterRightHand:IsBatterRightHand);
            _ball.SetActive(true);
            _moveList.Add(_ball.SetBallMove(
                startVector : startVector,
                endVector   : endVector,
                speed       : 10.0f
            ));
        }
        Debug.Log("Animation : HBP");
        yield return null;
    }

    public IEnumerator StrikeOutAnimation(bool isSwing) {
        if(_ball && _hitZoneList != null) {
            Vector3 startVector = GetPitchingStartVector;
            Vector3 endVector = _ball.GetEndPosition(
                isSwing ? BaseballResultTypes.STRIKE_OUT_SWING : BaseballResultTypes.STRIKE_OUT, 
                isBatterRightHand:IsBatterRightHand
            );

            if(isSwing) {
                BattingTimingTypes battingTiming = UnityEngine.Random.Range(0, 2) == 0 ? BattingTimingTypes.Early : BattingTimingTypes.Late;
                _hitZoneList[(int)battingTiming].SetIsBatting(true,  isContacted:false);
            }

            _ball.SetActive(true);
            _moveList.Add(_ball.SetBallMove(
                startVector : startVector,
                endVector   : endVector,
                speed       : 10.0f
            ));
        }
        Debug.Log("Animation : StrikeOut");
        yield return null;
    }

    public IEnumerator BaseOnBallsAnimation() {
        if(_ball) {
            Vector3 startVector = GetPitchingStartVector;
            Vector3 endVector = _ball.GetEndPosition(BaseballResultTypes.BB, isBatterRightHand:IsBatterRightHand);
            _ball.SetActive(true);
            _moveList.Add(_ball.SetBallMove(
                startVector : startVector,
                endVector   : endVector,
                speed       : 10.0f
            ));
        }
        Debug.Log("Animation : BaseOnBalls");
        yield return null;
    }

    public IEnumerator HitBallAnimation(DefenseCatchSituation catchSituation) {
        if(_ball && _hitZoneList != null && catchSituation != null) {
            Vector3 startVector = GetPitchingStartVector;
            Vector3 endVector = _ball.GetEndPosition(isBatterRightHand:IsBatterRightHand);

            _hitZoneList[(int)BattingTimingTypes.Normal].SetIsBatting(true);

            _hitBallGoVector = _ball.GetHitPosition(catchSituation:catchSituation);
            switch(catchSituation.NewResult) {
                case BaseballResultTypes.HIT2:
                case BaseballResultTypes.HIT2_LONG:
                case BaseballResultTypes.HIT3:
                    if(_wallManager != null) {
                        _wallBallGoVector = _wallManager.SetData(
                            afterVector : _hitBallGoVector,
                            position    : catchSituation.Position
                        );
                    }
                    else {
                        _wallBallGoVector = _hitBallGoVector;
                    }
                    break;
                default:
                    _wallBallGoVector = _hitBallGoVector;
                    break;
            }


            _catcher = DefenserList[(int)catchSituation.Position];
            if(_catcher != null) {
                _catcher.SetIsDefense(true);
            }

            _ball.SetActive(true);
            _moveList.Add(_ball.SetBallMove(
                startVector  : startVector,
                endVector    : endVector,
                speed        : 10.0f,
                isPitching   : true
            ));
        }
        Debug.Log("Animation : Hit");
        yield return null;
    }

    public IEnumerator CatchAnimation(DefenseCatchSituation catchSituation) {
        Debug.Log("Animation : Catch");
        yield return null;
    }

    public IEnumerator HomerunAnimation(DefenseCatchSituation catchSituation) {
         if(_ball && _hitZoneList != null && catchSituation != null) {
            Vector3 startVector = GetPitchingStartVector;
            Vector3 endVector = _ball.GetEndPosition(isBatterRightHand:IsBatterRightHand);

            _hitZoneList[(int)BattingTimingTypes.Normal].SetIsBatting(true);

            _hitBallGoVector = _ball.GetHitPosition(catchSituation:catchSituation);
            _wallBallGoVector = _hitBallGoVector;

            _ball.SetActive(true);
            _moveList.Add(_ball.SetBallMove(
                startVector  : startVector,
                endVector    : endVector,
                speed        : 10.0f,
                isPitching   : true
            ));
        }
        Debug.Log("Animation : Homerun");
        yield return null;
    }

    public IEnumerator TagUpAnimation(TagUpSituation tagUpSituation, bool isRelay) {
        if(_ball && tagUpSituation != null) {
            Vector3 startVector = _ball.transform.localPosition;

            Vector3 endVector = Vector3.zero;

            if(BatterPositionEnum.C <= tagUpSituation.Defense2 && tagUpSituation.Defense2 <= BatterPositionEnum.RF) {
                endVector = isRelay ? 
                    _defensePositionList[(int)tagUpSituation.Defense2] :
                    _basePositionList[tagUpSituation.ThrowBase];
            }

            _ball.SetThrowHeight();
            _ball.SetActive(true);
            _moveList.Add(_ball.SetBallMove(
                startVector  : startVector,
                endVector    : endVector,
                speed        : 10.0f
            ));
        }
        yield return null;
    }

    public IEnumerator ThrowGroundAnimation(ThrowGroundSituation throwGroundSituation, int index) {
        if(_ball && throwGroundSituation != null && throwGroundSituation.BaseList != null && throwGroundSituation.BaseList.Count > index) {
            Vector3 startVector = _ball.transform.localPosition;
            Vector3 endVector = _basePositionList[throwGroundSituation.BaseList[index]];

            _ball.SetThrowHeight();
            _ball.SetActive(true);
            _moveList.Add(_ball.SetBallMove(
                startVector  : startVector,
                endVector    : endVector,
                speed        : 10.0f
            ));
        }
        yield return null;
    }

    public IEnumerator ThrowHitAnimation(ThrowHitSituation throwHitSituation, bool isRelay) {
        if(_ball && throwHitSituation != null) {
            Vector3 startVector = _ball.transform.localPosition;
            Vector3 endVector = isRelay ? 
                _defensePositionList[(int)throwHitSituation.Defense2] :
                _basePositionList[throwHitSituation.ThrowBase];

            _ball.SetThrowHeight();
            _ball.SetActive(true);
            _moveList.Add(_ball.SetBallMove(
                startVector  : startVector,
                endVector    : endVector,
                speed        : 10.0f
            ));
        }
        yield return null;
    }

    void Batting() {
        if(_moveList != null && _ball != null) {
            Vector3 currentBallPosition = _ball.transform.localPosition;
            _moveList.Clear();
            
            MoveObject ballMove = _ball.SetBallMove(
                startVector : currentBallPosition,
                // endVector   : _hitBallGoVector,
                endVector   : _wallBallGoVector,
                speed       : 10.0f,
                lastSpeed   : 5.0f,
                isPitching  : false
            );

            MoveObject defenseMove = null;
            if(ballMove != null && _catcher != null) {
                defenseMove = _catcher.SetWalkByTime(
                    startVector : _catcher.transform.localPosition,
                    endVector   : _hitBallGoVector,
                    speed       : 1.0f,
                    timeSpend   : ballMove.TimeNeeded
                );
            }

            _moveList.Add(ballMove);
            _moveList.Add(defenseMove);
        }
    }

    void WallHit() {
        //_wallBallGoVector = _hitBallGoVector;
        _moveList.RemoveAt(0);
            MoveObject ballMove = _ball.SetBallMove(
                startVector : _ball.transform.localPosition,
                endVector   : _hitBallGoVector,
                speed       : 5.0f,
                lastSpeed   : 4.0f,
                isPitching  : false
            );
            _moveList.Add(ballMove);
    }
}
