using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineupBatter {
    public List<Batter> Playings {get; private set;}
    public List<Batter> Candidates {get; private set;}
    public List<Batter> Group2 {get; private set;}
    public List<BatterPositionEnum> DefenseList {get; private set;}

    public List<Batter> PlayedList {get; private set;}

    public LineupBatter(GameDataMediator gameData, TupleLineupBatter tuple) {
        Playings = new List<Batter>();
        Candidates = new List<Batter>();

        int teamId = tuple.TeamId;

        PlayedList = new List<Batter>();
        // 선발
        for(int i = 1; i <= 9; i++) {
            Playings.Add(gameData.Batters.Data[tuple.GetIdOfStartingOrder(i)]);
            PlayedList.Add(Playings[i - 1]);
        }

        // 수비자
        DefenseList = tuple.GetPositionList();

        // 후보
        List<int> candidateIdList = tuple.GetSubPlayersList();
        for(int i = 0; i < candidateIdList.Count; i++) {
            Candidates.Add(gameData.Batters.Data[candidateIdList[i]]);
        }
        
        Group2 = gameData.Batters.GetInTeam(teamID:teamId, isOnlyGroup2:true);
        for(int i = 0; i < Group2.Count; i++) {
            // Positions.Add(BatterPositionEnum.GROUP2);
        }
    }

    public Batter GetBatter(BatterPositionEnum position, int order) {
        Batter result = null;

        if(BatterPositionEnum.C <= position && position <= BatterPositionEnum.DH) {
            if(Utils.IsValidIndex(Playings, order - 1)) {
                result = Playings[order - 1];
            }
        }
        else if(position == BatterPositionEnum.CANDIDATE) {
            if(Utils.IsValidIndex(Candidates, order - 1)) {
                result = Candidates[order - 1];
            }
        }

        return result;
    }

    public bool PutSub(int subIndex, int order) {
        bool result = false;
        if(Utils.IsValidIndex(Candidates, subIndex) && Utils.IsValidIndex(Playings, order)) {
            PlayedList.Add(Candidates[subIndex]);
            Playings[order] = Candidates[subIndex];
            Candidates.RemoveAt(subIndex);
            result = true;
        }
        return result;
    }

    public bool ChangePosition(BatterPositionEnum position1, BatterPositionEnum position2) {
        bool result = false;
        if(position1 != position2 && IsPositionGroup1(position1) && IsPositionGroup1(position2) && DefenseList != null) {
            int index1 = GameConstants.NULL_INT, index2 = GameConstants.NULL_INT;

            for(int i = 0; i < DefenseList.Count && !result; i++) {
                if(position1 == DefenseList[i]) {
                    index1 = i;
                }
                if(position2 == DefenseList[i]) {
                    index2 = i;
                }

                if(index1 != GameConstants.NULL_INT && index2 != GameConstants.NULL_INT) {
                    result = true;
                }
            }

            if(result) {
                ChangePosition(index1, index2);
            }
        }
        
        return result;
    }

    bool IsPositionGroup1(BatterPositionEnum position) {
        return BatterPositionEnum.C <= position && position <= BatterPositionEnum.DH;
    }

    public bool ChangePosition(int index1, int index2) {
        bool result = false;
        if(Utils.IsValidIndex(DefenseList, index1) && Utils.IsValidIndex(DefenseList, index2)) {
            BatterPositionEnum temp = DefenseList[index1];
            DefenseList[index1] = DefenseList[index2];
            DefenseList[index2] = temp;
            result = true;
        }
        return result;
    }

    public int Group1Count { 
        get { 
            int result = 0;
            result += (Playings != null) ? Playings.Count : 0;
            result += (Candidates != null) ? Candidates.Count : 0;
            return result; 
        } 
    }

    public Dictionary<BatterPositionEnum, DefensePlayer> GetDefenseDict() {
        Dictionary<BatterPositionEnum, DefensePlayer> result = new Dictionary<BatterPositionEnum, DefensePlayer>();

        if(Playings != null && DefenseList != null) {
            for(int i = 0; i < Playings.Count && i < DefenseList.Count; i++) {
                result.Add(
                    DefenseList[i],
                    new DefensePlayer(
                        batter   : Playings[i],
                        position : DefenseList[i]
                    )
                );
            }
        }

        return result;
    }

}
