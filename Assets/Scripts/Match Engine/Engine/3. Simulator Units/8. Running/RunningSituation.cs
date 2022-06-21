public class RunningSituation : MatchSituationBase {
    public RunningStatus[] Runners {get; private set;}
    public RunningSituation(RunningStatus[] runners) {
        Runners = runners;
    }

    public int CatchAbleIndex {
        get {
            int result = GameConstants.NULL_INT;
            for(int i = 0; i < Runners.Length; i++) {
                if(Runners[i] != null && !Runners[i].IsSafe) {
                    result = i;
                    break;
                }
            }
            return result;
        }
    }

    public bool IsFull {
        get {
            bool result = true;
            for(int i = 0; i < Runners.Length; i++) {
                if(Runners[i] == null) {
                    result = false;
                    break;
                }
            }
            return result;
        }
    }

    public bool IsFilled(int index) {
        bool result = false;
        if(Runners != null && Runners.Length > index && index >= 0) {
            result = Runners[index] != null;
        }
        return result;
    }
}
