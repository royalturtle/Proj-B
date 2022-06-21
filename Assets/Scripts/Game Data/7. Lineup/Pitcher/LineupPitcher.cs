using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineupPitcher {
    public List<Pitcher> StartingPitchers {get; private set;}
    public List<int> StartingOrders {get; private set;}
    public List<Pitcher> ReliefPitchers {get; private set;}
    public List<int> ReliefOrders {get; private set;}
    public Pitcher SetupPitcher {get; private set;}
    public Pitcher CloserPitcher {get; private set;}
    public List<Pitcher> Group2 {get; private set;}
    public int StartingOrder {get; private set;}

    public Pitcher CurrentPitcher {get; private set; }

    public List<Pitcher> PlayedList {get; private set;}
    public bool IsSaveSituation {get; private set;}

    public LineupPitcher(PitcherDataMediator gameData, TupleLineupPitcher tuple) {
        StartingOrder = tuple.StartingOrder;
        IsSaveSituation = false;    
        
        List<int> startingsId = tuple.StartingsList;
        List<int> reliefsId = tuple.ReliefsList;
        int setupId = tuple.SetupId;
        int closerId = tuple.CloserId;

        StartingPitchers = new List<Pitcher>();
        ReliefPitchers = new List<Pitcher>();

        StartingOrders = new List<int>();
        ReliefOrders = new List<int>();

        for(int i = 0; i < startingsId.Count; i++) {
            StartingPitchers.Add(gameData.Data[startingsId[i]]);
            StartingOrders.Add(i + 1);
        }

        CurrentPitcher = StartingPitchers[StartingOrder - 1];
        PlayedList = new List<Pitcher> { CurrentPitcher };

        for(int i = 0; i < reliefsId.Count; i++) {
            ReliefPitchers.Add(gameData.Data[reliefsId[i]]);
            ReliefOrders.Add(i + 1);
        }
        

        SetupPitcher = (setupId != 0) ? gameData.Data[setupId] : null;
        CloserPitcher = (closerId != 0) ? gameData.Data[closerId] : null;

        int teamId = tuple.TeamId;
        Group2 = gameData.GetInTeam(teamID:teamId, isOnlyGroup2:true);
    }

    public Pitcher GetPitcher(PitcherPositionEnum position, int order) {
        Pitcher result = null;

        if(position == PitcherPositionEnum.STARTING) {
            if(Utils.IsValidIndex(StartingPitchers, order - 1)) {
                result = StartingPitchers[order - 1];
            }
        }
        else if(position == PitcherPositionEnum.RELIEF) {
            if(Utils.IsValidIndex(ReliefPitchers, order - 1)) {
                result = ReliefPitchers[order - 1];
            }
        }
        else if(position == PitcherPositionEnum.SETUP) {
            result = SetupPitcher;
        }
        else if(position == PitcherPositionEnum.CLOSER) {
            result = CloserPitcher;
        }

        return result;
    }

    public int Group1Count { 
        get { 
            int result = 0;
            result += (StartingPitchers != null) ? StartingPitchers.Count : 0;
            result += (ReliefPitchers != null) ? ReliefPitchers.Count : 0;
            result += (SetupPitcher != null) ? 1 : 0;
            result += (CloserPitcher != null) ? 1 : 0;
            return result; 
        } 
    }

    public bool ChangePitcher(Pitcher pitcher) {
        bool result = !CurrentPitcher.IsEqual(pitcher);

        if(!result) { return result; }

        bool findResult = false;
        for(int i = 0; i < StartingPitchers.Count; i++) {
            if(CurrentPitcher.IsEqual(StartingPitchers[i])) {
                StartingPitchers.RemoveAt(i);
                StartingOrders.RemoveAt(i);
                findResult = true;
                break;
            }
        }

        if(!findResult) {
            for(int i = 0; i < ReliefPitchers.Count; i++) {
                if(CurrentPitcher.IsEqual(ReliefPitchers[i])) {
                    ReliefPitchers.RemoveAt(i);
                    ReliefOrders.RemoveAt(i);
                    findResult = true;
                    break;
                }
            }
        }

        if(!findResult && CurrentPitcher.IsEqual(SetupPitcher)) {
            SetupPitcher = null;
            findResult = true;
        }

        if(!findResult && CurrentPitcher.IsEqual(CloserPitcher)) {
            CloserPitcher = null;
            findResult = true;
        }

        CurrentPitcher = pitcher;

        return result;
    }
}
