using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamDataInMatch {
    public Team RegisteredTeam {get; private set;}

    public LineupBatter BatterLineup {get; private set;}
    public LineupPitcher PitcherLineup {get; private set;}

    public List<Batter> PlayedBattersList {get; private set;}
    public List<Pitcher> PlayedPitchersList {get; private set;}

    public Dictionary<BatterPositionEnum, DefensePlayer> DefenseDict {get; private set;}

    public int CurrentBattingIndex { get; private set; }
    public Sprite TeamLogo {get; private set;}

    public TeamDataInMatch(Team team, LineupBatter batterLineup, LineupPitcher pitcherLineup, Sprite teamLogo) {
        RegisteredTeam = team;
        TeamLogo = teamLogo;

        BatterLineup = batterLineup;
        PitcherLineup = pitcherLineup;

        CurrentBattingIndex = 0;

        PlayedBattersList = new List<Batter>();
        PlayedPitchersList = new List<Pitcher>();

        for (int i = 0; i < 9; i++) {
            PlayedBattersList.Add(BatterLineup.Playings[i]);
            BatterLineup.Playings[i].Season.G++;
        }

        // 수비자 설정
        DefenseDict = batterLineup.GetDefenseDict();
    }

    public Batter CurrentBatter { get { return BatterLineup.Playings[CurrentBattingIndex]; } }
    public Pitcher CurrentPitcher { get { return PitcherLineup != null ? PitcherLineup.CurrentPitcher : null; } }
    
    public int StartPitcherId {
        get {
            return (PlayedPitchersList == null || PlayedPitchersList.Count < 1) ? GameConstants.NULL_INT : PlayedPitchersList[0].Base.ID;
        }
    }

    public void NextBatter() {
        if(CurrentBattingIndex < 8) {
            CurrentBattingIndex++;
        }
        else {
            CurrentBattingIndex = 0;
        }
    }
}
