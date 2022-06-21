using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchSceneManager : SceneBase {

    bool _isQuickMatch;

    [SerializeField] NormalMatchManagerObject _normalMatchManagerObj;
    [SerializeField] QuickMatchManagerObject _quickMatchManagerObj;

    protected override void StartAfter() {
        _isQuickMatch = _notDestroyObj.MatchArguments.IsQuickMatch;

        if(_isQuickMatch) {
            if(_quickMatchManagerObj) {
                _quickMatchManagerObj.SetData(GameData, _notDestroyObj.MatchArguments);
            }
        }
        else {
            if(_normalMatchManagerObj) {
                _normalMatchManagerObj.SetData(GameData);
            }
        }
    }

    void Update() {
        if(_isQuickMatch) {
            _quickMatchManagerObj.Progress();
        }
        else {
            _normalMatchManagerObj.Progress();
        }
    }

    public void AskExitAndGoToLobby() {
        if(_askDialog) {
            _askDialog.Open(
                text : LanguageSingleton.Instance.GetMatchScene(9),
                okAction: () => {
                    
                    TransitionSlide(
                        workAction : () => {
                            LoadInGameScene();
                        },
                        mode:TransitionModes.COROUTINE
                    );
                }
            );
        }
    }   

    public void GoToLobby() {
        TransitionSlide(
            workAction : () => {
                LoadInGameScene();
            },
            mode:TransitionModes.COROUTINE
        );
    }
}
