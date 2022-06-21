using UnityEngine;

public class TeamSeason {
    public Team _team {get; private set;} 
    public TupleLeagueSeason _season  {get; private set;} 
    public TeamSeason(Team team, TupleLeagueSeason season) {
        _team = team;
        _season = season;
    }

    public double GetStat(TeamStatType type) {
        if(_season != null) {
            switch(type) {
                case TeamStatType.WinCount: return _season.WinCount;
                case TeamStatType.DrawCount: return _season.DrawCount;
                case TeamStatType.LoseCount: return _season.LoseCount;
                case TeamStatType.GameCount: return _season.GameCount;
                case TeamStatType.WinRate: return _season.WinRate;
                case TeamStatType.WinDiff: return _season.WinDiff;
                case TeamStatType.BatterAB: return _season.BatterAB;
                case TeamStatType.BatterH: return _season.BatterH;
                case TeamStatType.BatterBB: return _season.BatterBB;
                case TeamStatType.BatterHBP: return _season.BatterHBP;
                case TeamStatType.BatterSF: return _season.BatterSF;
                case TeamStatType.Batter2B: return _season.Batter2B;
                case TeamStatType.Batter3B: return _season.Batter3B;
                case TeamStatType.BatterHR: return _season.BatterHR;
                case TeamStatType.BatterSB: return _season.BatterSB;
                case TeamStatType.BatterBA: return _season.BatterBA;
                case TeamStatType.BatterOBP: return _season.BatterOBP;
                case TeamStatType.BatterSLG: return _season.BatterSLG;
                case TeamStatType.BatterOPS: return _season.BatterOPS;
                case TeamStatType.PitcherIP: return _season.PitcherIP;
                case TeamStatType.PitcherBatters: return _season.PitcherBatters;
                case TeamStatType.PitcherOuts: return _season.PitcherOuts;
                case TeamStatType.PitcherSO: return _season.PitcherSO;
                case TeamStatType.PitcherH: return _season.PitcherH;
                case TeamStatType.PitcherBB: return _season.PitcherBB;
                case TeamStatType.PitcherHR: return _season.PitcherHR;
                case TeamStatType.PitcherER: return _season.PitcherER;
                case TeamStatType.PitcherIBB: return _season.PitcherIBB;
                case TeamStatType.PitcherHBP: return _season.PitcherHBP;
                case TeamStatType.PitcherERA: return _season.PitcherERA;
                case TeamStatType.PitcherFIP: return _season.PitcherFIP;
                case TeamStatType.PitcherWHIP: return _season.PitcherWHIP;
                case TeamStatType.PitcherBBRate: return _season.PitcherBBRate;
                case TeamStatType.PitcherSORate: return _season.PitcherSORate;
            }
        }
        return 0.0;
    }

    public static bool IsBiggerStatGood(TeamStatType type) {
        switch(type) {
            case TeamStatType.LoseCount:
            case TeamStatType.WinDiff:
            case TeamStatType.PitcherERA:
            case TeamStatType.PitcherFIP:
            case TeamStatType.PitcherWHIP:
            case TeamStatType.PitcherBBRate:
                return false;
        }
        
        return true;
    }
}
