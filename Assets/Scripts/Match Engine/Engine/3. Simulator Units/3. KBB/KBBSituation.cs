public class KBBSituation : MatchSituationBase {
    public BaseballResultTypes Result {get; private set;}

    public KBBSituation(BaseballResultTypes result) {
        Result = result;
    }
}
