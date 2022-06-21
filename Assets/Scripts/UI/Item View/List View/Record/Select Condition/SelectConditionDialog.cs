using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectConditionDialog : DialogBase {
    [SerializeField] Button _confirmButton, _initButton;
    [SerializeField] protected List<SelectConditionListView> conditionViewList;
    [SerializeField] SetConditionDialog _setConditionDialog;

    public virtual void Ready() {}
    
    protected void SetPanels(int[] pages, int start, int end) {
        if(pages != null && pages.Length > 0) {
            int currentIndex = start;
            for(int pageIndex = 0; pageIndex < pages.Length; pageIndex++) {
                int page = pages[pageIndex];
                if(Utils.IsValidIndex(conditionViewList, page)) {
                    int addMax = conditionViewList[page].Count;
                    for(int addCount = 0; addCount < addMax; addCount++) {
                        conditionViewList[page].SetData(
                            index : addCount, 
                            key   : GetTextKey(),
                            value : currentIndex <= end ? currentIndex++ : GameConstants.NULL_INT
                        );
                    }
                }
            }
        }
    }

    void OpenSetConditionDialog(LocalizationTypes localize, RecordCondition condition) {
        if(_setConditionDialog) {
            _setConditionDialog.Open(
                localize:localize, condition:condition
            );
        }
    }

    protected virtual LocalizationTypes GetTextKey() {return LocalizationTypes.GUI;}
}
