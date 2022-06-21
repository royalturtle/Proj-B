using SQLite4Unity3d;
using System.Collections;
using System.Collections.Generic;

public class TupleLineupBatter : DBBase {
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public int TeamId { get; set; }
    public string Name { get; set;}
    public int Order {get; set;}
    public int A1Id {get; set;}
    public int A2Id {get; set;}
    public int A3Id {get; set;}
    public int A4Id {get; set;}
    public int A5Id {get; set;}
    public int A6Id {get; set;}
    public int A7Id {get; set;}
    public int A8Id {get; set;}
    public int A9Id {get; set;}

    public int OrderC {get; set;}
    public int OrderB1 {get; set;}
    public int OrderB2 {get; set;}
    public int OrderB3 {get; set;}
    public int OrderSS {get; set;}
    public int OrderLF {get; set;}
    public int OrderCF {get; set;}
    public int OrderRF {get; set;}
    public int OrderDH {get; set;}

    public static readonly int SubCountMax = 20;
    public int B1Id {get; set;}
    public int B2Id {get; set;}
    public int B3Id {get; set;}
    public int B4Id {get; set;}
    public int B5Id {get; set;}
    public int B6Id {get; set;}
    public int B7Id {get; set;}
    public int B8Id {get; set;}
    public int B9Id {get; set;}
    public int B10Id {get; set;}
    public int B11Id {get; set;}
    public int B12Id {get; set;}
    public int B13Id {get; set;}
    public int B14Id {get; set;}
    public int B15Id {get; set;}
    public int B16Id {get; set;}
    public int B17Id {get; set;}
    public int B18Id {get; set;}
    public int B19Id {get; set;}
    public int B20Id {get; set;}

    public int RunnerId {get; set;}
    public int LeftPitcherHitterId {get; set;}
    public int RightPitcherHitterDHId {get; set;}

    public TupleLineupBatter() {}

    public TupleLineupBatter(int teamId, string name) {
        TeamId = teamId;
        Name = name;
    }

    public TupleLineupBatter(TupleLineupBatter tuple) {
        if(tuple != null) {
            Id = tuple.Id;
            TeamId = tuple.TeamId;
            Name = tuple.Name;
            Order = tuple.Order;

            for(int i = 1; i <= 9; i++) {
                SetIdOfOrder(id:GetIdOfStartingOrder(i), order:i);
                BatterPositionEnum position = (BatterPositionEnum)i;
                SetOrderOfPosition(order: GetOrderOfPosition(position:position), position:position);
            }

            RunnerId = tuple.RunnerId;
            LeftPitcherHitterId = tuple.LeftPitcherHitterId;
            RightPitcherHitterDHId = tuple.RightPitcherHitterDHId;
        }
    }

    public int GetIdOfStartingOrder(int order) {
        switch(order) {
            case 1: return A1Id;
            case 2: return A2Id;
            case 3: return A3Id;
            case 4: return A4Id;
            case 5: return A5Id;
            case 6: return A6Id;
            case 7: return A7Id;
            case 8: return A8Id;
            case 9: return A9Id;
            default : return 0;
        }
    }

    public int FindOrderById(int id) {
        for(int i = 1; i <= 9; i++) {
            if(id == GetIdOfStartingOrder(i)) {
                return i;
            }
        }
        return 0;
    }

    public int GetOrderOfPosition(BatterPositionEnum position) {
        switch(position) {
            case BatterPositionEnum.C: return OrderC;
            case BatterPositionEnum.B1: return OrderB1;
            case BatterPositionEnum.B2: return OrderB2;
            case BatterPositionEnum.B3: return OrderB3;
            case BatterPositionEnum.SS: return OrderSS;
            case BatterPositionEnum.LF: return OrderLF;
            case BatterPositionEnum.CF: return OrderCF;
            case BatterPositionEnum.RF: return OrderRF;
            case BatterPositionEnum.DH: return OrderDH;
            default : return 0;
        }
    }

    public List<BatterPositionEnum> GetPositionList() {
        BatterPositionEnum[] result = new BatterPositionEnum[9];
        result[OrderC-1] = BatterPositionEnum.C;
        result[OrderB1-1] = BatterPositionEnum.B1;
        result[OrderB2-1] = BatterPositionEnum.B2;
        result[OrderB3-1] = BatterPositionEnum.B3;
        result[OrderSS-1] = BatterPositionEnum.SS;
        result[OrderLF-1] = BatterPositionEnum.LF;
        result[OrderCF-1] = BatterPositionEnum.CF;
        result[OrderRF-1] = BatterPositionEnum.RF;
        result[OrderDH-1] = BatterPositionEnum.DH;
        return new List<BatterPositionEnum>(result);
    }

    public Dictionary<BatterPositionEnum, int> GetPositionDict() {
        Dictionary<BatterPositionEnum, int> result = new Dictionary<BatterPositionEnum, int>();

        result[BatterPositionEnum.C] = OrderC-1;
        result[BatterPositionEnum.B1] = OrderB1-1;
        result[BatterPositionEnum.B2] = OrderB2-1;
        result[BatterPositionEnum.B3] = OrderB3-1;
        result[BatterPositionEnum.SS] = OrderSS-1;
        result[BatterPositionEnum.LF] = OrderLF-1;
        result[BatterPositionEnum.CF] = OrderCF-1;
        result[BatterPositionEnum.RF] = OrderRF-1;
        result[BatterPositionEnum.DH] = OrderDH-1;

        return result;
    }

    public int GetIdOfPosition(BatterPositionEnum position) {
        switch(position) {
            case BatterPositionEnum.C: return GetIdOfStartingOrder(OrderC);
            case BatterPositionEnum.B1: return GetIdOfStartingOrder(OrderB1);
            case BatterPositionEnum.B2: return GetIdOfStartingOrder(OrderB2);
            case BatterPositionEnum.B3: return GetIdOfStartingOrder(OrderB3);
            case BatterPositionEnum.SS: return GetIdOfStartingOrder(OrderSS);
            case BatterPositionEnum.LF: return GetIdOfStartingOrder(OrderLF);
            case BatterPositionEnum.CF: return GetIdOfStartingOrder(OrderCF);
            case BatterPositionEnum.RF: return GetIdOfStartingOrder(OrderRF);
            case BatterPositionEnum.DH: return GetIdOfStartingOrder(OrderDH);
            default : return 0;
        }
    }

    public BatterPositionEnum GetPositionOfId(int id) {
        BatterPositionEnum result = BatterPositionEnum.NONE;
        int endPosition = (int)BatterPositionEnum.DH;
        
        for(int i = (int)BatterPositionEnum.C; i <= endPosition; i++) {
            result = (BatterPositionEnum)i;
            if(id == GetIdOfPosition(result)) {
                return result;
            }
        }

        return (FindSub(id:id) != GameConstants.NULL_INT) ? BatterPositionEnum.CANDIDATE : BatterPositionEnum.GROUP2;
    }

    public void SetIdOfOrder(int id, int order) {
        switch(order) {
            case 1: A1Id = id; break;
            case 2: A2Id = id; break;
            case 3: A3Id = id; break;
            case 4: A4Id = id; break;
            case 5: A5Id = id; break;
            case 6: A6Id = id; break;
            case 7: A7Id = id; break;
            case 8: A8Id = id; break;
            case 9: A9Id = id; break;
            default : break;
        }
    }

    public void SetOrderOfPosition(int order, BatterPositionEnum position) {
        switch(position) {
            case BatterPositionEnum.C: OrderC = order; break;
            case BatterPositionEnum.B1: OrderB1 = order; break;
            case BatterPositionEnum.B2: OrderB2 = order; break;
            case BatterPositionEnum.B3: OrderB3 = order; break;
            case BatterPositionEnum.SS: OrderSS = order; break;
            case BatterPositionEnum.LF: OrderLF = order; break;
            case BatterPositionEnum.CF: OrderCF = order; break;
            case BatterPositionEnum.RF: OrderRF = order; break;
            case BatterPositionEnum.DH: OrderDH = order; break;
            default: break;
        }
    }

    public int GetSubPlayerId(int index) {
        switch(index) {
            case 1: return B1Id;
            case 2: return B2Id;
            case 3: return B3Id;
            case 4: return B4Id;
            case 5: return B5Id;
            case 6: return B6Id;
            case 7: return B7Id;
            case 8: return B8Id;
            case 9: return B9Id;
            case 10: return B10Id;
            case 11: return B11Id;
            case 12: return B12Id;
            case 13: return B13Id;
            case 14: return B14Id;
            case 15: return B15Id;
            case 16: return B16Id;
            case 17: return B17Id;
            case 18: return B18Id;
            case 19: return B19Id;
            case 20: return B20Id;
            default : return 0;
        }
    }

    public void SetSubPlayerId(int id, int index) {
        switch(index) {
            case 1: B1Id = id; break;
            case 2: B2Id = id; break;
            case 3: B3Id = id; break;
            case 4: B4Id = id; break;
            case 5: B5Id = id; break;
            case 6: B6Id = id; break;
            case 7: B7Id = id; break;
            case 8: B8Id = id; break;
            case 9: B9Id = id; break;
            case 10: B10Id = id; break;
            case 11: B11Id = id; break;
            case 12: B12Id = id; break;
            case 13: B13Id = id; break;
            case 14: B14Id = id; break;
            case 15: B15Id = id; break;
            case 16: B16Id = id; break;
            case 17: B17Id = id; break;
            case 18: B18Id = id; break;
            case 19: B19Id = id; break;
            case 20: B20Id = id; break;
            default : break;
        }
    }

    public int SubPlayersCount() {
        int result = 0;
        int i = 1, playerId = GetSubPlayerId(i);
        while(playerId != 0) {
            result++;
            playerId = GetSubPlayerId(++i);
        }
        return result;
    }

    public List<int> GetSubPlayersList() {
        List<int> result = new List<int>();
        int i = 1, playerId = GetSubPlayerId(i);
        while(playerId != 0) {
            result.Add(playerId);
            playerId = GetSubPlayerId(++i);
        }
        return result;
    }

    public int FindSub(int id) {
        bool result = false;
        int index = 1, playerId = GetSubPlayerId(index);
        
        while(playerId != 0) {
            if(id == playerId) {
                result = true;
                break;
            }
            else {
                playerId = GetSubPlayerId(++index);
            }
        }
        return result ? index : GameConstants.NULL_INT;
    }

    public bool DeleteSub(int id) {
        int searchIndex = FindSub(id:id);

        if(searchIndex != GameConstants.NULL_INT) {
            int playerId = GetSubPlayerId(searchIndex + 1);

            while(playerId != 0) {
                SetSubPlayerId(id:playerId, index:searchIndex++);
                playerId = GetSubPlayerId(searchIndex + 1);
            }

            SetSubPlayerId(index:searchIndex, id:0);
        }

        return searchIndex != GameConstants.NULL_INT;
    }
}
