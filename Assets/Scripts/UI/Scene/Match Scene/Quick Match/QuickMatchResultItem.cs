using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuickMatchResultItem : MonoBehaviour {
    [SerializeField] LocalizeScriptWithOther _dateText;
    [SerializeField] TextMeshProUGUI _homeScoreText, _awayScoreText, _resultText;
    [SerializeField] TeamSingleRecordBasic _awayTeam, _homeTeam;
    [SerializeField] GameObject _scoreObj, m_contentLayer;
    [SerializeField] Animator _loadingAnimator;
    Func<int, Sprite> _getTeamLogoAction;

    [SerializeField] GameObject _myAwayTeamImage, _myHomeTeamImage;

    public void Ready(Func<int, Sprite> getTeamLogoAction) {
        _getTeamLogoAction = getTeamLogoAction;
    }

    Sprite GetTeamLogo(int teamId) {
        return (_getTeamLogoAction == null) ? null : _getTeamLogoAction(teamId);
    }

    public void SetData(QuickMatchData quickMatchData, bool isShowFrame = true) {
        if(quickMatchData != null && quickMatchData.IsMatchRegistered) {
            this.gameObject.SetActive(true);
            if(m_contentLayer != null) {
                m_contentLayer.SetActive(true);
            }
            
            if(_dateText) {
                _dateText.SetData(prefix:quickMatchData.Date + "-", postfix:")", key:(int)quickMatchData.DayName);
            }

            if(_awayTeam) {
                _awayTeam.SetTeamData(
                    name:quickMatchData.AwayTeam.Name,
                    sprite:GetTeamLogo(quickMatchData.AwayTeam.ID)
                );
            }

            if(_homeTeam) {
                _homeTeam.SetTeamData(
                    name:quickMatchData.HomeTeam.Name,
                    sprite:GetTeamLogo(quickMatchData.HomeTeam.ID)
                );
            }

            if(_scoreObj != null) {
                if (quickMatchData.IsMatchFinished) {
                    SetScore(
                        homeScore: quickMatchData.ScoreHome,
                        awayScore: quickMatchData.ScoreAway,
                        playerStatus: quickMatchData.PlayerStatus
                    );
                }
                else {
                    _scoreObj.SetActive(false);
                }
            }
            bool isMyTeamHome = false, isMyTeamAway= false;

            if(quickMatchData != null) {
                switch(quickMatchData.PlayerStatus) {
                    case PlayerStatusInMatch.HOME:
                        isMyTeamHome = true;
                        break;
                    case PlayerStatusInMatch.AWAY:
                        isMyTeamAway = true;
                        break;
                }
            }
        
            if(_myAwayTeamImage) {
                _myAwayTeamImage.SetActive(isMyTeamAway);
            }
            if(_myHomeTeamImage) {
                _myHomeTeamImage.SetActive(isMyTeamHome);
            }
        }

        else if(quickMatchData != null  && !quickMatchData.IsMatchRegistered) {
            this.gameObject.SetActive(true);
            if(m_contentLayer != null) {
                m_contentLayer.SetActive(false);
            }
            
            if(_dateText) {
                _dateText.SetData(prefix:quickMatchData.Date + "-", postfix:")", key:(int)quickMatchData.DayName);
            }
        }
        else {
            this.gameObject.SetActive(false);
            if(m_contentLayer != null) {
                m_contentLayer.SetActive(false);
            }
        }
    }

    public bool IsEmpty {
        get {return !m_contentLayer.activeSelf; }
    }

    public void SetAnimator(bool value) {
        if(_loadingAnimator) {
            _loadingAnimator.SetTrigger("Turn" + (value ? "On" : "Off"));
        }
    }

    public void ChangedSelected(bool isSelected) {
        Image layoutImage = gameObject.GetComponent<Image>();

        if (layoutImage != null) {
            layoutImage.color = (isSelected) ? new Color32(0, 147, 123, 255) : new Color32(51, 51, 51, 255);
        }
    }

    public int SetScore(int homeScore, int awayScore, PlayerStatusInMatch playerStatus) {
        int result = 0;
        if(_scoreObj != null) {
            if (_homeScoreText!= null) {
                _homeScoreText.text = homeScore.ToString();
            }
            if (_awayScoreText != null) {
                _awayScoreText.text = awayScore.ToString();
            }

            if(_resultText != null) {
                if (homeScore == awayScore) {
                    _resultText.text = "DRAW";
                }
                else if (playerStatus == PlayerStatusInMatch.HOME) {
                    _resultText.text = (homeScore > awayScore) ? "WIN" : "LOSE";
                    _resultText.color = (homeScore > awayScore) ? ColorConstants.Instance.Green : ColorConstants.Instance.Red;
                    result = (homeScore > awayScore) ? 1 : -1;
                }
                else {
                    _resultText.text = (homeScore > awayScore) ? "LOSE" : "WIN";
                    _resultText.color = (homeScore > awayScore) ? ColorConstants.Instance.Red : ColorConstants.Instance.Green;
                    result = (homeScore > awayScore) ? -1 : 1;
                }
            }

            _scoreObj.SetActive(true);
        }
        return result;
    }
}
