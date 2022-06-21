using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningSimulator : SimulatorUnitBase {
    static readonly int BaseCount = 4;
    int BaseEnd { get { return BaseCount + 1; } }

    static readonly double SumProbability = 90.0;

    public RunningSituation Get(BaseMultipleStatus bases, BaseballResultTypes resultType) {
        RunningStatus[] runners = new RunningStatus[BaseCount];
        int runnerGoBase = 1;
        bool isAdditional = false;

        switch(resultType) {
            case BaseballResultTypes.HIT1:
                runnerGoBase = 1;
                isAdditional = false;
                break;
            case BaseballResultTypes.HIT1_LONG:
                runnerGoBase = 1;
                isAdditional = true;
                break;
            case BaseballResultTypes.HIT2:
                runnerGoBase = 2;
                isAdditional = false;
                break;
            case BaseballResultTypes.HIT2_LONG:
                runnerGoBase = 2;
                isAdditional = true;
                break;
            // case BaseballResultTypes.HIT3:
            default:
                runnerGoBase = 3;
                isAdditional = false;
                break;
        }

        int maxBase = BaseEnd;
        for(int i = BaseCount - 1; i > 0; i--) {
            BaseSingleStatus baseStatus = bases.GetSingleStatus(i);
            if(baseStatus != null) {
                int runnerGo = runnerGoBase;
                bool isSafe = true;
                double speed = baseStatus.Runner.Stats.Speed;
                double successProb = ArriveProbability(speed);
                double giveUpProb  = SumProbability - successProb;
                double randomValue = random.NextDouble();

                if(isAdditional) {
                    runnerGo += (randomValue <= successProb) ? 1 : 0;
                    isSafe = randomValue <= successProb + giveUpProb;
                }

                // Check Front
                int goingBase = i + runnerGo;
                if(goingBase >= maxBase) {
                    goingBase = maxBase - 1;
                    isSafe = true;
                }

                if(maxBase != BaseCount) {
                    maxBase = goingBase;
                }
                
                runners[i] = new RunningStatus(
                    runner : baseStatus,
                    start  : i,
                    goal   : goingBase,
                    isSafe : isSafe
                );
            }
        }
        runners[0] = new RunningStatus(
            runner : bases.GetSingleStatus(0),
            start  : 0,
            goal   : runnerGoBase,
            isSafe : true
        );

        return new RunningSituation(runners:runners);
    }

    public RunningSituation GetGround(BaseMultipleStatus bases, int outCount) {
        RunningStatus[] runners = new RunningStatus[BaseCount];

        if(outCount >= 2) {
            for(int i = 0; i < BaseCount; i++) {
                BaseSingleStatus baseStatus = bases.GetSingleStatus(i);
                if(baseStatus != null) {
                    runners[i] = new RunningStatus(
                        runner : baseStatus,
                        start  : i,
                        goal   : i + 1,
                        isSafe : false
                    );
                }
                
            }
        }
        else {            
            runners[0] = new RunningStatus(
                runner : bases.GetSingleStatus(0),
                start  : 0,
                goal   : 1,
                isSafe : false
            );

            bool isFormer = true;
            for(int i = 1; i < BaseCount; i++) {
                BaseSingleStatus baseStatus = bases.GetSingleStatus(i);
                if(baseStatus != null) {
                    runners[i] = new RunningStatus(
                        runner : baseStatus,
                        start  : i,
                        goal   : i + ((isFormer) ? 1 : 0),
                        isSafe : !isFormer
                    );
                }
                else {
                    isFormer = false;
                }
            }
        }

        return new RunningSituation(runners:runners);
    }

    double ArriveProbability(double speed) {
        return MathUtils.Linear(x:speed, a:0.006153846153846154, b:-0.2846153846153846);
    }

}