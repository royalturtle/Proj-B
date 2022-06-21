public class DefensePlayer {
    public double Defense {
        get {
            double result = 0.0;
            if(_batter != null && _batter.Stats != null) {
                result = _batter.Stats.GetDefenseAbility(position:Position);
            }
            return result;
        }
    }

    public BatterPositionEnum Position  {get; private set;}
    Batter _batter;

    public DefensePlayer(Batter batter, BatterPositionEnum position) {
        _batter = batter;
        Position = position;
    }
}
