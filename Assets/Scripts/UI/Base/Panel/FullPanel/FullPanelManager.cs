using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullPanelManager {
    List<(InGameScenePanelModes mode, int id)> _panelList;
    Dictionary<InGameScenePanelModes, FullPanel> _panelModeDict;
    
    public FullPanelManager(Dictionary<InGameScenePanelModes, FullPanel> panelDict) {
        _panelList = new List<(InGameScenePanelModes mode, int id)>();
        _panelModeDict = panelDict;
    }

    public void NextPanel(InGameScenePanelModes mode, int id = 0) {
        if(Utils.NotNull(_panelModeDict, _panelList)) {
            FullPanel turnOffPanel = null;
            if(_panelList.Count >= 1) {
                turnOffPanel = GetPanelObject(_panelList[_panelList.Count - 1].mode);
            }

            FullPanel turnOnPanel = GetPanelObject(mode);
            _panelList.Add((mode, id));

            TurnOnAndOffPanel(turnOnPanel, turnOffPanel);
        }
    }

    public void PrevPanel() {
        if(Utils.NotNull(_panelModeDict, _panelList))  {
            if(_panelList.Count == 1) {
                ClosePanel();
            }
            else if(_panelList.Count > 1) {
                FullPanel turnOnPanel = null;
                if(_panelList.Count > 1) {
                    turnOnPanel = GetPanelObject(_panelList[_panelList.Count - 2].mode);
                }

                FullPanel turnOffPanel = GetPanelObject(_panelList[_panelList.Count - 1].mode);
                _panelList.RemoveAt(_panelList.Count - 1);

                TurnOnAndOffPanel(turnOnPanel, turnOffPanel);
            }
        }
    }

    void TurnOnAndOffPanel(FullPanel turnOnPanel, FullPanel turnOffPanel) {
        if(turnOnPanel) {
            turnOnPanel.SetBackButton(_panelList.Count >= 2);
            turnOnPanel.gameObject.SetActive(true);
        }
        if(turnOffPanel) {
            turnOffPanel.gameObject.SetActive(false);
        }
    }

    public void ClosePanel() {
        if(_panelList != null && _panelList.Count > 0) {
            FullPanel panel = GetPanelObject(_panelList[_panelList.Count - 1].mode);
            if(panel) { 
                panel.gameObject.SetActive(false); 
            }
            _panelList.Clear();

            for(int i = 0; i < _panelList.Count - 1; i++) {
                panel = GetPanelObject(_panelList[i].mode);
                if(panel != null) {
                    panel.BeforeClose();
                }
            }
        }
    }

    FullPanel GetPanelObject(InGameScenePanelModes mode) {
        FullPanel result = null;
        if(_panelModeDict.ContainsKey(mode)) {
            result = _panelModeDict[mode];
        }
        return result;
    }
}
