using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowGroundSimulator : SimulatorUnitBase {
    static readonly double SecondOutWeight = 10.0;
    public ThrowGroundSituation Get(DefenseCatchSituation defenseSituation, Dictionary<BatterPositionEnum, DefensePlayer> defenses, RunningSituation runningSituation, int outCount) {
        ThrowGroundSituation result = null;
        if(Utils.NotNull(defenseSituation, defenses, runningSituation)) {
            (List<BatterPositionEnum> defenseList, List<int> baseList) = GetDefenseList(
                runningSituation:runningSituation,
                position:defenseSituation.Position,
                outCount:outCount
            );

            List<bool> IsCatchList = new List<bool>();
            List<bool> IsErrorList = new List<bool>();

            // bool isError = GetError(defenses[defenseList[0]]);
            if(defenseList.Count >= 2) {
                (List<bool> errorList, bool isCatch) = GetDefenseSuccess(defenses[defenseList[0]], defenses[defenseList[1]]);

                IsErrorList.AddRange(errorList);
                IsCatchList.Add(isCatch);

                if(defenseList.Count >= 3) {
                    (errorList, isCatch) = GetDefenseSuccess(defenses[defenseList[1]], defenses[defenseList[2]]);

                    IsErrorList.AddRange(errorList);

                    double sumDefense = 0.0;
                    for(int i = 0; i < defenseList.Count; i++) {
                        sumDefense += defenses[defenseList[i]].Defense;
                    }
                    sumDefense /= 3.0;

                    double speed = runningSituation.Runners[baseList[baseList.Count - 1]].Runner.Runner.Stats.Speed;

                    IsCatchList.Add(isCatch && (sumDefense + SecondOutWeight - speed >= 0));
                    
                }
            }
            else {
                IsCatchList.Add(true);
                IsErrorList.Add(false);
            }

            result = new ThrowGroundSituation(
                defenseSituation:defenseSituation,
                defenseList : defenseList,
                isErrorList : IsErrorList,
                isCatchList : IsCatchList,
                running : runningSituation,
                baseList : baseList
            );
        }
        return result;
    }

    (List<bool>, bool) GetDefenseSuccess(DefensePlayer defense1, DefensePlayer defense2) {
        List<bool> errorList = new List<bool>();
        bool isCatch = false;

        errorList.Add(GetError(defense1));
        if(!errorList[0]) {
            errorList.Add(GetError(defense2));
            if(!errorList[0]) {
                isCatch = true;
            }
        }

        return (errorList, isCatch);
    }

    (List<BatterPositionEnum>, List<int>) GetDefenseList(RunningSituation runningSituation, BatterPositionEnum position, int outCount) {
        List<BatterPositionEnum> result = new List<BatterPositionEnum>();
        List<int> baseResult = new List<int>();
        result.Add(position);

        if(outCount <= 1 && runningSituation.IsFull) {
            result.Add(BatterPositionEnum.C);
            result.Add(BatterPositionEnum.B1);
            baseResult.Add(4);
        }
        else if(outCount <= 1 && !runningSituation.IsFull && runningSituation.IsFilled(1)) {
            if(position == BatterPositionEnum.B1 || position == BatterPositionEnum.B2) {
                result.Add(BatterPositionEnum.SS);
            }
            else {
                result.Add(BatterPositionEnum.B2);
            }
            result.Add(BatterPositionEnum.B1);

            baseResult.Add(2);
        }
        else {
            if(position != BatterPositionEnum.B1) {
                result.Add(BatterPositionEnum.B1);
            }
        }
        baseResult.Add(1);
        return (result, baseResult);
    }

    bool GetError(DefensePlayer defense) {
        return random.NextDouble() <= MathUtils.Exponential(x:(int)defense.Defense, a:0.3615, b:0.9447);
    }
}