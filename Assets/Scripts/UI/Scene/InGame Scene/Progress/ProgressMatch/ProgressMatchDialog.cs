using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressMatchDialog : DialogBase {
    [SerializeField] Button _confirmButton, _viewButton;
    [SerializeField] TeamSingleRecordBasic _awayTeam, _homeTeam;
    [SerializeField] GameObject _awayTeamImage, _homeTeamImage;

    Func<int, Sprite> _getTeamLogo;
    MatchInfo _matchInfo;

    public void Ready(
        Action confirmAction, 
        Action<Team> viewAction,
        Func<int, Sprite> getTeamLogo
    ) {
        _getTeamLogo = getTeamLogo;

        if(_confirmButton) {
            _confirmButton.onClick.RemoveAllListeners();
            _confirmButton.onClick.AddListener(() => {
                if(confirmAction != null) {
                    confirmAction();
                }
            });
        }

        if(_viewButton) {
            _viewButton.onClick.RemoveAllListeners();
            _viewButton.onClick.AddListener(() => {
                if(_matchInfo != null && viewAction != null) {
                    viewAction(_matchInfo.GetOppTeam());
                }
                TurnActive(false);
            });
        }

    }

    public void SetData(MatchInfo matchInfo) {
        _matchInfo = matchInfo;

        if(_matchInfo != null) {
            if(_getTeamLogo != null) {
                if(_awayTeam != null) {
                    _awayTeam.SetTeamData(
                        name:_matchInfo.AwayTeam.Name,
                        sprite:_getTeamLogo(_matchInfo.AwayTeam.ID)
                    );
                }

                if(_homeTeam != null) {
                    _homeTeam.SetTeamData(
                        name:_matchInfo.HomeTeam.Name,
                        sprite:_getTeamLogo(_matchInfo.HomeTeam.ID)
                    );
                }
            }
            
            bool isAwayTeam = false, isHomeTeam = false;
            switch(_matchInfo.PlayerStatus) {
                case PlayerStatusInMatch.HOME:
                    isHomeTeam = true;
                    break;
                case PlayerStatusInMatch.AWAY:
                    isAwayTeam = true;
                    break;
            }

            if(_awayTeamImage) {
                _awayTeamImage.SetActive(isAwayTeam);
            }

            if(_homeTeamImage) {
                _homeTeamImage.SetActive(isHomeTeam);
            }
        }
    }
}
