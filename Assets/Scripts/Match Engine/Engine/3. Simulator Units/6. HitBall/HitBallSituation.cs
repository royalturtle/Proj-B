public class HitBallSituation : MatchSituationBase {
    public BaseballResultTypes Result {get; private set;}

    public HitBallSituation(BaseballResultTypes result) {
        Result = result;
    }
}
