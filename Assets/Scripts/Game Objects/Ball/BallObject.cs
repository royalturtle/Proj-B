using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallObject : MoveObject {
    const float HeightMax = 16.0f, HeightMin = 0.0f;    

    public float Height {get; private set;}

    [SerializeField] BallOnlyObject Ball;
    [SerializeField] BallShadowObject Shadow;

    Vector3 bigScaleVector, smallScaleVector;
    float bigScaleSize, smallScaleSize;

    // const float catcherYpos = -3.2f, batterYPos = -2.9f;
    const float catcherYpos = -3.2f, batterYPos = -3.1f;
    const float strikeInnerXPos = 0.07f, strikeInnerSwingXPos = 0.09f, strikeOuterXPos = 0.11f, wildPitchingXPos = 0.18f, hbpXPos = 0.22f;

    Vector3 startVectorLeftHand, startVectorRightHand;
    BallHeightInfo _heightInfo;
    bool _isPitching;

    void Start() {
        _heightInfo = new BallHeightInfo();
        bigScaleVector = new Vector3(bigScaleSize, bigScaleSize, bigScaleSize);
        smallScaleVector = new Vector3(smallScaleSize, smallScaleSize, smallScaleSize);

        startVectorLeftHand = new Vector3(0.12f, -1.3f, 0.0f);
        startVectorRightHand = new Vector3(-0.12f, -1.3f, 0.0f);

        if(Shadow != null) {
            Shadow.Ready(HeightMax); 
        }
    }

    public void Init() {
        if(_heightInfo != null) {
            _heightInfo.Clear();
        }
        _isPitching = false;
    }

    public MoveObject SetBallMove(
        Vector3 startVector,
        Vector3 endVector,
        float speed,
        float lastSpeed = 1.0f,
        bool isPitching=false
    ) {
        _isPitching = isPitching;
        return SetWalk(startVector:startVector, endVector:endVector, speed:speed, lastSpeed:lastSpeed);
    }

    public Vector3 GetPitchingPosition(bool isPitcherRightHand) {
        return isPitcherRightHand ? startVectorRightHand : startVectorLeftHand;
    }

    public Vector3 GetEndPosition(BaseballResultTypes result = BaseballResultTypes.NONE, bool isBatterRightHand = true) {
        float xPos = 0.0f, yPos = 0.0f;
        switch(result) {
            case BaseballResultTypes.STRIKE_OUT:
                xPos = UnityEngine.Random.Range(-strikeInnerXPos, strikeInnerXPos);
                yPos = catcherYpos;
                break;
            case BaseballResultTypes.STRIKE_OUT_SWING:
                xPos = UnityEngine.Random.Range(-strikeInnerSwingXPos, strikeInnerSwingXPos);
                yPos = catcherYpos;
                break;
            case BaseballResultTypes.BB:
                xPos = UnityEngine.Random.Range(strikeInnerXPos, strikeOuterXPos);
                xPos *= (UnityEngine.Random.Range(0, 2) == 0) ? 1.0f : -1.0f;
                yPos = catcherYpos;
                break;
            case BaseballResultTypes.WP:
                xPos = UnityEngine.Random.Range(strikeOuterXPos, wildPitchingXPos);
                xPos *= (UnityEngine.Random.Range(0, 2) == 0) ? 1.0f : -1.0f;
                yPos = catcherYpos;
                break;
            case BaseballResultTypes.HBP:
                xPos = UnityEngine.Random.Range(wildPitchingXPos, hbpXPos);
                xPos *= (isBatterRightHand) ? -1.0f : 1.0f;
                yPos = batterYPos;
                break;
            default:
                xPos = UnityEngine.Random.Range(-strikeInnerSwingXPos, strikeInnerSwingXPos);
                yPos = batterYPos;
                break;
        }
        return new Vector3(xPos, yPos, 0.0f);
    }

    public void SetThrowHeight() {
        if(_heightInfo != null) {
            _heightInfo.SetThrowHeight();
        }
    }

    public Vector3 GetHitPosition(DefenseCatchSituation catchSituation) {
        Vector3 result = Vector3.zero;
        
        if(catchSituation != null) {
            BatterPositionEnum position = catchSituation.Position;
            switch(catchSituation.NewResult) {
                // Complete
                case BaseballResultTypes.HIT1:
                    switch(position) {
                        case BatterPositionEnum.LF:
                        case BatterPositionEnum.RF:
                            // result = GetRangeVector(xMin:1.8f, xMax:2.2f, yMin:0.6f, yMax:0.77f, position:position);
                            result = GetRangeVector(xMin:2.2f, xMax:2.6f, yMin:0.77f, yMax:1.2f, position:position);
                            break;
                        case BatterPositionEnum.CF:
                            result = GetRangeVector(xMin:-0.17f, xMax:0.17f, yMin:1.4f, yMax:1.5f, position:position);
                            break;
                    }
                    break;
                case BaseballResultTypes.HIT1_LONG:
                    switch(position) {
                        case BatterPositionEnum.LF:
                        case BatterPositionEnum.RF:
                            // result = GetRangeVector(xMin:1.8f, xMax:2.2f, yMin:0.6f, yMax:0.77f, position:position);
                            result = GetRangeVector(xMin:2.2f, xMax:2.6f, yMin:0.77f, yMax:1.2f, position:position);
                            break;
                        case BatterPositionEnum.CF:
                            result = GetRangeVector(xMin:-0.17f, xMax:0.17f, yMin:1.3f, yMax:1.4f, position:position);
                            break;
                    }
                    break;
                case BaseballResultTypes.HIT2:
                    switch(position) {
                        case BatterPositionEnum.LF:
                        case BatterPositionEnum.RF:
                            result = GetRangeVector(xMin:3.2f, xMax:3.4f, yMin:1.9f, yMax:2.0f, position:position);
                            break;
                        case BatterPositionEnum.CF:
                            result = GetRangeVector(xMin:-0.2f, xMax:0.2f, yMin:3.0f, yMax:3.2f, position:position);
                            break;
                    }
                    break;
                case BaseballResultTypes.HIT2_LONG:
                    switch(position) {
                        case BatterPositionEnum.LF:
                        case BatterPositionEnum.RF:
                            result = GetRangeVector(xMin:3.2f, xMax:3.4f, yMin:1.9f, yMax:2.0f, position:position);
                            break;
                        case BatterPositionEnum.CF:
                            result = GetRangeVector(xMin:-0.2f, xMax:0.2f, yMin:3.0f, yMax:3.2f, position:position);
                            break;
                    }
                    break;
                case BaseballResultTypes.HIT3:
                    switch(position) {
                        case BatterPositionEnum.LF:
                        case BatterPositionEnum.RF:
                            result = GetRangeVector(xMin:3.2f, xMax:3.4f, yMin:1.9f, yMax:2.0f, position:position);
                            break;
                        case BatterPositionEnum.CF:
                            result = GetRangeVector(xMin:-0.2f, xMax:0.2f, yMin:3.0f, yMax:3.2f, position:position);
                            break;
                    }
                    break;
                case BaseballResultTypes.HOMERUN:
                    switch(position) {
                        case BatterPositionEnum.LF:
                        case BatterPositionEnum.RF:
                            result = GetRangeVector(xMin:3.0f, xMax:4.0f, yMin:3.5f, yMax:4.0f, position:position);
                            break;
                        case BatterPositionEnum.CF:
                            result = GetRangeVector(xMin:-1.0f, xMax:1.0f, yMin:4.5f, yMax:5.0f, position:position);
                            break;
                    }
                    break;
                // Complete
                case BaseballResultTypes.FLY_INNER:
                case BaseballResultTypes.INFIELD_FLY:
                case BaseballResultTypes.GROUND_BALL:
                    switch(position) {
                        case BatterPositionEnum.B1:
                            result = GetRangeVector(xMin:1.2f, xMax:1.7f, yMin:-1.0f, yMax:-0.6f, position:position);
                            break;
                        case BatterPositionEnum.B2:
                            result = GetRangeVector(xMin:0.5f, xMax:1.2f, yMin:0.0f, yMax:0.5f, position:position);
                            break;
                        case BatterPositionEnum.B3:
                            result = GetRangeVector(xMin:-1.2f, xMax:-0.5f, yMin:0.0f, yMax:0.5f, position:position);
                            break;
                        case BatterPositionEnum.SS:
                            result = GetRangeVector(xMin:-1.7f, xMax:-1.2f, yMin:-1.0f, yMax:-0.6f, position:position);
                            break;
                    }
                    break;
                // Complete
                case BaseballResultTypes.FLY_OUTSIDE:
                    switch(position) {
                        case BatterPositionEnum.LF:
                        case BatterPositionEnum.RF:
                            result = GetRangeVector(xMin:2.2f, xMax:2.6f, yMin:0.77f, yMax:1.2f, position:position);
                            break;
                        case BatterPositionEnum.CF:
                            result = GetRangeVector(xMin:-0.17f, xMax:0.17f, yMin:1.5f, yMax:1.7f, position:position);
                            break;
                    }
                    break;
            }

            if(_heightInfo != null) {
                _heightInfo.SetHeight(result:catchSituation.NewResult);
            }
        }
        return result;
    }

    Vector3 GetRangeVector(float xMin, float xMax, float yMin, float yMax, BatterPositionEnum position) {
        return new Vector3(
            UnityEngine.Random.Range(xMin, xMax) * (position == BatterPositionEnum.LF ? -1.0f : 1.0f),
            UnityEngine.Random.Range(yMin, yMax),
            0.0f
        );
    }

    public void SetActive(bool value) {
        gameObject.SetActive(value);
        if(!value) {
            SetHeight(0.0f);
        }
    }

    public void SetHeight(float height) {
        height = HeightMin + (HeightMax - HeightMin) * height;

        if(Ball != null) {
            Ball.Move(height, HeightMax);
        }

        if(Shadow != null) {
            Shadow.SetShadow(height);
        }
    }

    protected override void WalkAfter(bool isEnd) {
        if(!_isPitching) {
            if(_heightInfo != null) {
                SetHeight(
                    height:_heightInfo.GetHeight(
                        relativeXpos:RelativeMagnitude
                    )
                );
            }
        }
        else {
            SetHeight(height:0.1f);
        }
        
    }
}
