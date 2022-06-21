using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.UI;

public class SaveDialog : DialogBase {
    [SerializeField] Button m_saveButton, m_loadButton, m_deleteButton;
    [SerializeField] ToggleGroup m_toggleGroup;
    [SerializeField] AskDialog _askDialog;
    [SerializeField] List<SaveView> _saveObjList;
    [SerializeField] bool _isSaveAble;
    [SerializeField] SceneBase _scene;

    int SelectedIndex;

    void Start() {
        List<SaveData> saveDataList = SaveManager.GetSaveList();
        SaveView saveView = null;
        for(int i = 0; i < ChildCount; i++) {
            saveView = _saveObjList[i];
            saveView.m_toggle.onValueChanged.AddListener((isSelected) => {
                ToggleChanged();
                // if(isSelected) { ToggleChanged(); }
            });
            saveView.SetData(saveDataList[i]);
        }

        if(m_saveButton) { 
            m_saveButton.onClick.RemoveAllListeners();
            m_saveButton.onClick.AddListener(() => {
                int selected = SelectedIndex;
                if(SaveManager.IsSaveExist(selected)) {
                    if(_askDialog) {
                        _askDialog.Open(text:"삭제하고 저장?", okAction:DeleteAndSaveData);
                    }
                }
                else {
                    SaveData();
                }
            });
        }

        if(m_deleteButton) {
            m_deleteButton.onClick.RemoveAllListeners();
            m_deleteButton.onClick.AddListener(() => {
                if(_askDialog) {
                    _askDialog.Open(text:"삭제?", okAction:DeleteSaveData);
                }
            });
        }

        if(m_loadButton) {
            m_loadButton.onClick.RemoveAllListeners();
            m_loadButton.onClick.AddListener(() => {
                if(_askDialog) {
                    _askDialog.Open(text:"삭제하고 불러오기?", okAction:LoadData);
                }
            });

        }

        if(m_saveButton != null) { m_saveButton.gameObject.SetActive(_isSaveAble); }
        ToggleChanged();
    }

    void ToggleChanged() {
        // https://stackoverflow.com/questions/52739763/how-to-get-selected-toggle-from-toggle-group
        Toggle ActiveToggle = m_toggleGroup.ActiveToggles().FirstOrDefault();
        if(ActiveToggle) {
            SaveView activeToggleData = ActiveToggle.gameObject.GetComponent<SaveView>();
            SelectedIndex = activeToggleData.Index;
            ReadyUiIsLoadable(isLoadable:!activeToggleData.IsEmpty);
            if(m_saveButton != null) { m_saveButton.interactable = true; }
        }
        else {
            SelectedIndex = 0;
            ReadyUiIsLoadable(isLoadable:false);
            if(m_saveButton != null) { m_saveButton.interactable = false; }
        }
    }

    // [-1] : None, [0] : Current, [else] : Index
    public int SelectedSaveDataIndex {
        get {
            SaveView toggleObject = GetSelectedToggle();
            return toggleObject != null ? toggleObject.Index : -1;
        }
    }

    SaveView GetSelectedToggle() {
        int childCount = ChildCount;
        for (int i = 0; i < childCount; i++) {
            SaveView toggleObject = _saveObjList[i];
            if(toggleObject != null && toggleObject.isOn) { return toggleObject; }
        }
        return null;
    }

    #region PlayerActions
    void SaveData() {
        int selected = SelectedIndex;
        bool result = SaveManager.SaveData(selected);
        if(result) {
            _saveObjList[selected - 1].SetData(SaveManager.GetSaveData(selected));
        }
    }

    void DeleteAndSaveData() {
        DeleteSaveData();
        SaveData();
    }

    void DeleteSaveData() {
        int selected = SelectedIndex;
        bool deleteResult = SaveManager.DeleteData(selected);
        if(deleteResult) {
            _saveObjList[selected - 1].SetData(null);
            ReadyUiIsLoadable(false);
        }
    }

    void LoadData() {
        if(_scene == null) { return; }

        SaveManager.DeleteCurrentData();
        bool result = SaveManager.LoadData(SelectedIndex);
        if(result) {
            _scene.TransitionSlide(
                workAction: () => {
                    _scene.LoadCurrentGame();
                    _scene.LoadInGameScene();
                },
                mode:TransitionModes.COROUTINE
            );
        }
    }
    #endregion

    void ReadyUiIsLoadable(bool isLoadable) {
        if(m_loadButton != null) { m_loadButton.interactable = isLoadable; }
        if(m_deleteButton != null) { m_deleteButton.interactable = isLoadable; }
    }

    int ChildCount { 
        get { 
            return (_saveObjList == null) ? 0 : _saveObjList.Count; 
        } 
    }
}
