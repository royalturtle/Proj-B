using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DayLayout : MonoBehaviour {
    [SerializeField] TextMeshProUGUI _dayText, _homeAwayText, _resultText, _scoreText;
    [SerializeField] Image _oppLogoImage, _darkenImage;
    [SerializeField] GameObject _speicalExpressObj;
    [SerializeField] GameObject _todayObject, _selectObject;
    
    public int Turn {get; private set;}
    Button _button;

    string specialDayDescription = "";

    void Awake() {
        _button = GetComponent<Button>();
    }

    public void Ready(Action<DayLayout> clickAction) {
        if(_button) {
            _button.onClick.AddListener(() => {
                if(clickAction != null) {
                    clickAction(this);
                }
            });
        }
    }

    public void SetToday(bool value) {
        if(_todayObject != null) {
            _todayObject.SetActive(value);
        }
    }

    public void SetSelected(bool value) {
        if(_selectObject != null) {
            _selectObject.SetActive(value);
        }
    }

    public void SetData(
        int day, 
        int turn,
        bool isClickAble=true,
        string specialStr="",
        MatchInfo matchInfo=null,
        int myTeamId = 1,
        int todayTurn = -1
    ) {
        Turn = turn;

        if(_button != null) {
            _button.interactable = isClickAble;
        }
        if (_dayText != null) { 
            _dayText.text = day.ToString(); 
        }
        
        if(_darkenImage != null) {
            _darkenImage.gameObject.SetActive(!isClickAble);
        }

        if(_speicalExpressObj != null) {
            specialDayDescription = specialStr;
            if(specialDayDescription == "") { _speicalExpressObj.gameObject.SetActive(false); }
            else { _speicalExpressObj.gameObject.SetActive(true); }
        }

        // empty match info first, Reset
        if(_resultText != null) { _resultText.text = ""; }
        if(_scoreText != null) { _scoreText.text = ""; }
        if(_homeAwayText != null) { _homeAwayText.text =""; }

        if(_todayObject) {
            _todayObject.SetActive(todayTurn == Turn);
        }

        // Match List
        if(matchInfo != null) {
            TupleMatch match = matchInfo.MatchData;
            PlayerStatusInMatch status = match.IsTeamIDExist(myTeamId);
            if(status != PlayerStatusInMatch.NONE) {
                // Home Away
                if(_homeAwayText != null) {
                    _homeAwayText.text = status == PlayerStatusInMatch.HOME ? "H" : "A";
                }
            
                // Opp Team Logo
                if(_oppLogoImage) {
                    _oppLogoImage.gameObject.SetActive(true);
                    Team oppTeam = matchInfo.GetTeam(status:status, isOpp:true);
                    if(oppTeam != null) {
                        _oppLogoImage.sprite = ResourcesUtils.GetTeamIconImage(oppTeam.LogoName);
                    }
                }

                // IF Match Result is Existed
                if(match.IsFinished != 0) {
                    // Result Text
                    if(_resultText != null) {
                        if(match.IsTeamDraw(myTeamId)) { 
                            _resultText.text = "D"; 
                            _resultText.color = ColorConstants.Instance.White;
                        }
                        else if(match.IsTeamWin(myTeamId)) {
                            _resultText.text = "W"; 
                            _resultText.color = ColorConstants.Instance.Green;
                        }
                        else { 
                            _resultText.text = "L"; 
                            _resultText.color = ColorConstants.Instance.Red;
                        }
                    }

                    if(_scoreText != null) {
                        _scoreText.text = match.HomeScore.ToString() + "-" + match.AwayScore.ToString() ;
                    }
                }
                // If match is existed, but not finished
                else {
                    if(_resultText != null) { _resultText.text = ""; }
                    if(_scoreText != null) { _scoreText.text = ""; } 
                }
            }
            else {
                if(_oppLogoImage) { 
                    _oppLogoImage.gameObject.SetActive(false);
                }
            }   
        }
        else {
            if(_oppLogoImage) { 
                _oppLogoImage.gameObject.SetActive(false);
            }
        }


    }
}
