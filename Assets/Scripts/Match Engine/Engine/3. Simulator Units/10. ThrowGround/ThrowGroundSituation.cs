using System.Collections;
using System.Collections.Generic;

public class ThrowGroundSituation : MatchSituationBase {
    public DefenseCatchSituation DefenseSituation {get; private set;}
    public List<BatterPositionEnum> DefenseList {get; private set;}
    public List<bool> IsErrorList {get; private set;}
    public List<bool> IsCatchList {get; private set;}
    public RunningSituation Running {get; private set;}
    public List<int> BaseList {get; private set;}

    public ThrowGroundSituation(
        DefenseCatchSituation defenseSituation,
        List<BatterPositionEnum> defenseList,
        List<bool> isErrorList,
        List<bool> isCatchList,
        RunningSituation running,
        List<int> baseList
    ) {
        DefenseSituation = defenseSituation;
        DefenseList = defenseList;
        IsErrorList = isErrorList;
        IsCatchList = isCatchList;
        Running = running;
        BaseList = baseList;
    }

    public List<RunningStatus> GetDataList(bool isDead) {
        List<RunningStatus> result = new List<RunningStatus>();
        for(int i = 0; i < Running.Runners.Length; i++) {
            RunningStatus runner = Running.Runners[i];
            if(runner != null) {
                bool isCaught = false;
                int goal = runner.Goal;
                if(BaseList != null && IsCatchList != null) {
                    for(int j = 0; j < BaseList.Count; j++) {
                        if(BaseList[j] == goal && IsCatchList[j]) {
                            isCaught = true;
                            break;
                        }
                    }
                }

                if((isDead && isCaught) || (!isDead && !isCaught)) {
                    result.Add(runner);
                }
            }
        }
        return result;
    }
}
