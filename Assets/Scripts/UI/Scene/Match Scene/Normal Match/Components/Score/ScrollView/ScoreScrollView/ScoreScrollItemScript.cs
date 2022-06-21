using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreScrollItemScript : MonoBehaviour {
    [SerializeField] TextMeshProUGUI _inningText, _homeText, _awayText;

    public void SetInning(int value) {
        if(_inningText) { _inningText.text = value.ToString();}
    }

    public void SetScoreHome(int value) {
        if(_homeText) { _homeText.text = value.ToString(); }
    }

    public void SetScoreAway(int value) {
        if(_awayText) { _awayText.text = value.ToString(); }
    }
}
