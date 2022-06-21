using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScheduleBaseDialog : DialogBase {
    protected TurnManager _turnManager;
    [SerializeField] Button _backMonthButton, _nextMonthButton;
    [SerializeField] TextMeshProUGUI _yearText, _monthText;

    Dictionary<int, MatchInfo> _myMatchDict;
    protected MatchInfo GetMatch(int turn) {
        return _myMatchDict == null || !_myMatchDict.ContainsKey(turn) ? null : _myMatchDict[turn];
    }
    [SerializeField] List<DayLayout> _dayObjectList;

    protected int _selectedTurn;
    protected bool IsSelected { get { return _selectedTurn != GameConstants.NULL_INT; }}
    protected int _year, _month, _todayTurn;

    void Start() {
        _selectedTurn = GameConstants.NULL_INT;
        if(_backMonthButton) {
            _backMonthButton.onClick.AddListener(GoBackMonth);
        }

        if(_nextMonthButton) {
            _nextMonthButton.onClick.AddListener(GoNextMonth);
        }

        if(_dayObjectList != null) {
            for(int i = 0; i < _dayObjectList.Count; i++) {
                if(_dayObjectList[i] != null) {
                    _dayObjectList[i].Ready(clickAction:DayClicked);
                }
            }
        }
    }

    public void Ready(TurnManager turnManager) {
        _turnManager = turnManager;
        if(_turnManager != null) {
            _myMatchDict = _turnManager.GetMyMatchDict();
        }
    }
    
    public void SetDataByTurn(int year, int turn) {
        DateObj date = new DateObj(year:year, turn:turn);
        _todayTurn = turn;
        SetData(year:year, month:date.Month);
        
    }

    public void SetData(int year, int month) {
        MatchInfo matchInfo = null;
        SetYear(year:year);
        SetMonth(month:month);

        if(_backMonthButton != null) { 
            _backMonthButton.interactable = !(_month == 1); 
        }
        if(_nextMonthButton != null) {
            _nextMonthButton.interactable = !(_month == 12); 
        }

        DateObj startDate = new DateObj(year: _year, month: _month, day: 1);
        DateObj endDate = new DateObj(year: _year, month: _month, isEndMonth:true);

        // Set Former Month
        int loop = endDate.Turn - startDate.Turn + 1;
        int dayName = (int)startDate.DayName;
        int dayObjectIndex = 0;
        int gap = dayName + 1;
        int formerTurn = startDate.Turn;

        // Set Former Month
        if(gap < 7) {
            formerTurn = startDate.Turn - gap;
            int formerDay = DateObj.GetDaysInMonth(
                year: (_month == 1) ? _year - 1 : _year,
                month: (_month == 1) ? 12 : _month - 1
            );
            formerDay -= gap - 1;

            for(int i = 0; i < gap; i++) {
                if(_dayObjectList[dayObjectIndex] != null) {
                    matchInfo = GetMatch(formerTurn);
                    _dayObjectList[dayObjectIndex].SetData(
                        turn        : formerTurn,
                        day         : formerDay,
                        isClickAble : false,
                        matchInfo   : matchInfo,
                        todayTurn   : _todayTurn
                    );
                }
                dayObjectIndex++;
                formerTurn++;
                formerDay++;
            }
        }

        // Set Current Month
        for (int i = 0; i < loop; i++) {
            if(_dayObjectList[dayObjectIndex] != null) {
                matchInfo = GetMatch(formerTurn);
                _dayObjectList[dayObjectIndex].SetData(
                    turn        : formerTurn,
                    day         : i + 1,
                    isClickAble : GetIsClickable(matchInfo),
                    matchInfo   : matchInfo,
                    todayTurn   : _todayTurn
                );
            }
            dayObjectIndex++;
            formerTurn++;
        }

        // Set Next Month
        int laterDay = 1;
        for(; dayObjectIndex < _dayObjectList.Count; dayObjectIndex++) {
            if(_dayObjectList[dayObjectIndex] != null) {
                matchInfo = GetMatch(formerTurn);
                _dayObjectList[dayObjectIndex].SetData(
                    turn        : formerTurn,
                    day         : laterDay,
                    isClickAble : false,
                    matchInfo   : matchInfo,
                    todayTurn   : _todayTurn
                );
            }
            formerTurn++;
            laterDay++;
        }

        CheckSelected();
    }

    protected virtual bool GetIsClickable(MatchInfo matchInfo) {
        return true;
    }

    void SetYear(int year) {
        _year = year;
        if(_yearText) {
            _yearText.text = _year.ToString();
        }
    }

    void SetMonth(int month) {
        _month = month;
        if(_monthText) {
            _monthText.text = _month.ToString();
        }
    }

    void GoBackMonth() {
        if(_month != 1) {
            SetData(year: _year, month: _month - 1);
        }
    }

    void GoNextMonth() {
        if(_month != 12) {
            SetData(year: _year, month: _month + 1);
        }
    }

    void DayClicked(DayLayout dayObject) {

        if(dayObject != null && dayObject.Turn != _selectedTurn) {
            _selectedTurn = dayObject.Turn;

        }
        else {
            _selectedTurn = GameConstants.NULL_INT;
        }

        DateObj date = IsSelected ? new DateObj(year:_year, turn:_selectedTurn) : null; 

        DayClickedAfter(date);
        CheckSelected();
    }

    protected virtual void DayClickedAfter(DateObj date) {}

    void CheckSelected() {
        if(_dayObjectList != null) {
            for(int i = 0; i < _dayObjectList.Count; i++) {
                if(_dayObjectList[i] != null) {
                    _dayObjectList[i].SetSelected(_selectedTurn == _dayObjectList[i].Turn);
                }
            }
        }
    }
}
