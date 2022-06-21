using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class TitleSceneManager : SceneBase {
    [SerializeField] Button _continueButton;

    protected override void StartAfter() {
        if(_continueButton) {
            _continueButton.gameObject.SetActive(SaveManager.IsCurrentSaveExist());
        }
    }

    public void ContinueGame() {
        if(SaveManager.IsCurrentSaveExist()) {
            TransitionSlide(
                workAction:delegate {
                    LoadInGameScene();
                },
                mode:TransitionModes.COROUTINE
            );
        }
        else {
            if(_askDialog) {
                _askDialog.Open(text:"NO DATA");
            }
        }
    }

    public void NewGame() {
        if(SaveManager.IsCurrentSaveExist()) {
            _askDialog.Open(
                text:LanguageSingleton.Instance.GetWarning(1),
                okAction: () => {
                    SaveManager.DeleteCurrentData();
                    GotoNewGameScene();
                }
            );
        }
        else {
            GotoNewGameScene();
        }
    }

    void GotoNewGameScene() {
        TransitionSlide(
            workAction:delegate {
                LoadNewGameScene();
            },
            mode:TransitionModes.NORMAL
        );
    }

    public void LoadGame(int index) {
        LoadInGameScene();
    }
}
