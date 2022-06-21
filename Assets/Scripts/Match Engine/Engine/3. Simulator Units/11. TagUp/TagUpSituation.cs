public class TagUpSituation : MatchSituationBase {
    public DefenseCatchSituation DefenseSituation {get; private set;}

    public BatterPositionEnum Defense1 {get; private set;}
    public BatterPositionEnum Defense2 {get; private set;}
    public bool IsOut {get; private set;}

    public bool IsB3 {get; private set;}
    public bool IsHome {get; private set;}

    public TagUpSituation(
        DefenseCatchSituation defenseSituation, 
        BatterPositionEnum position1,
        BatterPositionEnum position2,
        bool isB3,
        bool isHome,
        bool isOut
    ) {
        DefenseSituation = defenseSituation;
        Defense1 = position1;
        Defense2 = position2;
        IsOut = isOut;
        IsB3 = isB3;
        IsHome = isHome;
    }

    public int ThrowBase {
        get { return IsHome ? 4 : (IsB3 ? 3 : GameConstants.NULL_INT); }
    }
}
