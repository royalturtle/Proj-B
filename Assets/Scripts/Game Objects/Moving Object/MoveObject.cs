using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObject : MonoBehaviour {
    public bool IsFinished {get; protected set;}
    public Vector3 StartVector {get; protected set;}
    public Vector3 EndVector {get; protected set;}
    public Vector3 DirectionVector {get; protected set;}
    public float EndMagnitude {get; protected set;}
    public float CurrentMagnitude {get; protected set;}
    public float RelativeMagnitude {get { return CurrentMagnitude / EndMagnitude; }}
    protected float WalkSpeed = 1.0F, _acceleration, _startSpeed, _lastSpeed;

    public MoveObject SetWalk(
        Vector3 startVector,
        Vector3 endVector,
        float speed = 1.0f,
        float lastSpeed = 1.0f
    ) {
        SetIsFinished(false);
        StartVector = startVector;
        EndVector = endVector;
        WalkSpeed = speed;
        _startSpeed = WalkSpeed;
        _lastSpeed = lastSpeed;
        transform.localPosition = StartVector;
        CurrentMagnitude = 0.0f;
        EndMagnitude = (endVector - startVector).magnitude;
        DirectionVector = (endVector - startVector).normalized;
        SetAcceleration();
        return this;
    }

    public MoveObject SetWalkByTime(
        Vector3 startVector,
        Vector3 endVector,
        float timeSpend,
        float speed = 1.0f
    ) {
        float magnitude = (endVector - startVector).magnitude;
        Vector3 directionVector = (endVector - startVector).normalized;

        return SetWalk(
            startVector:startVector + directionVector * (magnitude - speed * timeSpend),
            endVector:endVector,
            speed:speed,
            lastSpeed:speed
        );
    }

    void SetAcceleration() {
        _acceleration = _startSpeed == _lastSpeed ? 0.0f : (_lastSpeed * _lastSpeed - _startSpeed * _startSpeed) / (2.0f * EndMagnitude);
    }

    public float TimeNeeded { get {
        return (_lastSpeed - _startSpeed) / _acceleration;
    }}

    public void Walk() {
        CurrentMagnitude += Time.deltaTime * WalkSpeed;
        WalkSpeed += (_acceleration * Time.deltaTime);
        bool isEnd = CurrentMagnitude >= EndMagnitude;
        WalkAfter(isEnd);
        if(isEnd) {
            transform.localPosition = EndVector;
            SetIsFinished(true);
        }
        else {
            transform.localPosition = StartVector + (DirectionVector * CurrentMagnitude);
        }
    }

    protected virtual void WalkAfter(bool isEnd) {}

    void SetIsFinished(bool value) {
        IsFinished = value;        
    }
}
