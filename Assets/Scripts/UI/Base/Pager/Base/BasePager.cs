using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePager : MonoBehaviour {
    public int CurrentPage {get; private set;}
    protected int _totalPage;
    protected bool _isLoopable;
    [SerializeField] protected bool Interactable = true;

    public event System.Action<int> PageChanged = (page) => { };
    
    void Awake() {
        SetInteractable(Interactable);
        ChangeCurrentPage(0);
        AwakeAfter();
    }
    protected virtual void AwakeAfter() {}

    void Start() {
        StartAfter();
    }
    protected virtual void StartAfter() {}

    public void SetInteractable(bool value) {
        Interactable = value;
        SetInteractableAfter();
    }

    protected void SetInteractableAfter() {}
    
    public void InitObject(int totalPage, bool isLoopable=true, bool isUpdate=false) {
        _isLoopable = isLoopable;
        SetTotalPage(totalPage);

        if(!isUpdate) {
            ChangeCurrentPage(0);
        }
        else if(CurrentPage >= totalPage) {
            ChangeCurrentPage(totalPage - 1);
        }
        else {
            ChangeCurrentPage(CurrentPage);
        }
    }

    public void PreviousPage() {
        int index = CurrentPage - 1;
        if(index < 0) {
            if(_isLoopable) {
                ChangeCurrentPage(_totalPage - 1);
            }
        }
        else {
            ChangeCurrentPage(index);
        }
    }

    public void NextPage() {
        int index = CurrentPage + 1;
        if(index >= _totalPage) {
            if(_isLoopable) {
                ChangeCurrentPage(0);
            }
        }
        else {
            ChangeCurrentPage(index);
        }
    }

    public void SetPage(int page, bool isForce = false) {
        if(CurrentPage != page) { 
            ChangeCurrentPage(page, isForce); 
        }
    }

    protected void ChangeCurrentPage(int currentPage, bool isForce = false) {
        if((Interactable || isForce) && currentPage >= 0 && currentPage < _totalPage) { 
            
            CurrentPage = currentPage;
            ChangeUI();
            PageChanged(CurrentPage);
        }
    }

    protected virtual void ChangeUI() {}

    void SetTotalPage(int totalPage) {
        _totalPage = totalPage;
        ChangeTotalUI();
    }

    protected virtual void ChangeTotalUI() {}
}
