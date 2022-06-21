public class Batter : PlayerBase {
    public TupleBatterStats Stats;
    public TupleBatterSeason Season;

    public Batter(
        TuplePlayerBase player,
        TupleBatterStats stats,
        TupleBatterSeason season = null
    ) {
        Base = player;
        Stats = stats;
        Season = season;
    }

    public override double GetStat(int statType) {
        BatterStatType type = (BatterStatType)statType;
        if(Stats != null && Season != null) {
            switch(type) {
                case BatterStatType.G: return Season.G;
                case BatterStatType.PA: return Season.PA;
                case BatterStatType.AB: return Season.AB;
                case BatterStatType.R: return Season.R;
                case BatterStatType.H: return Season.H;
                case BatterStatType.H2: return Season.H2;
                case BatterStatType.H3: return Season.H3;
                case BatterStatType.HR: return Season.HR;
                case BatterStatType.RBI: return Season.RBI;
                case BatterStatType.SB: return Season.SB;
                case BatterStatType.CS: return Season.CS;
                case BatterStatType.BB: return Season.BB;
                case BatterStatType.SO: return Season.SO;
                case BatterStatType.GIDP: return Season.GIDP;
                case BatterStatType.HBP: return Season.HBP;
                case BatterStatType.SH: return Season.SH;
                case BatterStatType.SF: return Season.SF;
                case BatterStatType.IBB: return Season.IBB;
                case BatterStatType.E: return Season.E;
                case BatterStatType.AVG: return Season.AVG;
                case BatterStatType.OBP: return Season.OBP;
                case BatterStatType.SLG: return Season.SLG;
                case BatterStatType.OPS: return Season.OPS;
                case BatterStatType.Hit: return Stats.Hit;
                case BatterStatType.Eye: return Stats.Eye;
                case BatterStatType.Power: return Stats.Power;
                case BatterStatType.GapPower: return Stats.GapPower;
                case BatterStatType.AvoidK: return Stats.AvoidK;
                case BatterStatType.Speed: return Stats.Speed;
                case BatterStatType.DefenseC: return Stats.DefenseC;
                case BatterStatType.Defense1B: return Stats.Defense1B;
                case BatterStatType.Defense2B: return Stats.Defense2B;
                case BatterStatType.Defense3B: return Stats.Defense3B;
                case BatterStatType.DefenseSS: return Stats.DefenseSS;
                case BatterStatType.DefenseOF: return Stats.DefenseOF;
            }
        }
        return 0.0;
    }
}
