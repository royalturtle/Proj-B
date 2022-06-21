public class PlayerBase {
    public TuplePlayerBase Base;

    public virtual double GetStat(int statType) {
        return 0.0;
    }

    public bool IsEqual(PlayerBase player) {
        return player != null && player.Base != null && Base != null && Base.ID == player.Base.ID;
    }
}
