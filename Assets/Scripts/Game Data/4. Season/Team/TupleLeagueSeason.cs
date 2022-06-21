using System;
using SQLite4Unity3d;

public class TupleLeagueSeason : DBBase {
    [PrimaryKey, AutoIncrement]
    public int ID { get; set; }
    public int LeagueID { get; set; }
    public int Year { get; set; }
    public int TeamID { get; set; }
    public int Rank { get; set; }
    public int WinCount { get; set; }
    public int DrawCount { get; set; }
    public int LoseCount { get; set; }

    public int GameCount { get { return WinCount + DrawCount + LoseCount; } }
    public double WinRate { get {
        return (WinCount + LoseCount == 0) ? 0 : (1.0 * WinCount) / (WinCount + LoseCount);
    } }
    public double WinDiff {get; set;}

    public int BatterAB { get; set; }
    public int BatterH { get; set; }
    public int BatterBB { get; set; }
    public int BatterHBP { get; set; }
    public int BatterSF { get; set; }
    public int Batter2B { get; set; }
    public int Batter3B { get; set; }
    public int BatterHR { get; set; }
    public int BatterSB { get; set; }

    public double BatterBA { get { return (BatterAB == 0) ? 0 : 1.0 * BatterH / BatterAB; } }
    public double BatterOBP { get { return (BatterAB + BatterBB + BatterHBP + BatterSB == 0) ? 0 : 1.0 * (BatterH + BatterBB + BatterHBP) / (BatterAB + BatterBB + BatterHBP + BatterSB); } }
    public double BatterSLG { get { return (BatterAB == 0) ? 0 : 1.0 * (BatterH + Batter2B  + Batter3B * 2 + BatterHR * 3) / BatterAB;  } }
    public double BatterOPS { get { return BatterOBP + BatterSLG; } }

    public double PitcherIP { get { return (PitcherOuts / 3) + (0.1 * (PitcherOuts % 3));; } }
    public int PitcherBatters { get; set; }
    public int PitcherOuts { get; set; }
    public int PitcherSO { get; set; }
    public int PitcherH { get; set; }
    public int PitcherBB { get; set; }
    public int PitcherHR { get; set; }
    public int PitcherER { get; set; }
    public int PitcherIBB { get; set; }
    public int PitcherHBP { get; set; }

    public double PitcherERA { get { return (PitcherIP == 0) ? 0 : 9.0 * PitcherER / PitcherIP; } }
    public double PitcherFIP { get { return (PitcherIP == 0) ? 0 : (13.0 * PitcherHR + 3 * (PitcherBB - PitcherIBB + PitcherHBP) - 2 * PitcherSO) / PitcherIP + GameConstants.BASEBALL_FIP_C; } }
    public double PitcherWHIP { get { return (PitcherIP == 0) ? 0 : (PitcherBB + PitcherH) / PitcherIP; } }
    public double PitcherBBRate { get { return (PitcherBatters == 0) ? 0 : 1.0 * PitcherBB / PitcherBatters; } }
    public double PitcherSORate { get { return (PitcherBatters == 0) ? 0 : 1.0 * PitcherSO / PitcherBatters; } }
}
