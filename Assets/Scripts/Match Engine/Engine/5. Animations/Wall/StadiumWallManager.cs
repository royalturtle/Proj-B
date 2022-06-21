using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StadiumWallManager : MonoBehaviour {
    [SerializeField] List<StadiumWallObject> _lfWallList;
    [SerializeField] List<StadiumWallObject> _cfWallList;
    [SerializeField] List<StadiumWallObject> _rfWallList;

    public void Init() {
        InitList(_lfWallList);
        InitList(_cfWallList);
        InitList(_rfWallList);
    }

    void InitList(List<StadiumWallObject> list) {
        if(list != null) {
            for(int i = 0; i < list.Count; i++) {
                if(list[i] != null) {
                    list[i].Init();
                }
            }
        }
    }

    public void Ready(Action wallHitAction) {
        ReadyList(_lfWallList, wallHitAction);
        ReadyList(_cfWallList, wallHitAction);
        ReadyList(_rfWallList, wallHitAction);
    }

    void ReadyList(List<StadiumWallObject> list, Action wallHitAction) {
        if(list != null) {
            for(int i = 0; i < list.Count; i++) {
                if(list[i] != null) {
                    list[i].Ready(wallHitAction);
                }
            }
        }
    }

    public Vector3 SetData(Vector3 afterVector, BatterPositionEnum position) {
        Vector3 result = Vector3.zero;
        List<StadiumWallObject> list = null;
        switch(position) {
            case BatterPositionEnum.LF:
                list = _lfWallList;
                break;
            case BatterPositionEnum.CF:
                list = _cfWallList;
                break;
            case BatterPositionEnum.RF:
                list = _rfWallList;
                break;
        }

        if(list != null) {
            StadiumWallObject wall = list[UnityEngine.Random.Range(0, list.Count)];
            if(wall != null) {
                wall.SetOn();
                result = wall.transform.localPosition;
            }
        }
        
        return result;
    }
}
