using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneBase : MonoBehaviour {
    [SerializeField] NotDestroyObject _notDestroyObjOrig;
    protected NotDestroyObject _notDestroyObj;
    public GameDataMediator GameData { get { return _notDestroyObj != null ? _notDestroyObj.GameData : null; } }
    
    [SerializeField] protected AskDialog _askDialog;
    [SerializeField] protected CheckDialog _checkDialog;

    void Awake() {
        GameObject notDestroyObjParent = GameObject.FindWithTag(GameConstants.TAG_NOT_DESTROY_OBJECT);
        if(notDestroyObjParent == null && _notDestroyObjOrig != null) { 
            notDestroyObjParent = Instantiate(_notDestroyObjOrig).gameObject; 
        }
        if(notDestroyObjParent != null) {
            _notDestroyObj = notDestroyObjParent.GetComponent<NotDestroyObject>();
            if(_notDestroyObj != null) {
                _notDestroyObj.StartAction();
            }
        }
        
        AwakeAfter();
    }
    protected virtual void AwakeAfter() {}

    void Start() {
        StartAfter();
    }
    protected virtual void StartAfter() { }

    protected void LoadTitleScene() {
        SceneManager.LoadScene(1);
    }

    protected void LoadNewGameScene() {
        SceneManager.LoadScene(2);
    }

    public void LoadInGameScene() {
        SceneManager.LoadScene(3);
    }

    protected void LoadMatchScene() {
        SceneManager.LoadScene(4);
    }

    protected void CheckQuitGame() {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            if(_askDialog != null) {
                _askDialog.Open(text:LanguageSingleton.Instance.GetGUI(27), okAction:QuitGame);
            }
        }
    }

    public void LoadCurrentGame() {
        if(_notDestroyObj != null) {
            _notDestroyObj.LoadCurrentGame();
        }
    }

    public void TransitionGradient(
        Action workAction, 
        TransitionModes mode, 
        Action threadAction = null,
        bool isPercentage = false,
        bool isDate = false,
        byte alpha = 255
    ) {
        if(_notDestroyObj != null) {
            _notDestroyObj.Transition.SetGradient(
                value         : true, 
                mode          : mode,
                workingAction : workAction,
                threadAction  : threadAction,
                isPercentage  : isPercentage,
                isDate        : isDate,
                alpha         : alpha
            );
        }
    }

    public void TransitionSlide(
        Action workAction,
        TransitionModes mode,
        Action threadAction = null,
        bool isPercentage = false,
        bool isDate = false,
        byte alpha = 255
    ) {
        if(_notDestroyObj != null) {
            _notDestroyObj.Transition.SetSlide(
                value         : true, 
                mode          : mode,
                workingAction : workAction,
                threadAction  : threadAction,
                isPercentage  : isPercentage,
                isDate        : isDate,
                alpha         : alpha
            );
        }
    }

    public void AskLoadTitleScene() {
        OpenAskDialog(
            text:LanguageSingleton.Instance.GetWarning(2),
            okAction:() => {
                TransitionSlide(
                    workAction: () => {
                        LoadTitleScene();
                    },
                    mode:TransitionModes.COROUTINE
                );
            }
        );
    }

    public void OpenAskDialog(string text, Action okAction=null, Action closeAction=null) {
        if(_askDialog) {
            _askDialog.Open(
                text        : text, 
                okAction    : okAction,
                closeAction : closeAction
            );
        }
    }

    public void AskQuitGame() {
        OpenAskDialog(
            text:LanguageSingleton.Instance.GetWarning(3),
            okAction:() => {
                QuitGame();
            }
        );
    }

    public void QuitGame() {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }
}
