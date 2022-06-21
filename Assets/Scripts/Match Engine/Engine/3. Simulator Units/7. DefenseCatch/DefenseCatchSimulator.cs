using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseCatchSimulator : SimulatorUnitBase {
    public DefenseCatchSituation Get(Batter batter, Dictionary<BatterPositionEnum, DefensePlayer> defenses, double direction, int outCount, BaseballResultTypes result, BaseMultipleStatus bases) {
        DefenseCatchSituation returnResult = null;

        BatterPositionEnum position = GetPosition(direction:direction, result:result);
        if(result != BaseballResultTypes.HOMERUN) {
            BaseballResultTypes newResult = BaseballResultTypes.NONE;
            DefensePlayer defense = defenses[position];

            if(result == BaseballResultTypes.HIT1) {
                if(IsAdditional(defense)) {
                    result = BaseballResultTypes.HIT1_LONG;
                }
            }
            else if(result == BaseballResultTypes.HIT2) {
                if(IsAdditional(defense)) {
                    result = BaseballResultTypes.HIT2_LONG;
                }
            }

            bool isError = false, isGood = false;
            if(result == BaseballResultTypes.FLY_INNER && outCount < 2 && bases.IsB1Filled() && bases.IsB2Filled()) {
                newResult = BaseballResultTypes.INFIELD_FLY;
            }
            else {
                double error = GetError(defense:defense);
                double good  = GetGood(defense:defense);
                double randomValue = random.NextDouble();

                if(randomValue <= error) {
                    // newResult = ConvertError(result, outCount:outCount);
                    newResult = result;
                    isError = true;
                }
                else if(randomValue <= error + good) {
                    newResult = ConvertGood(result, outCount:outCount);
                    isGood = true;
                }
                else {
                    newResult = result;
                }
            }

            returnResult = new DefenseCatchSituation(
                originalResult:result, 
                newResult:newResult, 
                position:position, 
                defense:defense,
                isError:isError,
                isGood:isGood
            );
        }
        else {
            returnResult = new DefenseCatchSituation(
                originalResult:BaseballResultTypes.HOMERUN, 
                newResult:BaseballResultTypes.HOMERUN, 
                position:position, 
                defense:null
            );
        }
        
        return returnResult;
    }

    bool IsAdditional(DefensePlayer defense) {
        bool result = false;
        if(defense != null) {
            result = random.NextDouble() <= MathUtils.Linear(x:defense.Defense, a:-0.004615384615384616, b:1.0615384615384615);
        }

        return result;
    }

    BatterPositionEnum GetPosition(double direction, BaseballResultTypes result) {
        BatterPositionEnum position = BatterPositionEnum.B1;
        switch(result) {
            case BaseballResultTypes.FLY_INNER:
            case BaseballResultTypes.GROUND_BALL:
                if(direction <= 0.2) {
                    position = BatterPositionEnum.B3;
                }
                else if(direction <= 0.5) {
                    position = BatterPositionEnum.SS;
                }
                else if(direction <= 0.8) {
                    position = BatterPositionEnum.B2;
                }
                break;
            default:
                if(direction <= 0.3) {
                    position = BatterPositionEnum.LF;
                }
                else if(direction <= 0.7) {
                    position = BatterPositionEnum.CF;
                }
                else {
                    position = BatterPositionEnum.RF;
                }
                break;
        }
        return position;
    }

    double GetError(DefensePlayer defense) {
        return MathUtils.Exponential(x:(int)defense.Defense, a:0.3615, b:0.9447);
    }

    double GetGood(DefensePlayer defense) {
        return MathUtils.Exponential(x:(int)defense.Defense, a:0.00019329527596312376, b: 1.0471813819686953);
    }

    /*
    BaseballResultTypes ConvertError(BaseballResultTypes former, int outCount) {
        BaseballResultTypes result = former;
        switch(former) {
            case BaseballResultTypes.HIT1:
            case BaseballResultTypes.HIT1_LONG:
                result = BaseballResultTypes.HIT2;
                break;
            case BaseballResultTypes.HIT2:
            case BaseballResultTypes.HIT2_LONG:
                result = BaseballResultTypes.HIT3;
                break;
            case BaseballResultTypes.HIT3:
                result = BaseballResultTypes.HOMERUN;
                break;
            case BaseballResultTypes.GROUND_BALL:
                result = BaseballResultTypes.HIT1;
                break;
            case BaseballResultTypes.FLY_INNER:
                result = (outCount >= 2) ? BaseballResultTypes.HIT1_LONG : BaseballResultTypes.FLY_INNER_ERROR;
                break;
            case BaseballResultTypes.FLY_OUTSIDE:
                result = (outCount >= 2) ? BaseballResultTypes.HIT1_LONG : BaseballResultTypes.HIT1;
                break;
        }
        return result;
    }
    */

    BaseballResultTypes ConvertGood(BaseballResultTypes former, int outCount) {
        BaseballResultTypes result = former;
        switch(former) {
            case BaseballResultTypes.HIT1:
            case BaseballResultTypes.HIT1_LONG:
                result = BaseballResultTypes.FLY_OUTSIDE;
                break;
            case BaseballResultTypes.HIT2:
            case BaseballResultTypes.HIT2_LONG:
                result = BaseballResultTypes.HIT1_LONG;
                break;
            case BaseballResultTypes.HIT3:
                result = BaseballResultTypes.HIT2_LONG;
                break;
            case BaseballResultTypes.GROUND_BALL:
                result = BaseballResultTypes.GROUND_BALL;
                break;
            case BaseballResultTypes.FLY_INNER:
                result = BaseballResultTypes.FLY_INNER;
                break;
            case BaseballResultTypes.FLY_OUTSIDE:
                result = BaseballResultTypes.FLY_OUTSIDE;
                break;
        }
        return result;
    }
}