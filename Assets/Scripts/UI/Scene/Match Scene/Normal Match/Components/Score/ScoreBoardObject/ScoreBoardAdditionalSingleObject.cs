using UnityEngine;
using TMPro;

public class ScoreBoardAdditionalSingleObject : MonoBehaviour {
    [SerializeField] TextMeshProUGUI _homeText, _awayText;

    public void SetData(MatchPosition matchPosition, int value) {
        if (matchPosition == MatchPosition.HOME) {_homeText.text = value.ToString(); }
        else { _awayText.text = value.ToString(); }
    }
}
