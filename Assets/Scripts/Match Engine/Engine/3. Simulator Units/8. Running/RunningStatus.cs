using UnityEngine;

public class RunningStatus {
    public BaseSingleStatus Runner {get; private set;}
    public int Start {get; private set;}
    public int Goal {get; private set;}
    public bool IsSafe {get; private set;}

    public RunningStatus(BaseSingleStatus runner, int start, int goal, bool isSafe) {
        Runner = runner;
        Start = start;
        Goal = goal;
        IsSafe = isSafe;
    }
}
