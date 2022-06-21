using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowHitSimulator : SimulatorUnitBase {
    static readonly double RelayThreshold = 75.0;

    public ThrowHitSituation Get(DefenseCatchSituation defenseSituation, RunningSituation running, Dictionary<BatterPositionEnum, DefensePlayer> defenses) {
        ThrowHitSituation result = null;
        
        if(Utils.NotNull(defenseSituation, running, defenses)) {
            int throwBase = running.CatchAbleIndex;
            if(throwBase != GameConstants.NULL_INT) {
                RunningStatus runningStatus = running.Runners[throwBase]; 
                double speed = runningStatus.Runner.Runner.Stats.Speed;
                double ofDefense = defenseSituation.Defense.Defense;
                BatterPositionEnum position = defenseSituation.Position;

                bool isHome = runningStatus.Goal == 4;
                double distancePenalty = GetDistancePenalty(position:position, isHome:isHome);

                BatterPositionEnum position2 = BatterPositionEnum.NONE;
                double sumDefense = ofDefense;
                if(IsRelay(ofDefense, position, isHome)) {
                    position2 = GetRelayPosition(ofPosition:position, isHome:isHome);
                    sumDefense = ((ofDefense + defenses[position2].Defense) / 2.0) * 0.8;
                }

                bool isOut = (speed >= sumDefense) ? false : (random.NextDouble() <= ((sumDefense-speed) * 0.01));
                result = new ThrowHitSituation(
                    defenseSituation:defenseSituation,
                    isOut : isOut,
                    running : running,
                    throwBase : throwBase,
                    defense1:position,
                    defense2:position2
                );
            }
        }
        return result;
    }

    double GetDistancePenalty(BatterPositionEnum position, bool isHome) {
        if(isHome) {
            return 5.0;
        }
        else {
            switch(position) {
                case BatterPositionEnum.LF: return -5.0;
                case BatterPositionEnum.CF: return 0.0;
                case BatterPositionEnum.RF: return 5.0;
                default: return 0.0;
            }
        }
    }
    
    bool IsRelay(double defense, BatterPositionEnum ofPosition, bool isHome) {
        return defense < RelayThreshold && !isHome && ofPosition == BatterPositionEnum.LF;
    }

    BatterPositionEnum GetRelayPosition(BatterPositionEnum ofPosition, bool isHome) {
        switch(ofPosition) {
            case BatterPositionEnum.RF:
                return BatterPositionEnum.B2;
            default:
                return BatterPositionEnum.SS;
        }
    }
}