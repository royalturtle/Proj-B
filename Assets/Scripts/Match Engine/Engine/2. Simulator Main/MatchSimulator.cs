using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchSimulator {
    public MatchStatus Status {get; private set;}
    public TupleMatch MatchTuple { get; private set; }
    public PlayerStatusInMatch PlayerStatus { get; private set; }
    public List<MatchSituationBase> SituationsList {get; private set;}

    // Simulator Units
    WildPitchingSimulator _WildPitchingSimulator = new WildPitchingSimulator();
    HBPSimulator _HBPSimulator = new HBPSimulator();
    KBBSimulator _KBBSimulator = new KBBSimulator();
    HitSimulator _HitSimulator = new HitSimulator();
    OutBallSimulator _OutBallSimulator = new OutBallSimulator();
    HitBallSimulator _HitBallSimulator = new HitBallSimulator();
    DefenseCatchSimulator _DefenseCatchSimulator = new DefenseCatchSimulator();
    RunningSimulator _RunningSimulator = new RunningSimulator();
    ThrowHitSimulator _ThrowHitSimulator = new ThrowHitSimulator();
    ThrowGroundSimulator _ThrowGroundSimulator = new ThrowGroundSimulator();
    TagUpSimulator _TagUpSimulator = new TagUpSimulator();

    public MatchSimulator(
        NationTypes leagueType,
        TupleMatch matchData,
        PlayerStatusInMatch playerStatus,
        TeamDataInMatch homeTeamInfo,
        TeamDataInMatch awayTeamInfo) {
        // Match Tuple
        MatchTuple = matchData;

        // Match Status
        Status = new MatchStatus(
            leagueType   : leagueType,
            matchType    : matchData.MatchType,
            homeTeamInfo : homeTeamInfo,
            awayTeamInfo : awayTeamInfo,
            playerStatus : playerStatus
        );

        // Player Status
        PlayerStatus = playerStatus;

        // Situations List
        SituationsList = new List<MatchSituationBase>();
    }

    public void Progress() {
        SituationsList = new List<MatchSituationBase>();

        Pitcher pitcher = Status.CurrentPitcher;
        Batter  batter  = Status.CurrentBatter;
        BaseMultipleStatus baseStatus = Status.BaseStatus;

        // 폭투 시뮬레이터
        bool isWP = !baseStatus.IsEmpty && _WildPitchingSimulator.Get(pitcher:pitcher);
        if(isWP) {
            SituationsList.Add(new WildPitchingSituation());
        }
        else {
            // 사구 시뮬레이터
            bool isHBP = _HBPSimulator.Get(pitcher:pitcher);
            if(isHBP) {
                SituationsList.Add(new HBPSituation());
            }
            else {
                // 삼진, 볼넷 시뮬레이터
                KBBSituation kbb = _KBBSimulator.Get(pitcher:pitcher, batter:batter);
                if(kbb.Result != BaseballResultTypes.NONE) {
                    SituationsList.Add(kbb);
                }
                else {
                    // 안타 시뮬레이터
                    bool isHit = _HitSimulator.Get(pitcher:pitcher, batter:batter);
                    double direction = _HitBallSimulator.GetDirection(batter:batter);
                    BaseballResultTypes result = BaseballResultTypes.NONE;

                    // 안타 타구 시뮬레이터
                    if(isHit) {
                        HitBallSituation hitSituation = _HitBallSimulator.Get(pitcher:pitcher, batter:batter);
                        result = hitSituation.Result;
                        SituationsList.Add(hitSituation);
                    }
                    // 아웃 타구 시뮬레이터
                    else {
                        OutBallSituation outSituation = _OutBallSimulator.Get(pitcher:pitcher, batter:batter);
                        result = outSituation.Result;
                        SituationsList.Add(outSituation);
                    }
                    
                    // 수비(캐치) 시뮬레이터
                    DefenseCatchSituation defenseSituation = _DefenseCatchSimulator.Get(
                        batter    : batter, 
                        defenses  : Status.CurrentDefenseDict, 
                        direction : direction,
                        outCount  : Status.Outs,
                        result    : result,
                        bases     : Status.BaseStatus
                    );
                    SituationsList.Add(defenseSituation);

                    if(!defenseSituation.IsError) {
                        BaseballResultTypes defenseType = defenseSituation.NewResult;

                        switch(defenseType) {
                            case BaseballResultTypes.NONE:
                            case BaseballResultTypes.STRIKE_OUT:
                            case BaseballResultTypes.STRIKE_OUT_SWING:
                            case BaseballResultTypes.FLY_INNER:
                            case BaseballResultTypes.INFIELD_FLY:
                            // case BaseballResultTypes.FLY_INNER_ERROR:
                            // case BaseballResultTypes.ONLY_BATTER_ALIVE:
                            case BaseballResultTypes.BB:
                            case BaseballResultTypes.IBB:
                            case BaseballResultTypes.HBP:
                            case BaseballResultTypes.WP:
                            case BaseballResultTypes.HOMERUN:
                            case BaseballResultTypes.HIT1:
                            case BaseballResultTypes.HIT2:
                            case BaseballResultTypes.HIT3:
                                break;
                            case BaseballResultTypes.FLY_OUTSIDE:
                                if(Status.Outs <= 1 && (Status.BaseStatus.IsB2Filled() || Status.BaseStatus.IsB3Filled())) {
                                    // 태그업 시뮬레이터
                                    TagUpSituation tagUpSituation = _TagUpSimulator.Get(
                                        defenseSituation : defenseSituation,
                                        defenses         : Status.CurrentDefenseDict,
                                        bases            : Status.BaseStatus
                                    );
                                    SituationsList.Add(tagUpSituation);
                                }
                                break;
                            case BaseballResultTypes.GROUND_BALL:
                                // 송구(땅볼) 시뮬레이터
                                RunningSituation runningGroundSituation = _RunningSimulator.GetGround(
                                    bases    : Status.BaseStatus, 
                                    outCount : Status.Outs
                                );

                                ThrowGroundSituation throwGroundSituation = _ThrowGroundSimulator.Get(
                                    defenseSituation : defenseSituation,
                                    defenses         : Status.CurrentDefenseDict,
                                    runningSituation : runningGroundSituation,
                                    outCount         : Status.Outs
                                );
                                SituationsList.Add(throwGroundSituation);
                                break;
                            case BaseballResultTypes.HIT1_LONG:
                            case BaseballResultTypes.HIT2_LONG:
                                // 주루 시뮬레이터
                                RunningSituation runningSituation = _RunningSimulator.Get(
                                    bases      : Status.BaseStatus, 
                                    resultType : defenseType
                                );

                                // 송구(안타) 시뮬레이터
                                ThrowHitSituation throwHitSituation = _ThrowHitSimulator.Get(
                                    defenseSituation : defenseSituation,
                                    running          : runningSituation,
                                    defenses         : Status.CurrentDefenseDict
                                );

                                if(throwHitSituation == null) {
                                    SituationsList.Add(runningSituation);
                                }
                                else {
                                    SituationsList.Add(throwHitSituation);
                                }
                                break;
                        }
                    }
                }
            }
        }

        /*
        for(int i = 0; i < SituationsList.Count; i++) {
            Debug.Log(SituationsList[i].GetType().Name);
            Debug.Log(SituationsList[i].ToString());
        }*/
    }

    public SimulatorResult GetResult() {
        return new SimulatorResult(
            situationList : SituationsList,
            matchStatus   : Status
        );
    }

    public bool UpdateResult(SimulatorResult result) {
        Status.Update(result);
        return Status.IsMatchEnd;
    }

    public TupleMatch GetFinishedMatch() {
        MatchTuple.IsFinished = 1;
        MatchTuple.HomeScore = Status.Score.GetScore(isHome:true);
        MatchTuple.AwayScore = Status.Score.GetScore(isHome:false);

        return MatchTuple;
    }
}
