public class OutBallSituation : MatchSituationBase {
    public BaseballResultTypes Result {get; private set;}

    public OutBallSituation(BaseballResultTypes result) {
        Result = result;
    }
}
