public class ThrowHitSituation : MatchSituationBase {
    public DefenseCatchSituation DefenseSituation {get; private set;}
    public BatterPositionEnum Defense1 {get; private set;}
    public BatterPositionEnum Defense2 {get; private set;}
    public RunningSituation Running {get; private set;}
    public int ThrowBase {get; private set;}
    public bool IsOut {get; private set;}

    public ThrowHitSituation(
        DefenseCatchSituation defenseSituation,
        bool isOut,
        int throwBase,
        RunningSituation running,
        BatterPositionEnum defense1, 
        BatterPositionEnum defense2 = BatterPositionEnum.NONE
    ) {
        DefenseSituation = defenseSituation;
        Defense1 = defense1;
        Defense2 = defense2;
        Running = running;
        IsOut = isOut;
        ThrowBase = throwBase;
    }
}
