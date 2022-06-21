using System.Collections;
using System.Collections.Generic;
using SQLite4Unity3d;
using UnityEngine;

public class TupleLineupPitcher : DBBase {
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public int TeamId { get; set; }

    public int StartingOrder { get; set; }

    public static readonly int StartingsMax = 8;
    public int Starting1Id {get; set;}
    public int Starting2Id {get; set;}
    public int Starting3Id {get; set;}
    public int Starting4Id {get; set;}
    public int Starting5Id {get; set;}
    public int Starting6Id {get; set;}
    public int Starting7Id {get; set;}
    public int Starting8Id {get; set;}

    public static readonly int ReliefsMax = 20;
    public int Relief1Id {get; set;}
    public int Relief2Id {get; set;}
    public int Relief3Id {get; set;}
    public int Relief4Id {get; set;}
    public int Relief5Id {get; set;}
    public int Relief6Id {get; set;}
    public int Relief7Id {get; set;}
    public int Relief8Id {get; set;}
    public int Relief9Id {get; set;}
    public int Relief10Id {get; set;}
    public int Relief11Id {get; set;}
    public int Relief12Id {get; set;}
    public int Relief13Id {get; set;}
    public int Relief14Id {get; set;}
    public int Relief15Id {get; set;}
    public int Relief16Id {get; set;}
    public int Relief17Id {get; set;}
    public int Relief18Id {get; set;}
    public int Relief19Id {get; set;}
    public int Relief20Id {get; set;}
    
    public int SetupId  { get; set; }
    public int CloserId  { get; set; }

    public TupleLineupPitcher() {}

    public TupleLineupPitcher(TupleLineupPitcher tuple) {
        if(tuple != null) {
            Id = tuple.Id;
            TeamId = tuple.TeamId;
            StartingOrder = tuple.StartingOrder;
            SetupId = tuple.SetupId;
            CloserId = tuple.CloserId;

            List<int> startingList = tuple.StartingsList;
            if(startingList != null) {
                for(int i = 0; i < startingList.Count; i++) {
                    SetStartingId(index:i+1, id:startingList[i]);
                }
            }

            List<int> reliefList = tuple.ReliefsList;
            if(reliefList != null) {
                for(int i = 0; i < reliefList.Count; i++) {
                    SetReliefId(index:i+1, id:reliefList[i]);
                }
            }
        }
    }

    public int Count {
        get {
            return StartingsCount + ReliefsCount + (IsSetupExist ? 1 : 0) + (IsCloserExist ? 1 : 0);
        }
    }

    public int StartingsCount {
        get {
            int result = 0;
            for(int i = 1; GetStartingId(i) > 0; i++) {
                result++;
            }
            return result;
        }
    }

    public bool IsStartingFull {
        get {
            return StartingsCount >= StartingsMax;
        }
    }

    public int ReliefsCount {
        get {
            int result = 0;
            for(int i = 1; GetReliefId(i) > 0; i++) {
                result++;
            }
            return result;
        }
    }

    public bool IsReliefFull {
        get {
            return ReliefsCount >= ReliefsMax;
            
        }
    }

    public List<int> StartingsList {
        get {
            List<int> result = new List<int>();

            int index = 1;
            int startingId = GetStartingId(index);

            while(startingId != 0) {
                result.Add(startingId);
                startingId = GetStartingId(++index);
            }

            return result;
        }
    }

    public List<int> ReliefsList {
        get {
            List<int> result = new List<int>();

            int index = 1;
            int reliefId = GetReliefId(index);

            while(reliefId != 0) {
                result.Add(reliefId);
                reliefId = GetReliefId(++index);
            }

            return result;
        }
    }

    public bool IsSetupExist {
        get { return SetupId > 0;}
    }

    public bool IsCloserExist {
        get { return CloserId > 0;}
    }

    public int GetStartingId(int index) {
        switch(index) {
            case 1: return Starting1Id;
            case 2: return Starting2Id;
            case 3: return Starting3Id;
            case 4: return Starting4Id;
            case 5: return Starting5Id;
            case 6: return Starting6Id;
            case 7: return Starting7Id;
            case 8: return Starting8Id;
            default: return 0;
        }
    }

    public void SetStartingId(int index, int id) {
        switch(index) {
            case 1: Starting1Id = id; break;
            case 2: Starting2Id = id; break;
            case 3: Starting3Id = id; break;
            case 4: Starting4Id = id; break;
            case 5: Starting5Id = id; break;
            case 6: Starting6Id = id; break;
            case 7: Starting7Id = id; break;
            case 8: Starting8Id = id; break;
        }
    }
    public int GetReliefId(int index) {
        switch(index) {
            case 1: return Relief1Id;
            case 2: return Relief2Id;
            case 3: return Relief3Id;
            case 4: return Relief4Id;
            case 5: return Relief5Id;
            case 6: return Relief6Id;
            case 7: return Relief7Id;
            case 8: return Relief8Id;
            case 9: return Relief9Id;
            case 10: return Relief10Id;
            case 11: return Relief11Id;
            case 12: return Relief12Id;
            case 13: return Relief13Id;
            case 14: return Relief14Id;
            case 15: return Relief15Id;
            case 16: return Relief16Id;
            case 17: return Relief17Id;
            case 18: return Relief18Id;
            case 19: return Relief19Id;
            case 20: return Relief20Id;
            default: return 0;
        }
    }

    public void SetReliefId(int index, int id) {
        switch(index) {
            case 1:  Relief1Id = id; break;
            case 2:  Relief2Id = id; break;
            case 3:  Relief3Id = id; break;
            case 4:  Relief4Id = id; break;
            case 5:  Relief5Id = id; break;
            case 6:  Relief6Id = id; break;
            case 7:  Relief7Id = id; break;
            case 8:  Relief8Id = id; break;
            case 9:  Relief9Id = id; break;
            case 10: Relief10Id = id; break;
            case 11: Relief11Id = id; break;
            case 12: Relief12Id = id; break;
            case 13: Relief13Id = id; break;
            case 14: Relief14Id = id; break;
            case 15: Relief15Id = id; break;
            case 16: Relief16Id = id; break;
            case 17: Relief17Id = id; break;
            case 18: Relief18Id = id; break;
            case 19: Relief19Id = id; break;
            case 20: Relief20Id = id; break;
        }
    }

    bool DeleteInStarting(int index) {
        int search = GetStartingId(index);
        bool result = search > 0;
        while(search > 0) {
            search = GetStartingId(index + 1);
            SetStartingId(index:index++, id:search);
        }
        SetStartingId(index:index, id:0);
        return result;
    }

    bool DeleteInRelief(int index) {
        int search = GetReliefId(index);
        bool result = search > 0;
        while(search > 0) {
            search = GetReliefId(index + 1);
            SetReliefId(index:index++, id:search);
        }
        SetReliefId(index:index, id:0);
        return result;
    }

    public int SearchInStarting(int id) {
        int result = 0, index = 1;
        int search = GetStartingId(index);
        while(search > 0) {
            if(search == id) {
                result = index;
                break;
            }
            else {
                search = GetStartingId(++index);
            }
        }

        return result;
    }

    public int SearchInRelief(int id) {
        int result = 0, index = 1;
        int search = GetReliefId(index);
        while(search > 0) {
            if(search == id) {
                result = index;
                break;
            }
            else {
                search = GetReliefId(++index);
            }
        }

        return result;
    }

    public (PitcherPositionEnum, int) FindId(int id) {
        PitcherPositionEnum position = PitcherPositionEnum.GROUP2;
        int index = 1;

        // 선발
        int search = SearchInStarting(id);
        if(search > 0) {
            position = PitcherPositionEnum.STARTING;
            index = search;
        }

        // 중계
        if(position == PitcherPositionEnum.GROUP2) {
            search = SearchInRelief(id);
            if(search > 0) {
                position = PitcherPositionEnum.RELIEF;
                index = search;
            }
        }

        // 셋업
        if(SetupId == id) {
            position = PitcherPositionEnum.SETUP;
        }
        // 마무리
        else if(CloserId == id) {
            position = PitcherPositionEnum.CLOSER;
        }

        return (position, index);
    }

    public bool DeletePlayer(int id) {
        bool result = false;
        (PitcherPositionEnum position, int index) = FindId(id);

        switch(position) {
            case PitcherPositionEnum.STARTING:
                result = DeleteInStarting(index:index);
                break;
            case PitcherPositionEnum.RELIEF:
                result = DeleteInRelief(index:index);
                break;
            case PitcherPositionEnum.SETUP:
                SetupId = 0;
                result = true;
                break;
            case PitcherPositionEnum.CLOSER:
                CloserId = 0;
                result = true;
                break;
        }

        return result;
    }

    public void SetId(int id, PitcherPositionEnum position, int index = 0) {
        switch(position) {
            case PitcherPositionEnum.STARTING:
                SetStartingId(index:index, id:id);
                break;
            case PitcherPositionEnum.RELIEF:
                SetReliefId(index:index, id:id);
                break;
            case PitcherPositionEnum.SETUP:
                SetupId = id;
                break;
            case PitcherPositionEnum.CLOSER:
                CloserId = id;
                break;
        }
    }

    public bool ChangePosition(int id, PitcherPositionEnum position) {
        bool result = false;
        bool isDeleteComplete = DeletePlayer(id);

        if(isDeleteComplete) {
            result = AddPosition(id:id, position:position);
        }

        return result;
    }

    public bool AddPosition(int id, PitcherPositionEnum position) {
        bool result = false;
        switch(position) {
            case PitcherPositionEnum.STARTING:
                int startingCount = StartingsCount;
                if(startingCount < StartingsMax) {
                    SetStartingId(index:startingCount + 1, id: id);
                    result = true;
                }
                break;
            case PitcherPositionEnum.RELIEF:
                int reliefCount = ReliefsCount;
                if(reliefCount < ReliefsMax) {
                    SetReliefId(index:reliefCount + 1, id: id);
                    result = true;
                }
                break;
            case PitcherPositionEnum.SETUP:
                if(IsSetupEmpty()) {
                    SetupId = id;
                    result = true;
                }
                break;
            case PitcherPositionEnum.CLOSER:
                if(IsCloserEmpty()) {
                    CloserId = id;
                    result = true;
                }
                break;
        }

        return result;
    }

    bool IsSetupEmpty() {
        return SetupId == 0;
    }

    bool IsCloserEmpty() {
        return CloserId == 0;
    }
}
