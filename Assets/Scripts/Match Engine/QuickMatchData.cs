using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickMatchData {
    // Date
    DateObj _date;
    public string Date { get { return _date.MMaDDString; } }
    public DaysEnum DayName { get { return _date.DayName; } }
    
    // Teams
    public Team HomeTeam {get; private set;}
    public string HomeName { get { return HomeTeam == null ? "" : HomeTeam.Name;} }
    public int HomeRank { get; private set; }

    public Team AwayTeam {get; private set;}
    public string AwayName { get { return AwayTeam == null ? "" : AwayTeam.Name;} }
    public int AwayRank { get; private set; }

    public PlayerStatusInMatch PlayerStatus { get; private set; }

    // Result
    public int ScoreHome { get; private set; }
    public int ScoreAway { get; private set; }
    public bool IsMatchFinished { get; private set; }
    public bool IsMatchRegistered {get; set;}

    
    public QuickMatchData(
        bool isMatchRegistered,
        int year,
        int turn, 
        Team homeTeam=null,
        Team awayTeam=null,
        int homeRank=0, 
        int awayRank=0, 
        PlayerStatusInMatch playerStatus = PlayerStatusInMatch.NONE
    ) {
        _date = new DateObj(year: year, turn: turn);

        HomeTeam = homeTeam;
        AwayTeam = awayTeam;
        HomeRank = homeRank;
        AwayRank = awayRank;
        PlayerStatus = playerStatus;

        IsMatchFinished = false;
        ScoreHome = 0;
        ScoreAway = 0;
        IsMatchRegistered = isMatchRegistered;
    }

    
    public void SetScore(int homeScore, int awayScore) {
        ScoreHome = homeScore;
        ScoreAway = awayScore;
        IsMatchFinished = true;
    }
}
