using UnityEngine;

public class ScoreBoardAdditionalObject : MonoBehaviour {
    [SerializeField]
    ScoreBoardAdditionalSingleObject _scoreObj, _hitObj, _errorObj, _bbObj;

    public void SetScore(MatchPosition matchPosition, int value) {
        _scoreObj.SetData(matchPosition, value);
    }

    public void SetHit(MatchPosition matchPosition, int value) {
        _hitObj.SetData(matchPosition, value);
    }

    public void SetError(MatchPosition matchPosition, int value) {
        _errorObj.SetData(matchPosition, value);
    }

    public void SetBB(MatchPosition matchPosition, int value) {
        _bbObj.SetData(matchPosition, value);
    }
}
