using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewGameSceneManager : SceneBase {
    int _index;
    [SerializeField] NewGameNamePanel _namePanel;
    List<SequencePanel> _panelList;
    [SerializeField] Button _exitButton, _prevButton, _nextButton, _submitButton;

    protected override void AwakeAfter() {
        _panelList = new List<SequencePanel> {_namePanel};
        if(_namePanel) {
            _namePanel.CheckAction = CheckNextAble;
        }

        if(_nextButton) {_nextButton.interactable = false;}
        if(_submitButton) {_submitButton.interactable = false;}
    }

    protected override void StartAfter() {
        TurnOnCurrent();
    }

    public void NewGame() {
        if(_namePanel != null) {
            TransitionSlide(
                threadAction : () => {
                    if(_notDestroyObj != null) {
                        _notDestroyObj.CreateNewGame(
                            teamName: _namePanel.TeamName, 
                            ownerName: _namePanel.OwnerName
                        );
                    }
                },
                workAction : () => {
                    LoadInGameScene();
                },
                mode:TransitionModes.THREAD,
                isPercentage:true
            );
        }
        
    }

    public void BackToTitle() {
        TransitionSlide(
            workAction:delegate {
                LoadTitleScene();
            },
            mode:TransitionModes.NORMAL
        );
    }

    void CheckNextAble(bool value) {
        if(!IsEnd) {
            if(_nextButton) {
                _nextButton.interactable = true;
            }
        }
        else {
            if(_submitButton) {
                _submitButton.interactable = value;
            }
        }
        
    }

    void SetOffAll() {
        if(_panelList != null) {
            for(int i = 0; i < _panelList.Count; i++) {
                _panelList[i].gameObject.SetActive(false);
            }
        }
    }

    void TurnOnCurrent() {
        SetOffAll();

        if(Utils.IsValidIndex(_panelList, _index)) {
            _panelList[_index].gameObject.SetActive(true);

            if(_prevButton) {
                _prevButton.gameObject.SetActive(_index > 0);
            }

            bool isEnd = IsEnd;
            if(_nextButton) {
                _nextButton.gameObject.SetActive(!isEnd);
            }
            if(_submitButton) {
                _submitButton.gameObject.SetActive(isEnd);
            }
        }
    }

    bool IsEnd { get { return _index >= _panelList.Count - 1; } }
}
