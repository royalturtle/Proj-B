using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulatorResult {
    public int CurrentOut { get; set; }
    public BaseMultipleStatus BaseStatus {get; private set;}

    public BaseballResultTypes ResultType {get; private set;}

    public List<BaseSingleStatus> ScoredPlayerList { get; private set; }
    public int ScoredAdded { get { return ScoredPlayerList.Count; } }

    public BatterPositionEnum  ErrorPosition {get; private set;}
    
    public bool IsNextBatter {get; private set;}
    
    public SimulatorResult(List<MatchSituationBase> situationList, MatchStatus matchStatus) {
        InitData();

        CurrentOut = matchStatus.Outs;

        MatchSituationBase firstSituation = situationList[0];
        if(firstSituation is WildPitchingSituation) {
            WildPitching(matchStatus);
            ResultType = BaseballResultTypes.WP;
        }
        else if(firstSituation is HBPSituation) {
            BB(matchStatus);
            ResultType = BaseballResultTypes.HBP;
        }
        else if(firstSituation is KBBSituation) {
            KBBSituation kbb = (KBBSituation)firstSituation;
            switch(kbb.Result) {
                case BaseballResultTypes.STRIKE_OUT:
                case BaseballResultTypes.STRIKE_OUT_SWING:
                    AddOut(matchStatus);
                    break;
                case BaseballResultTypes.BB:
                    BB(matchStatus);
                    break;
            }
            ResultType = kbb.Result;
        }
        else {
            DefenseCatchSituation defenseCatch = (DefenseCatchSituation)situationList[1];
            ResultType = defenseCatch.NewResult;

            switch(defenseCatch.NewResult) {
                case BaseballResultTypes.GROUND_BALL:
                    if(!defenseCatch.IsError) {
                        ThrowGroundSituation throwGround = (ThrowGroundSituation)situationList[2];
                        List<RunningStatus> liveList = throwGround.GetDataList(isDead:false);
                        List<RunningStatus> deadList = throwGround.GetDataList(isDead:true);

                        CurrentOut += matchStatus.Outs + deadList.Count;
                        if(CurrentOut < 3) {
                            for(int i = 0; i < liveList.Count; i++) {
                                int start = liveList[i].Start;
                                int goal  = liveList[i].Goal;

                                if(goal < 4) {
                                    BaseStatus.SetSingleStatus(
                                        index  : goal,
                                        status : (start == 0) ? GetBatterRunner(matchStatus) : liveList[i].Runner
                                    );
                                }
                                else {
                                    ScoredPlayerList.Add((start == 0) ? GetBatterRunner(matchStatus) : liveList[i].Runner);
                                }
                            }
                        }
                    }
                    else {
                        ErrorPosition = defenseCatch.Position;
                        Hit1(matchStatus, isError:true);
                    }
                    break;
                case BaseballResultTypes.FLY_INNER:
                    if(!defenseCatch.IsError) {
                        AddOut(matchStatus);
                    }
                    else {
                        ErrorPosition = defenseCatch.Position;
                        if(matchStatus.Outs >= 2) {
                            Hit1Long(matchStatus, isError:true);
                        }
                        else {
                            BB(matchStatus, isError:true);
                        }
                    }
                    break;
                case BaseballResultTypes.INFIELD_FLY:
                    AddOut(matchStatus);
                    break;
                case BaseballResultTypes.FLY_OUTSIDE:
                    if(!defenseCatch.IsError) {
                        AddOut(matchStatus);
                        if(situationList.Count >= 3) {
                            TagUpSituation tagUp = (TagUpSituation)situationList[2];
                            TagUp(matchStatus:matchStatus, tagUp:tagUp);
                        }
                    }
                    else {
                        ErrorPosition = defenseCatch.Position;
                        if(matchStatus.Outs <= 1) {
                            Hit1(matchStatus, isError:true);
                        }
                        else {
                            Hit1Long(matchStatus, isError:true);
                        }
                    }
                    break;
                case BaseballResultTypes.HIT1:
                    if(!defenseCatch.IsError) {
                        Hit1(matchStatus);
                    }
                    else {
                        ErrorPosition = defenseCatch.Position;
                        Hit2(matchStatus);
                    }
                    break;
                case BaseballResultTypes.HIT1_LONG:
                    if(!defenseCatch.IsError) {
                        if(situationList.Count >= 3) {
                            MatchSituationBase thirdSituation = situationList[2];
                            if(thirdSituation is ThrowHitSituation) {
                                CheckAdditionalRun(matchStatus:matchStatus,throwHit:(ThrowHitSituation)thirdSituation);
                            }
                            else {
                                CheckRun(matchStatus:matchStatus,running:(RunningSituation)thirdSituation);
                            }
                        }
                    }
                    else {
                        ErrorPosition = defenseCatch.Position;
                        Hit2(matchStatus);
                    }
                    break;
                case BaseballResultTypes.HIT2:
                    if(!defenseCatch.IsError) {
                        Hit2(matchStatus);
                    }
                    else {
                        Hit3(matchStatus);
                        ErrorPosition = defenseCatch.Position;
                    }
                    break;
                case BaseballResultTypes.HIT2_LONG:
                    if(!defenseCatch.IsError) {
                        if(situationList.Count >= 3) {
                            MatchSituationBase thirdSituation = situationList[2];
                            if(thirdSituation is ThrowHitSituation) {
                                CheckAdditionalRun(matchStatus:matchStatus,throwHit:(ThrowHitSituation)thirdSituation);
                            }
                            else {
                                CheckRun(matchStatus:matchStatus,running:(RunningSituation)thirdSituation);
                            }
                        }
                    }
                    else {
                        Hit3(matchStatus);
                        ErrorPosition = defenseCatch.Position;
                    }
                    break;
                case BaseballResultTypes.HIT3:
                    if(!defenseCatch.IsError) {
                        Hit3(matchStatus);
                    }
                    else {
                        Homerun(matchStatus);
                        ErrorPosition = defenseCatch.Position;
                    }
                    break;
                case BaseballResultTypes.HOMERUN:
                    Homerun(matchStatus);
                    break;
            }
        }
    }

    void InitData() {
        ScoredPlayerList = new List<BaseSingleStatus>();
        BaseStatus = new BaseMultipleStatus();
        IsNextBatter = false;      
        ErrorPosition = BatterPositionEnum.NONE;  
    }

    void AddOut(MatchStatus matchStatus) {
        BaseStatus = matchStatus.BaseStatus;
        CurrentOut = matchStatus.Outs + 1;
        IsNextBatter = true;
    }

    void WildPitching(MatchStatus matchStatus) {
        if(matchStatus.BaseStatus.IsB1Filled()) {
            BaseStatus.B2 = matchStatus.BaseStatus.B1;
        }
        if(matchStatus.BaseStatus.IsB2Filled()) {
            BaseStatus.B3 = matchStatus.BaseStatus.B2;
        }
        if(matchStatus.BaseStatus.IsB3Filled()) {
            ScoredPlayerList.Add(matchStatus.BaseStatus.B3);
        }
    }

    void BB(MatchStatus matchStatus, bool isError=false) {
        BaseStatus.B1 = GetBatterRunner(matchStatus, isByError:isError);
        bool isFormerComing = false;

        if(isError) {
            for(int i = 1; i < 4; i++) {
                BaseSingleStatus baseStatus = matchStatus.BaseStatus.GetSingleStatus(i);
                if(baseStatus != null) {
                    baseStatus.SetIsByError(true);
                }
            }
        }

        if (matchStatus.BaseStatus.IsB1Filled()) {
            BaseStatus.B2 = matchStatus.BaseStatus.B1;
            isFormerComing = true;
        }

        if(matchStatus.BaseStatus.IsB2Filled()) {
            if(isFormerComing) {
                BaseStatus.B3 = matchStatus.BaseStatus.B2;
                isFormerComing = true;
            }
            else {
                BaseStatus.B2 = matchStatus.BaseStatus.B2;
                isFormerComing = false;
            }
        }
        else {
            isFormerComing = false;
        }

        if(matchStatus.BaseStatus.IsB3Filled()) {
            if(isFormerComing) {
                ScoredPlayerList.Add(matchStatus.BaseStatus.B3);
            }
            else {
                BaseStatus.B3 = matchStatus.BaseStatus.B3;
            }
        }

        CurrentOut = matchStatus.Outs;
        IsNextBatter = true;
    }

    void Hit1(MatchStatus matchStatus, bool isError=false) {
        BaseStatus.B1 = GetBatterRunner(matchStatus, isByError:isError);
        if(isError) {
            for(int i = 1; i < 4; i++) {
                BaseSingleStatus baseStatus = matchStatus.BaseStatus.GetSingleStatus(i);
                if(baseStatus != null) {
                    baseStatus.SetIsByError(true);
                }
            }
        }

        if (matchStatus.BaseStatus.IsB1Filled()) {
            BaseStatus.B2 = matchStatus.BaseStatus.B1;
        }
        if (matchStatus.BaseStatus.IsB2Filled()) {
            BaseStatus.B3 = matchStatus.BaseStatus.B2;
        }
        if (matchStatus.BaseStatus.IsB3Filled()) {
            ScoredPlayerList.Add(matchStatus.BaseStatus.B3);
        }
    }

    void Hit1Long(MatchStatus matchStatus, bool isError=false) {
        BaseStatus.B1 = GetBatterRunner(matchStatus, isByError:isError);

        if(isError) {
            for(int i = 1; i < 4; i++) {
                BaseSingleStatus baseStatus = matchStatus.BaseStatus.GetSingleStatus(i);
                if(baseStatus != null) {
                    baseStatus.SetIsByError(true);
                }
            }
        }

        if (matchStatus.BaseStatus.IsB1Filled()) {
            BaseStatus.B3 = matchStatus.BaseStatus.B1;
        }
        if (matchStatus.BaseStatus.IsB2Filled()) {
            ScoredPlayerList.Add(matchStatus.BaseStatus.B2);
        }
        if (matchStatus.BaseStatus.IsB3Filled()) {
            ScoredPlayerList.Add(matchStatus.BaseStatus.B3);
        }
    }

    void Hit2(MatchStatus matchStatus) {
        BaseStatus.B2 = GetBatterRunner(matchStatus);
        if (matchStatus.BaseStatus.IsB1Filled()) {
            BaseStatus.B3 = matchStatus.BaseStatus.B1;
        }
        if (matchStatus.BaseStatus.IsB2Filled()) {
            ScoredPlayerList.Add(matchStatus.BaseStatus.B2);
        }
        if (matchStatus.BaseStatus.IsB3Filled()) {
            ScoredPlayerList.Add(matchStatus.BaseStatus.B3);
        }
    }

    void Hit2Long(MatchStatus matchStatus) {
        BaseStatus.B2 = GetBatterRunner(matchStatus);
        if (matchStatus.BaseStatus.IsB1Filled()) {
            ScoredPlayerList.Add(matchStatus.BaseStatus.B2);
        }
        if (matchStatus.BaseStatus.IsB2Filled()) {
            ScoredPlayerList.Add(matchStatus.BaseStatus.B2);
        }
        if (matchStatus.BaseStatus.IsB3Filled()) {
            ScoredPlayerList.Add(matchStatus.BaseStatus.B3);
        }
    }

    void Hit3(MatchStatus matchStatus) {
        BaseStatus.B3 = GetBatterRunner(matchStatus);
        if (matchStatus.BaseStatus.IsB1Filled()) {
            ScoredPlayerList.Add(matchStatus.BaseStatus.B1);
        }
        if (matchStatus.BaseStatus.IsB2Filled()) {
            ScoredPlayerList.Add(matchStatus.BaseStatus.B2);
        }
        if (matchStatus.BaseStatus.IsB3Filled()) {
            ScoredPlayerList.Add(matchStatus.BaseStatus.B3);
        }
        CurrentOut = matchStatus.Outs;
        IsNextBatter = true;
    }

    void TagUp(MatchStatus matchStatus, TagUpSituation tagUp) {
        int throwBase = tagUp.ThrowBase;
        if(throwBase != GameConstants.NULL_INT) {
            if (matchStatus.BaseStatus.IsB1Filled()) {
                BaseStatus.B1 = matchStatus.BaseStatus.B1;
            }
            if (matchStatus.BaseStatus.IsB3Filled()) { 
                if(tagUp.IsHome) {
                    BaseStatus.B3 = null;
                    if(throwBase == 4 && tagUp.IsOut) {
                        CurrentOut++;
                    }
                    else {
                        ScoredPlayerList.Add(GetBatterRunner(matchStatus));
                    }
                }   
                else {
                    BaseStatus.B3 = matchStatus.BaseStatus.B3;
                }           
            }
            if (matchStatus.BaseStatus.IsB2Filled()) {
                if(tagUp.IsB3) {
                    BaseStatus.B2 = null;
                    if(throwBase == 3 && tagUp.IsOut) {
                        CurrentOut++;
                    }
                    else {
                        BaseStatus.B3 = matchStatus.BaseStatus.B2;
                    }
                }
                else {
                    BaseStatus.B2 = matchStatus.BaseStatus.B2;
                }
            }

        }
        else {
            AddOut(matchStatus);    
        }
    }

    void Homerun(MatchStatus matchStatus) {
        ScoredPlayerList.Add(GetBatterRunner(matchStatus));
        if (matchStatus.BaseStatus.IsB1Filled()) {
            ScoredPlayerList.Add(matchStatus.BaseStatus.B1);
        }
        if (matchStatus.BaseStatus.IsB2Filled()) {
            ScoredPlayerList.Add(matchStatus.BaseStatus.B2);
        }
        if (matchStatus.BaseStatus.IsB3Filled()) {
            ScoredPlayerList.Add(matchStatus.BaseStatus.B3);
        }
        CurrentOut = matchStatus.Outs;
        IsNextBatter = true;
    }

    void CheckAdditionalRun(MatchStatus matchStatus, ThrowHitSituation throwHit) {
        RunningSituation running = throwHit.Running;
        List<RunningStatus> liveList = new List<RunningStatus>();
        CurrentOut = matchStatus.Outs;
        for(int i = 0; i < running.Runners.Length; i++) {
            RunningStatus runner = running.Runners[i];
            if(runner != null) {
                if(runner.Goal == throwHit.ThrowBase && throwHit.IsOut) {
                    CurrentOut++;
                }
                else {
                    liveList.Add(runner);
                }
            }
            
        }

        for(int i = 0; i < liveList.Count; i++) {
            int start = liveList[i].Start;
            int goal  = liveList[i].Goal;
            if(goal < 4) {
                BaseStatus.SetSingleStatus(
                    index  : goal,
                    status : (start == 0) ? GetBatterRunner(matchStatus) : liveList[i].Runner
                );
            }
            else {
                ScoredPlayerList.Add((start == 0) ? GetBatterRunner(matchStatus) : liveList[i].Runner);
            }
        }
    }

    void CheckRun(MatchStatus matchStatus, RunningSituation running) {
        for(int i = 0; i < running.Runners.Length; i++) {
            RunningStatus runner = running.Runners[i];
            if(runner != null) {
                int start = runner.Start;
                int goal  = runner.Goal;
                if(goal < 4) {
                    BaseStatus.SetSingleStatus(
                        index  : goal,
                        status : (start == 0) ? GetBatterRunner(matchStatus) : runner.Runner
                    );
                }
                else {
                    ScoredPlayerList.Add((start == 0) ? GetBatterRunner(matchStatus) : runner.Runner);
                }
            }
        }
    }

    BaseSingleStatus GetBatterRunner(MatchStatus matchStatus, bool isByError=false) {
        return new BaseSingleStatus(
            runner:matchStatus.CurrentBatter,
            pitcher: matchStatus.CurrentPitcher,
            isByError : isByError
        );
    }
}

