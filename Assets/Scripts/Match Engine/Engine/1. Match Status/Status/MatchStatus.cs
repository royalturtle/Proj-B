using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchStatus {
    // About Teams
    public TeamDataInMatch HomeTeamInfo {get; private set;} 
    public TeamDataInMatch AwayTeamInfo {get; private set;}

    // Current Inning
    public InningInfo InningInfo {get; private set;}
    public int Outs { get { return InningInfo != null ? InningInfo.Outs : 0; } }
    public bool IsMatchEnd { get; private set; }
    
    // Score
    public MatchScore Score { get; private set; }

    // Base
    public BaseMultipleStatus BaseStatus {get; private set;}

    public PlayerStatusInMatch PlayerStatus {get; private set;}

    public MatchStatus(
        NationTypes leagueType,
        MatchTypes matchType,
        TeamDataInMatch homeTeamInfo,
        TeamDataInMatch awayTeamInfo,
        PlayerStatusInMatch playerStatus
    ) {
        PlayerStatus = playerStatus;
        // About Teams
        HomeTeamInfo = homeTeamInfo;
        AwayTeamInfo = awayTeamInfo;

        // Current Inning
        InningInfo = new InningInfo(matchType:matchType, nationType:leagueType);

        IsMatchEnd = false;

        // Score
        Score = new MatchScore();

        // Base
        BaseStatus = new BaseMultipleStatus();
    }

    public MatchResultTypes PlayerResult {
        get {
            MatchResultTypes result = MatchResultTypes.NONE;

            if(PlayerStatus != PlayerStatusInMatch.NONE && Score != null) {
                result = Score.GetResultOfHomeTeam;
                
                if(PlayerStatus == PlayerStatusInMatch.AWAY) {
                    if(result == MatchResultTypes.WIN) {
                        result = MatchResultTypes.LOSE;
                    }
                    else if(result == MatchResultTypes.LOSE) {
                        result = MatchResultTypes.WIN;
                    }
                }
            }
            return result;
        }
    }

    public void Update(SimulatorResult newStatus) {
        // 점수 더하기
        Score.AddScore(inningInfo:InningInfo, score: newStatus.ScoredAdded);
        BaseStatus = newStatus.BaseStatus;
        int outChange = newStatus.CurrentOut - Outs;
        InningInfo.SetOut(newStatus.CurrentOut);

        PrintStatus(status:newStatus);

        // 시즌 기록 변경
        switch(newStatus.ResultType) {
            case BaseballResultTypes.STRIKE_OUT:
            case BaseballResultTypes.STRIKE_OUT_SWING:
                CurrentPitcher.Season.BF++;
                CurrentPitcher.Season.OUT++;
                CurrentPitcher.Season.SO++;

                CurrentBatter.Season.PA++;
                CurrentBatter.Season.SO++;
                break;

            case BaseballResultTypes.FLY_INNER:
            case BaseballResultTypes.INFIELD_FLY:
            case BaseballResultTypes.GROUND_BALL:
                CurrentPitcher.Season.BF++;
                CurrentPitcher.Season.OUT++;

                CurrentBatter.Season.PA++;
                if(outChange >= 2) {
                    CurrentBatter.Season.GIDP++;
                }
                break;
            case BaseballResultTypes.FLY_OUTSIDE:
                CurrentPitcher.Season.BF++;            
                CurrentPitcher.Season.OUT++;

                CurrentBatter.Season.PA++;
                if(newStatus.ScoredAdded > 0) {
                    CurrentBatter.Season.SF++;
                }
                break;

            case BaseballResultTypes.BB:
                CurrentPitcher.Season.BF++;
                CurrentPitcher.Season.BB++;

                CurrentBatter.Season.PA++;
                CurrentBatter.Season.BB++;
                break;
            case BaseballResultTypes.IBB:
                CurrentPitcher.Season.BF++;
                CurrentPitcher.Season.IBB++;

                CurrentBatter.Season.PA++;
                CurrentBatter.Season.IBB++;
                break;
            case BaseballResultTypes.HBP:
                CurrentPitcher.Season.BF++;
                CurrentPitcher.Season.HBP++;

                CurrentBatter.Season.PA++;
                CurrentBatter.Season.HBP++;
                break;
            case BaseballResultTypes.WP:
                CurrentPitcher.Season.WP++;
                break;
            case BaseballResultTypes.HIT1:
                CurrentPitcher.Season.BF++;
                CurrentPitcher.Season.H++;

                CurrentBatter.Season.PA++;
                CurrentBatter.Season.H++;
                break;

            case BaseballResultTypes.HIT1_LONG:
                CurrentPitcher.Season.BF++;
                CurrentPitcher.Season.H++;

                CurrentBatter.Season.PA++;
                CurrentBatter.Season.H++;
                break;

            case BaseballResultTypes.HIT2:
                CurrentPitcher.Season.BF++;
                CurrentPitcher.Season.H++;
                CurrentPitcher.Season.H2++;

                CurrentBatter.Season.PA++;
                CurrentBatter.Season.H++;
                CurrentBatter.Season.H2++;
                break;

            case BaseballResultTypes.HIT2_LONG:
                CurrentPitcher.Season.BF++;
                CurrentPitcher.Season.H++;
                CurrentPitcher.Season.H2++;

                CurrentBatter.Season.PA++;
                CurrentBatter.Season.H++;
                CurrentBatter.Season.H2++;
                break;

            case BaseballResultTypes.HIT3:
                CurrentPitcher.Season.BF++;
                CurrentPitcher.Season.H++;
                CurrentPitcher.Season.H3++;

                CurrentBatter.Season.PA++;
                CurrentBatter.Season.H++;
                CurrentBatter.Season.H3++;
                break;
            case BaseballResultTypes.HOMERUN:
                CurrentPitcher.Season.BF++;
                CurrentPitcher.Season.H++;
                CurrentPitcher.Season.HR++;

                CurrentBatter.Season.PA++;
                CurrentBatter.Season.H++;
                CurrentBatter.Season.HR++;
                break;
        }

        // 득점 관련 기록
        for(int i = 0; i < newStatus.ScoredPlayerList.Count; i++) {
            BaseSingleStatus baseStatus = newStatus.ScoredPlayerList[i];

            // 타자 - 득점
            baseStatus.Runner.Season.R++;

            // 투수 - 자책
            if(!baseStatus.IsByError) {
                baseStatus.ResponsivePitcher.Season.ER++;
            }

            // 투수 - 실점
            baseStatus.ResponsivePitcher.Season.R++;
        }
        // 타자 - 타점
        CurrentBatter.Season.RBI += newStatus.ScoredAdded;

        // 다음 타자 이동 여부
        if (newStatus.IsNextBatter) {
            if (InningInfo.IsOrderPre) { 
                AwayTeamInfo.NextBatter();
            }
            else { 
                HomeTeamInfo.NextBatter(); 
            }
        }

        // Update the Score board
        switch (newStatus.ResultType) {
            case BaseballResultTypes.BB:
                Score.AddBBCount(isHomeAttack:InningInfo.IsOrderPost);
                break;
            case BaseballResultTypes.HIT1:
            case BaseballResultTypes.HIT1_LONG:
            case BaseballResultTypes.HIT2:
            case BaseballResultTypes.HIT2_LONG:
            case BaseballResultTypes.HIT3:
            case BaseballResultTypes.HOMERUN:
                Score.AddHitCount(isHomeAttack:InningInfo.IsOrderPost);
                break;
        }

        if (Outs >= 3) {
            EndInning();
        }
    }

    void PrintStatus(SimulatorResult status) {
        Debug.Log("[ " + InningInfo.ToString() + " : " +
            " / Score : " + Score.ScoreString + 
            " / Pitcher : " + CurrentPitcher.Base.Name + 
            " / Batter : " + CurrentBatter.Base.Name + 
            " / Result : " + status.ResultType + 
            " / Outs : " + status.CurrentOut.ToString() + 
            " / Base : " + status.BaseStatus.ToString() + "]");
    }

    #region Ending
    void EndInning() {
        BaseStatus = new BaseMultipleStatus();
        
        if(InningInfo != null) {
            if(InningInfo.IsInningEnd(homeResult:Score.GetResultOfHomeTeam)) {
                InningInfo.SetOut(0);
                EndMatch();
            }
            else {
                InningInfo.SetOut(0);
                InningInfo.Next();
                Score.AddScore(inningInfo:InningInfo, score:0);
            }
        }
    }

    void EndMatch() {
        ApplyPitcherRecord(HomeTeamInfo.PitcherLineup);
        ApplyPitcherRecord(AwayTeamInfo.PitcherLineup);

        IsMatchEnd = true;
        if(InningInfo != null) {
            InningInfo.SetOut(0);
        }
    }

    void ApplyPitcherRecord(LineupPitcher lineup) {
        if(lineup != null && lineup.PlayedList != null) {
            for(int i = 0; i < lineup.PlayedList.Count; i++) {
                if(lineup.PlayedList[i] != null && lineup.PlayedList[i].Season != null) {
                    TuplePitcherSeason season = lineup.PlayedList[i].Season;
                    if(i == 0) { 
                        season.GS++; 

                        if(lineup.PlayedList.Count <= 1) {
                            season.CG++; 
                        }
                    }
                    
                    season.G++;

                    if(i >= lineup.PlayedList.Count - 1 && lineup.IsSaveSituation) {
                        season.SV++;
                    }
                }
            }
        }
    }

    #endregion

    #region Property
    public Pitcher CurrentPitcher {
        get { return (InningInfo.IsOrderPre) ? HomeTeamInfo.CurrentPitcher : AwayTeamInfo.CurrentPitcher; }
    }

    public Dictionary<BatterPositionEnum, DefensePlayer> CurrentDefenseDict {
        get { return (InningInfo.IsOrderPre) ? HomeTeamInfo.DefenseDict : AwayTeamInfo.DefenseDict; }
    }
    
    public Batter CurrentBatter {
        get { return (InningInfo.IsOrderPre) ? AwayTeamInfo.CurrentBatter : HomeTeamInfo.CurrentBatter; }
    }

    public List<Batter> CurrentBattersList {
        get { return (InningInfo.IsOrderPre) ? AwayTeamInfo.BatterLineup.Playings : HomeTeamInfo.BatterLineup.Playings; }
    }

    public int BatterOrder {
        get { return (InningInfo.IsOrderPre) ? AwayTeamInfo.CurrentBattingIndex + 1 : HomeTeamInfo.CurrentBattingIndex + 1; }
    }

    public List<Pitcher> UpdateNeedPitchers {
        get {
            List<Pitcher> result = HomeTeamInfo.PlayedPitchersList;
            result.AddRange(AwayTeamInfo.PlayedPitchersList);
            return result;
        }
    }
    
    public List<Batter> UpdateNeedBatters {
        get {
            List<Batter> result = HomeTeamInfo.PlayedBattersList;
            result.AddRange(AwayTeamInfo.PlayedBattersList);
            return result;
        }
    }
    #endregion
}


