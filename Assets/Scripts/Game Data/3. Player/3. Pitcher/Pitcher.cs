public class Pitcher : PlayerBase {
    public TuplePitcherStats Stats;
    public TuplePitcherSeason Season;

    public Pitcher(
        TuplePlayerBase player,
        TuplePitcherStats stats,
        TuplePitcherSeason season=null ) {
            
        Base = player;
        Stats = stats;
        Season = season;
    }

    public override double GetStat(int statType) {
        PitcherStatType type = (PitcherStatType)statType;
        if(Stats != null && Season != null) {
            switch(type) {
                case PitcherStatType.G: return Season.G;
                case PitcherStatType.GS: return Season.GS;
                case PitcherStatType.CG: return Season.CG;
                case PitcherStatType.SHO: return Season.SHO;
                case PitcherStatType.OUT: return Season.OUT;
                case PitcherStatType.W: return Season.W;
                case PitcherStatType.L: return Season.L;
                case PitcherStatType.SV: return Season.SV;
                case PitcherStatType.R: return Season.R;
                case PitcherStatType.ER: return Season.ER;
                case PitcherStatType.BF: return Season.BF;
                case PitcherStatType.H: return Season.H;
                case PitcherStatType.H2: return Season.H2;
                case PitcherStatType.H3: return Season.H3;
                case PitcherStatType.HR: return Season.HR;
                case PitcherStatType.BB: return Season.BB;
                case PitcherStatType.IBB: return Season.IBB;
                case PitcherStatType.SO: return Season.SO;
                case PitcherStatType.HBP: return Season.HBP;
                case PitcherStatType.WP: return Season.WP;
                case PitcherStatType.GB: return Season.GB;
                case PitcherStatType.IP: return Season.IP;
                case PitcherStatType.ERA: return Season.ERA;
                case PitcherStatType.WHIP: return Season.WHIP;
                case PitcherStatType.FIP: return Season.FIP;
                case PitcherStatType.GBRate: return Season.GBRate;
                case PitcherStatType.Stamina: return Stats.Stamina;
                case PitcherStatType.Velocity: return Stats.Velocity;
                case PitcherStatType.Stuff: return Stats.Stuff;
                case PitcherStatType.KMov: return Stats.KMov;
                case PitcherStatType.GMov: return Stats.GMov;
                case PitcherStatType.Control: return Stats.Control;
                case PitcherStatType.Composure: return Stats.Composure;
                case PitcherStatType.LOpp: return Stats.LOpp;
                case PitcherStatType.ROpp: return Stats.ROpp;
            }
        }
        return 0.0;
    }
}
