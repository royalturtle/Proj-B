public class DefenseCatchSituation : MatchSituationBase {
    public BaseballResultTypes NewResult {get; private set;}
    public BaseballResultTypes OriginalResult {get; private set;}

    public bool IsError {get; private set;}
    public bool IsGood  {get; private set;}
    
    public BatterPositionEnum Position {get; private set;}
    public DefensePlayer Defense {get; private set;}

    public DefenseCatchSituation(
        BaseballResultTypes originalResult, 
        BaseballResultTypes newResult,
        BatterPositionEnum position,
        DefensePlayer defense,
        bool isError = false,
        bool isGood = false
    ) {
        OriginalResult = originalResult;
        NewResult = newResult;
        Position = position;
        Defense = defense;
        IsError = isError;
        IsGood = isGood;
    }
}
