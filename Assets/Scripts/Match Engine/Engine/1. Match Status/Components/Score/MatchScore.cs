using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchScore {
    public List<int> ScoreListHome { get; private set; }
    public List<int> ScoreListAway { get; private set; }

    public int HitCountHome { get; set; }
    public int HitCountAway { get; set; } 

    public int ErrorCountHome { get; set; }
    public int ErrorCountAway { get; set; } 

    public int BBCountHome { get; set; }
    public int BBCountAway { get; set; } 

    public MatchScore() {
        ScoreListHome = new List<int>();
        ScoreListAway = new List<int>();

        HitCountHome = 0;
        HitCountAway = 0;

        ErrorCountHome = 0;
        ErrorCountAway = 0;

        BBCountHome = 0;
        BBCountAway = 0;
    }

    public void AddBBCount(bool isHomeAttack) {
        if(isHomeAttack) {
            BBCountHome++;
        }
        else {
            BBCountAway++;
        }
    }

    public void AddHitCount(bool isHomeAttack) {
        if(isHomeAttack) {
            HitCountHome++;
        }
        else {
            HitCountAway++; 
        }
    }

    List<int> GetScoreList(bool isHome) {
        return isHome ? ScoreListHome : ScoreListAway;
    }

    List<int> GetScoreList(InningOrder order) {
        return (order == InningOrder.PRE) ? ScoreListAway : ScoreListHome;
    }

    public void AddScore(InningInfo inningInfo, int score) {
        if(inningInfo != null) {
            List<int> dataList = GetScoreList(inningInfo.Order);

            if(dataList != null) {
                if (inningInfo.Inning > dataList.Count) {
                    dataList.Add(0);
                }
                dataList[inningInfo.Inning - 1] += score;
            }

        }
    }

    public MatchResultTypes GetResultOfHomeTeam { 
        get {
            int homeScore = GetScore(isHome:true), awayScore = GetScore(isHome:false);

            if(homeScore > awayScore) {
                return MatchResultTypes.WIN;
            }
            else if(homeScore < awayScore) {
                return MatchResultTypes.LOSE;
            }
            else {
                return MatchResultTypes.DRAW;
            }
        }
    }

    public int GetScore(bool isHome) {
        List<int> dataList = GetScoreList(isHome:isHome);
        int result = 0;
        if(dataList != null) {
            for(int i = 0; i < dataList.Count; i++) {
                result += dataList[i];
            }
        }
        return result;
    }
    
    public bool IsScoreDifferent {
        get { 
            return GetScore(isHome:true) != GetScore(isHome:false);
        }
    }

    public string ScoreString {
        get { return GetScore(isHome:true).ToString() + " : " + GetScore(isHome:false).ToString(); }
    }

}
