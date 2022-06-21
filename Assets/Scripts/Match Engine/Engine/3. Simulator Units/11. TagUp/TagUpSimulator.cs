using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagUpSimulator : SimulatorUnitBase {
    static readonly double RelayThreshold = 75.0;
    static readonly double DefensePenalty = 10.0;

    public TagUpSituation Get(DefenseCatchSituation defenseSituation, Dictionary<BatterPositionEnum, DefensePlayer> defenses, BaseMultipleStatus bases) {
        double ofDefense = defenseSituation.Defense.Defense;
        BatterPositionEnum position = defenseSituation.Position;
        BatterPositionEnum position2 = BatterPositionEnum.NONE;

        double speed = 0.0;

        bool isB3 = false, isHome = false, isOut = false;

        if(bases.IsB2Filled()) {
            speed = bases.B2.Runner.Stats.Speed;
            isB3 = IsTagUpAble(speed:speed, defense:ofDefense, position:position, isHome:false);
        }

        if(bases.IsB3Filled()) {
            speed = bases.B3.Runner.Stats.Speed;
            isHome = IsTagUpAble(speed:speed, defense:ofDefense, position:position, isHome:true);
        }

        isB3 = !(bases.IsB3Filled() && !isHome);

        if(isHome || isB3) {
            double sumDefense = ofDefense;
            if(IsRelay(defense:ofDefense, ofPosition:position, isHome:isHome)) {
                position2 = GetRelayPosition(ofPosition:position, isHome:isHome);
                sumDefense = ((ofDefense + defenses[position2].Defense) / 2.0) * 0.8;
            }

            isOut = (speed >= sumDefense) ? false : (random.NextDouble() <= ((sumDefense-speed) * 0.01));
        }
        return new TagUpSituation(
            defenseSituation:defenseSituation, 
            position1:position,
            position2:position2,
            isOut:isOut,
            isB3:isB3,
            isHome:isHome
        );
    }

    bool IsTagUpAble(double speed, double defense, BatterPositionEnum position, bool isHome) {
        return speed - (defense - GetDistancePenalty(position:position, isHome:isHome) - DefensePenalty) >= 0;
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