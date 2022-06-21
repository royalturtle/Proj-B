using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using UnityEngine;

public class LineupDataMediator {
    GameDataMediator GameData;

    int PlayersBatterLineupOrder = 0;
    Dictionary<int, List<TupleLineupBatter>> BatterLineup;
    Dictionary<int, TupleLineupPitcher> PitcherLineup;

    public TupleLineupBatter GetTupleLineupBatter(int teamId, int index = 0) {
        return BatterLineup != null && 
            BatterLineup.ContainsKey(teamId) &&
            Utils.IsValidIndex(BatterLineup[teamId], index) ?
            BatterLineup[teamId][index] : null;
    }

    public TupleLineupPitcher GetTupleLineupPitcher(int teamId) {
        return PitcherLineup != null && 
            PitcherLineup.ContainsKey(teamId) ?
            PitcherLineup[teamId] : null;
    }

    public LineupDataMediator(GameDataMediator gameData) {
        GameData = gameData;

        Dictionary<int, Team> teamsDict = GameData.Teams.TeamDict;
        
        // DB에서 라인업 정보를 가져옴
        // 불러온 투수 라인업을 등록
        PitcherLineup = gameData.GameDB.GetPitcherLineup();
        if(PitcherLineup == null)PitcherLineup = new Dictionary<int, TupleLineupPitcher>();

        // 타자 라인업을 불러옴
        BatterLineup = new Dictionary<int, List<TupleLineupBatter>>();
        List<TupleLineupBatter> batterLineupList = gameData.GameDB.GetBatterLineup();

        // 불러온 타자 라인업을 등록시킴
        foreach(TupleLineupBatter batterLineup in batterLineupList) {
            if(!BatterLineup.ContainsKey(batterLineup.TeamId)) {
                BatterLineup[batterLineup.TeamId] = new List<TupleLineupBatter>();
            }

            BatterLineup[batterLineup.TeamId].Add(batterLineup);
        }

        // 모든 팀마다 혹시 라인업이 비어있는 팀이 있는지 확인
        foreach (KeyValuePair<int, Team> team in teamsDict) {
            if(!BatterLineup.ContainsKey(team.Value.ID) || BatterLineup[team.Value.ID].Count < 1) {
                BatterLineup[team.Value.ID] = new List<TupleLineupBatter>();
                TupleLineupBatter newBatterLineup = SetBatterLineUp(teamId:team.Value.ID, totalCount:14);
                BatterLineup[team.Value.ID].Add(newBatterLineup);
            }

            if(!PitcherLineup.ContainsKey(team.Value.ID)) {
                PitcherLineup[team.Value.ID] = SetPitcherLineUp(teamId:team.Value.ID, totalCount:12);
            }
        }
    }

    public int Group1RegisterLimitation {
        get{
            int result = 0;
            if(GameData != null) {
                result = GameConstants.Group1RegisterLimitation(
                    nation : GameData.LeagueNation,
                    year   : GameData.CurrentYear,
                    turn   : GameData.CurrentTurn
                );
            }
            else {
                result = GameConstants.Group1RegisterLimitation(nation:NationTypes.NONE);
            }
            return result;
        }
    }

    public int Group1PitcherLimitation {
        get {
            return GameConstants.Group1PitcherLimitation(
                nation : GameData != null ? GameData.LeagueNation : NationTypes.NONE
            );
        }
    }

    public (int, int) GetMyGroup1Count() {
        return GetGroup1EachCount(GameData.MyTeamIndex);
    }

    public (int, int) GetGroup1EachCount(int teamId) {
        int batterCount = 0, pitcherCount = 0;
        LineupBatter batterLineup = GetLineupBatter(teamId:teamId);
        LineupPitcher pitcherLineup = GetLineupPitcher(teamId:teamId);

        if(batterLineup != null) {
            batterCount = batterLineup.Group1Count;
        }

        if(pitcherLineup != null) {
            pitcherCount = pitcherLineup.Group1Count;
        }
        
        return (batterCount, pitcherCount);
    }
    
    public int GetGroup1Count(int teamId) {
        (int batterCount, int pitcherCount) = GetGroup1EachCount(teamId:teamId);
        return batterCount + pitcherCount;
    }

    public int GetStartingOrder(int teamId) {
        return PitcherLineup[teamId].StartingOrder;
    }

    public void SetStartingOrder(int teamId, int value = 1, bool isNext=false) {
        if(isNext) {
            int StartingCount = PitcherLineup[teamId].StartingsCount;
            value = GetStartingOrder(teamId) + 1;
            value = (value > StartingCount) ? value = 1 : value;
            PitcherLineup[teamId].StartingOrder = value;
            GameData.GameDB.UpdateData(PitcherLineup[teamId]);
        }
        else {
            PitcherLineup[teamId].StartingOrder = value;
            GameData.GameDB.UpdateData(PitcherLineup[teamId]);
        }
    }
    
    TupleLineupBatter SetBatterLineUp(int teamId, int totalCount, bool isDH = true, bool isContainGroup2 = false) {
        // 선수 리스트
        Expression<Func<TupleBatterStats, bool>> restriction1 = p => (p.TeamID == teamId);
        List<TupleBatterStats> batters = GameData.GameDB.Select<TupleBatterStats>(restriction1).ToList();

        // 2군 제외
        if(isContainGroup2) {
            batters.Remove(batters.Find(p => p.Group == 2));
        }

        // Ready Result Object
        TupleLineupBatter result = new TupleLineupBatter(
            teamId : teamId,
            name   : "Basic"
        );

        for(BatterPositionEnum position = BatterPositionEnum.C; position <= BatterPositionEnum.DH; position++) {
            result.SetOrderOfPosition(order:(int)position, position:position);
        }

        TupleBatterStats[] addBatterArray = new TupleBatterStats[9];

        List<BatterPositionEnum> addPositionList = new List<BatterPositionEnum>{
	        BatterPositionEnum.C,
	        BatterPositionEnum.SS,
	        BatterPositionEnum.B2,
	        BatterPositionEnum.CF,
	        BatterPositionEnum.B3,
	        BatterPositionEnum.RF,
	        BatterPositionEnum.LF,
	        BatterPositionEnum.B1
        };

        for(int i = 0; i < addPositionList.Count; i++) {
	        BatterPositionEnum position = addPositionList[i];
	        double startDefense = GetStartDefenseAbility(position);
            int newBatterIndex;
	
	        (newBatterIndex, startDefense) = FindBatter(batters:batters, position:position, startDefense:startDefense);
	        if(newBatterIndex == GameConstants.NULL_INT) {
		        (newBatterIndex, startDefense) = FindBatter(
                    batters      : batters,
                    position     : position,
                    startDefense : (double)((int)startDefense / 10) * 10.0
                );	
	        }
	
	        if(Utils.IsValidIndex(batters, newBatterIndex)) {
                TupleBatterStats player = batters[newBatterIndex];
		        addBatterArray[(int)position - 1] = player;
                player.Group = 1;
                GameData.Batters.UpdateStats(player);
		        batters.RemoveAt(newBatterIndex);
	        }
        }

        if (isDH) {
            batters.Sort((playerA, playerB) => playerB.Attack.CompareTo(playerA.Attack));
            addBatterArray[(int)BatterPositionEnum.DH - 1] = batters[0];
            batters[0].Group = 1;
            GameData.Batters.UpdateStats(batters[0]);
            batters.RemoveAt(0);
        }

        for(int i = 0; i < addBatterArray.Length; i++) {
            if(addBatterArray[i] != null) {
                result.SetIdOfOrder(addBatterArray[i].PlayerID, order : i + 1);
            }
        }


        Dictionary<BatterPositionEnum, List<TupleBatterStats>> batterDictionary = new Dictionary<BatterPositionEnum, List<TupleBatterStats>>();
        for(BatterPositionEnum position = BatterPositionEnum.C; position <= BatterPositionEnum.LF; position++) {
            batterDictionary.Add(position, new List<TupleBatterStats>());
        }

        // 선수들을 Main Position를 통해 구분
        for(int i = 0; i < batters.Count; i++) {
            batterDictionary[batters[i].MainDefense].Add(batters[i]);
        }

        for(BatterPositionEnum position = BatterPositionEnum.C; position <= BatterPositionEnum.LF; position++) {
            batterDictionary[position].Sort((playerA, playerB) => playerB.Attack.CompareTo(playerA.Attack));
        }

        int candidateStart = isDH ? 9 : 8;
        int skipPosition = 0, batterCount = batters.Count;

        for(int i = 0; candidateStart + i < totalCount && i < batterCount; i++) {
            BatterPositionEnum batterPosition = GetCandidatePosition(i + skipPosition);
            if(batterPosition == BatterPositionEnum.NONE || batterDictionary[batterPosition].Count < 1) {
                batterPosition = BatterPositionEnum.NONE;
                for(BatterPositionEnum position = BatterPositionEnum.LF; position >= BatterPositionEnum.C; position--) {
                    if(batterDictionary[position].Count > 0) {
                        batterPosition = position;
                        break;
                    }
                }
            }

            if(batterPosition == BatterPositionEnum.NONE) {
                break;
            }
            else {
                TupleBatterStats player = batterDictionary[batterPosition][0];
                batterDictionary[batterPosition].RemoveAt(0);
                player.Group = 1;
                GameData.Batters.UpdateStats(player);
                result.SetSubPlayerId(id:player.PlayerID, index:i + 1);
            }
        }

        GameData.GameDB.Insert(result);
        SortBatter(result);
        return result;
    }

    (int, double) FindBatter(
    	List<TupleBatterStats> batters,
    	BatterPositionEnum position, 
    	double startDefense
    ) {
	    double maxDefense = 0.0, maxAttack = 0.0;
	    int checkIndex = GameConstants.NULL_INT;
	    for(int i = 0; i < batters.Count; i++) {
		    double currentDefense = batters[i].GetDefenseAbility(position);
		    if(currentDefense >= startDefense) {
			    if(batters[i].Attack > maxAttack) {
                    maxAttack = batters[i].Attack;
				    checkIndex = i;
    			}
		    }
		    else if(checkIndex == GameConstants.NULL_INT) {
			    if(currentDefense > maxDefense) {
				    maxDefense = currentDefense;
    			}
		    }
    	}
    	return (checkIndex, maxDefense);
    }

    double GetStartDefenseAbility(BatterPositionEnum position) {
	    switch(position) {
		    case BatterPositionEnum.C:  return 70.0;
		    case BatterPositionEnum.B1: return 50.0;
		    case BatterPositionEnum.B2: return 60.0;
		    case BatterPositionEnum.B3: return 60.0;
		    case BatterPositionEnum.SS: return 60.0;
		    case BatterPositionEnum.LF: return 50.0;
		    case BatterPositionEnum.CF: return 60.0;
		    case BatterPositionEnum.RF: return 60.0;
		    default : return 0.0;
    	}
    }

    public void SortBatter(TupleLineupBatter lineup) {
        if(GameData != null && GameData.Batters != null && lineup != null) {
            TupleBatterStats[] result = new TupleBatterStats[9];
            int[] settingArray = new int[] {2, 3, 0, 1, 4, 5, 6, 8, 7};
            List<TupleBatterStats> batterList = new List<TupleBatterStats>();
            Dictionary<int, BatterPositionEnum> formerDictionary = new Dictionary<int, BatterPositionEnum>();

            for(int i = 1; i <= 9; i++) {
                batterList.Add(GameData.Batters.GetBatter(id:lineup.GetIdOfStartingOrder(i)).Stats);
            }

            for(BatterPositionEnum position = BatterPositionEnum.C; position <= BatterPositionEnum.DH; position++) {
                int order = lineup.GetOrderOfPosition(position) - 1;
                if(order < batterList.Count && batterList[order] != null) {
                    formerDictionary.Add(batterList[order].PlayerID, position);
                }
            }

            for(int i = 0; i < settingArray.Length; i++) {
                if(i == 0 || i == 3) {
                    batterList.Sort((playerA, playerB) => playerB.Attack.CompareTo(playerA.Attack));
                }
                else if(i == 2) {
                    batterList.Sort((playerA, playerB) => playerB.LeadOffAttacks.CompareTo(playerA.LeadOffAttacks));
                }

                result[settingArray[i]] = batterList[0];
                batterList.RemoveAt(0);
            }

            for(int i = 0; i < result.Length; i++) {
                if(result[i] != null) {
                    int playerId = result[i].PlayerID;

                    lineup.SetIdOfOrder(id:playerId, order:i+1);
                    lineup.SetOrderOfPosition(
                        order    : i + 1, 
                        position : formerDictionary.ContainsKey(playerId) ? formerDictionary[playerId] : BatterPositionEnum.NONE
                    );
                }
            }

            GameData.GameDB.UpdateData(lineup);
        }
    }

    BatterPositionEnum GetCandidatePosition(int index) {
        switch(index) {
            case 0 : return BatterPositionEnum.C;
            case 1 : return BatterPositionEnum.SS;
            case 2 : return BatterPositionEnum.B2;
            case 3 :
            case 4 : return BatterPositionEnum.LF;
            case 5 : return BatterPositionEnum.B3;
            case 6 : return BatterPositionEnum.B1;
            case 7 : return BatterPositionEnum.SS;
            default: return BatterPositionEnum.NONE;
        }
    }

    TupleLineupPitcher SetPitcherLineUp(int teamId, int totalCount, int[] pitcherStructure = null, bool isContainGroup2 = false) {
        Expression<Func<TuplePitcherStats, bool>> restriction2 = p => (p.TeamID == teamId);
        List<TuplePitcherStats> pitchers = GameData.GameDB.Select<TuplePitcherStats>(restriction2).ToList();

        if(isContainGroup2) {
            pitchers.Remove(pitchers.Find(p => p.Group == 2));
        }

        TupleLineupPitcher result = new TupleLineupPitcher {
            TeamId = teamId,
            StartingOrder = 1
        };

        // 투수 로테이션 관련 생성
        int[] structure = pitcherStructure;
        if(structure == null) {
            structure = new int[] { 5, totalCount - 7, 1, 1 };
        }

        // 투스들의 체력을 리스트 별로 해서 나눔
        int[] staminaCountList = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        for (int i = 0; i < pitchers.Count; i++) {
            staminaCountList[((int)pitchers[i].Stamina) / 10]++;
        }

        // 최소 Stamina 기준 확인 및 조정
        int staminaCount = 0, staminaBaseLine = 60;
        for(int i = 10; i >= staminaBaseLine / 10; i--) {
            staminaCount += staminaCountList[i];
        }

        // Stamina의 기준점 확인하기
        while (staminaCount < structure[0]) {
            staminaBaseLine -= 10;
            staminaCount += staminaCountList[staminaBaseLine / 10];
        }

        List<TuplePitcherStats> startCandidates = new List<TuplePitcherStats>();
        List<TuplePitcherStats> reliefCandidates = new List<TuplePitcherStats>();

        // Stamina 기준을 토대로 선발과 중계 list로 투수를 구분
        for (int i = 0; i < pitchers.Count; i++) {
            if (pitchers[i].Stamina >= staminaBaseLine) {
                startCandidates.Add(pitchers[i]);
            }
            else {
                reliefCandidates.Add(pitchers[i]);
            }
        }

        // 선발 투수를 능력치 별로 정렬
        startCandidates.Sort((playerA, playerB) => playerA.StartingAbility.CompareTo(playerB.StartingAbility));
        startCandidates.Reverse();

        // 선발 투수 정하기
        for (int i = 0; i < structure[0]; i++) {
            result.SetStartingId(index:i+1, id:startCandidates[i].PlayerID);

            startCandidates[i].Group = 1;
            GameData.Pitchers.UpdateStats(startCandidates[i]);
        }

        // 선발 탈락된 선수들 중계로 넣기
        for (int i = structure[0]; i < startCandidates.Count; i++) {
            reliefCandidates.Add(startCandidates[i]);
        }
        reliefCandidates.Sort((playerA, playerB) => playerA.ReliefAbility.CompareTo(playerB.ReliefAbility));
        reliefCandidates.Reverse();

        int reliefCount = 0;

        // 마무리
        if(structure[3] == 1) {
            result.CloserId = reliefCandidates[reliefCount].PlayerID;
            reliefCandidates[reliefCount].Group = 1;
            GameData.Pitchers.UpdateStats(reliefCandidates[reliefCount]);
            reliefCount++;
        }

        // 셋업
        if(structure[2] == 1) {
            result.SetupId = reliefCandidates[reliefCount].PlayerID;
            reliefCandidates[reliefCount].Group = 1;
            GameData.Pitchers.UpdateStats(reliefCandidates[reliefCount]);
            reliefCount++;
        }

        // 중계진
        for(int i = 0; i < structure[1]; i++) {
            result.SetReliefId(index:i+1, id:reliefCandidates[reliefCount].PlayerID);
            reliefCandidates[reliefCount].Group = 1;
            GameData.Pitchers.UpdateStats(reliefCandidates[reliefCount]);
            reliefCount++;
        }

        GameData.GameDB.Insert(result);
        return result;
    }

    public LineupBatter GetMyLineupBatter() {
        return GetLineupBatter(teamId:GameData.MyTeamIndex, order:PlayersBatterLineupOrder);
    }

    public LineupBatter GetLineupBatter(int teamId, int order = 0) {
        return new LineupBatter(
            gameData:GameData,
            tuple:BatterLineup[teamId][order]
        );
    }

    public LineupPitcher GetMyLineupPitcher() {
        return GetLineupPitcher(GameData.MyTeamIndex);
    }

    public LineupPitcher GetLineupPitcher(int teamId) {
        return new LineupPitcher(
            gameData : GameData.Pitchers,
            tuple    : PitcherLineup[teamId]
        );
    }

    public TupleLineupPitcher GetLineupPitcherTuple(int teamId) {
        return PitcherLineup != null && PitcherLineup.ContainsKey(teamId) ? PitcherLineup[teamId] : null;
    }

    public void ChangeBatterPosition(Batter batter, BatterPositionEnum newPosition) {
        if(batter != null) {
            if(BatterPositionEnum.C <= newPosition && newPosition <= BatterPositionEnum.DH) {
                TupleLineupBatter tuple = GetTupleLineupBatter(teamId:batter.Stats.TeamID);

                if(tuple != null) {
                    // batter의 현재 포지션을 얻어옴
                    BatterPositionEnum batterPosition = tuple.GetPositionOfId(batter.Base.ID);
                
                    if(batterPosition != newPosition && BatterPositionEnum.C <= batterPosition && batterPosition <= BatterPositionEnum.DH) {
                        // batter의 타순을 얻어옴
                        int batterOrder = tuple.FindOrderById(batter.Base.ID);
                
                        // 변경하고자 하는 포지션의 타순을 얻어옴
                        int otherOrder = tuple.GetOrderOfPosition(newPosition);

                        // 변경하고자 하는 포지션의 현재 선수 ID를 얻어옴
                        int otherId = tuple.GetIdOfPosition(newPosition);

                        // 해당 포지션 선수의 ID 가 변경될 선수의 ID가 다를 경우
                        if(batter.Base.ID != otherId) {
                            // batter의 타순과 before 선수의 타순의 내용을 서로 변경함
                            tuple.SetOrderOfPosition(batterOrder, newPosition);
                            tuple.SetOrderOfPosition(otherOrder, batterPosition);

                            GameData.GameDB.UpdateData(tuple);
                        }
                    }
                }
            }
        }
    }

    public ErrorType ChangePitcherPosition(Pitcher pitcher, PitcherPositionEnum position) {
        ErrorType result = ErrorType.InvalidPlayer;
        if(pitcher != null) {
            if(position != PitcherPositionEnum.NONE || position != PitcherPositionEnum.GROUP2) {
                TupleLineupPitcher tuple = GetLineupPitcherTuple(teamId:pitcher.Stats.TeamID);

                (PitcherPositionEnum originalPosition, int index) = tuple.FindId(pitcher.Base.ID);

                if((position == PitcherPositionEnum.SETUP && tuple.IsSetupExist) || (position == PitcherPositionEnum.CLOSER &&tuple.IsCloserExist)) {
                    result = ErrorType.PitcherPositionExist;
                }
                else if((position == PitcherPositionEnum.STARTING && tuple.IsStartingFull) || (position == PitcherPositionEnum.RELIEF && tuple.IsReliefFull)) {
                    result = ErrorType.PitcherPositionFull;
                }
                else if(originalPosition == PitcherPositionEnum.STARTING && tuple.StartingsCount <= 1) {
                    result = ErrorType.PitcherStartingZero;
                }
                else {
                    // Lineup에서 pitcher의 position을 변경한 결과
                    if(tuple.ChangePosition(id:pitcher.Base.ID, position:position)) {
                        GameData.GameDB.UpdateData(tuple);
                        result = ErrorType.None;
                    }
                }
            }
        }
        return result;
    }

    public bool IsGroup1Full(int teamId) {
        (int batterCount, int pitcherCount) = GetGroup1EachCount(teamId);
        int totalCount = batterCount + pitcherCount;
        int maxCount = Group1RegisterLimitation;

        return totalCount >= maxCount;
    }

    public ErrorType BatterToGroup1(Batter batter) {
        ErrorType result = ErrorType.InvalidPlayer;
        int remainDays = 0;
        if(batter != null && batter.Stats.Group == 2) {
            // 2군 등록 정보 확인
            remainDays = GameData.Group2.RemainDays(batter.Base.ID);
            
            if(IsGroup1Full(teamId:batter.Stats.TeamID)) {
                result = ErrorType.ExceedGroup1Count;
            }
            // 2군 남은 등록일이 0이하일 경우
            else if(remainDays <= 0) {
                TupleLineupBatter tuple = GetTupleLineupBatter(teamId:batter.Stats.TeamID);
                int subPlayerIndex = tuple.SubPlayersCount() + 1;

                if(subPlayerIndex > TupleLineupBatter.SubCountMax) {
                    result = ErrorType.BatterSubFull;
                }
                else {
                    tuple.SetSubPlayerId(id:batter.Base.ID, index:subPlayerIndex);
                    GameData.GameDB.UpdateData(tuple);

                    GameData.Group2.DeleteData(batter.Base.ID);
                    batter.Stats.Group = 1;
                    GameData.Batters.UpdateStats(batter.Stats);
                    result = ErrorType.None;
                }
            }
            else {
                result = ErrorType.Group2DayRemains;
            }
        }
        return result;
    }

    public ErrorType BatterToGroup2(Batter batter) {
        ErrorType result = ErrorType.InvalidPlayer;
        if(batter != null  && batter.Stats.Group != 2) {
            TupleLineupBatter tuple = GetTupleLineupBatter(teamId:batter.Stats.TeamID);
            int subCount = tuple.SubPlayersCount();

            // 후보가 1명이라도 있을 때
            if(subCount >= 1) {
                // batter의 현재 포지션을 얻어옴
                BatterPositionEnum batterPosition = tuple.GetPositionOfId(batter.Base.ID);

                // 새로 집어넣을 선수
                Batter newBatter = null;

                // position이 DH 일 경우
                if(batterPosition == BatterPositionEnum.DH) {
                    // 공격력 가장 높은 선수
                    for(int i = 1; i <= subCount; i++) {
                        Batter compareBatter = GameData.Batters.GetBatter(tuple.GetSubPlayerId(i));
                        if(compareBatter != null && (newBatter == null || (newBatter.Stats.Attack < compareBatter.Stats.Attack))) {
                            newBatter = compareBatter;
                        }
                    }
                }
                else if(batterPosition == BatterPositionEnum.CANDIDATE) {
                    if(tuple.DeleteSub(id:batter.Base.ID)) {
                        result = ErrorType.None;
                    }
                }
                // 그 외에
                else if(batterPosition != BatterPositionEnum.GROUP2 && batterPosition != BatterPositionEnum.NONE) {
                    // 해당 포지션에 가장 높은 수비력
                    for(int i = 1; i <= subCount; i++) {
                        Batter compareBatter = GameData.Batters.GetBatter(tuple.GetSubPlayerId(i));
                        if(compareBatter != null && (newBatter == null || (newBatter.Stats.GetDefenseAbility(batterPosition) < compareBatter.Stats.GetDefenseAbility(batterPosition)))) {
                            newBatter = compareBatter;
                        }
                    }
                }

                if(newBatter != null) {
                    result = ErrorType.None;
                }

                if(result == ErrorType.None) {
                    if(newBatter != null) {// batter의 타순을 얻어옴
                        tuple.SetIdOfOrder(
                            id    : newBatter.Base.ID, 
                            order : tuple.FindOrderById(batter.Base.ID)
                        );
                        
                        tuple.DeleteSub(newBatter.Base.ID);
                    }
                    batter.Stats.Group = 2;
                    GameData.Batters.UpdateStats(batter.Stats);
                    GameData.GameDB.UpdateData(tuple);

                    // 2군 남은 일수 등록
                    GameData.Group2.AddData(batter.Base.ID);
                }
            }
            else {
                result = ErrorType.ScarceBatterCount;
            }
        }
        return result;
    }

    public ErrorType PitcherToGroup1(Pitcher pitcher, bool isAutoFilled = true) {
        ErrorType result = ErrorType.InvalidPlayer;
        int remainDays = 0;
        if(pitcher != null && pitcher.Stats.Group == 2) {
            TupleLineupPitcher tuple = GetTupleLineupPitcher(teamId:pitcher.Stats.TeamID);

            if(tuple != null) {
                if(GetGroup1Count(pitcher.Stats.TeamID) >= Group1RegisterLimitation) {
                    result = ErrorType.ExceedGroup1Count;
                }
                else if(tuple.Count >= Group1PitcherLimitation) {
                    result = ErrorType.ExceedGroup1Count;
                }
                else {
                    // 2군 등록일 확인
                    remainDays = GameData.Group2.RemainDays(pitcher.Base.ID);
            
                    // 2군 남은 등록일이 0이하일 경우
                    if(remainDays <= 0) {
                        // 중계에 추가
                        if(isAutoFilled && tuple.ReliefsCount < TupleLineupPitcher.ReliefsMax) {
                            tuple.AddPosition(pitcher.Base.ID, PitcherPositionEnum.RELIEF);
                            GameData.GameDB.UpdateData(tuple);    
                        }
                
                        GameData.Group2.DeleteData(pitcher.Base.ID);

                        pitcher.Stats.Group = 1;
                        GameData.Pitchers.UpdateStats(pitcher.Stats);
                        result = ErrorType.None;
                    }
                    else {
                        result = ErrorType.Group2DayRemains;
                    }
                }
            }
        }
        return result;
    }

    public ErrorType PitcherToGroup2(Pitcher pitcher) {
        ErrorType result = ErrorType.InvalidPlayer;
        if(pitcher != null && pitcher.Stats.Group != 2) {
            TupleLineupPitcher tuple = GetTupleLineupPitcher(teamId:pitcher.Stats.TeamID);
            if(tuple != null) {
                PitcherPositionEnum position = PitcherPositionEnum.NONE;
                int order = 0;
                (position, order) = tuple.FindId(pitcher.Base.ID);
                if(tuple.StartingsCount <= 1 && position == PitcherPositionEnum.STARTING) {
                    result = ErrorType.ScarcePitcherCount;
                }
                else {
                    bool deleteResult = tuple.DeletePlayer(pitcher.Base.ID);

                    if(deleteResult) {
                        pitcher.Stats.Group = 2;
                        GameData.Pitchers.UpdateStats(pitcher.Stats);
                        GameData.GameDB.UpdateData(tuple);

                        // 2군 남은 일수 등록
                        GameData.Group2.AddData(pitcher.Base.ID);
                        result = ErrorType.None;
                    }
                }
            }
        }
        return result;
    }

    public ErrorType ChangeBatter(Batter player1, Batter player2) {
        ErrorType result = ErrorType.InvalidPlayer;
        if(player1 != null && player2 != null) {
            TupleLineupBatter tuple;
            bool isBatter1Group1 = player1.Stats.Group == 1;
            bool isBatter2Group1 = player2.Stats.Group == 1;

            if(isBatter1Group1 && isBatter2Group1) {
                tuple = GetTupleLineupBatter(teamId:player1.Stats.TeamID);

                int player1Order = tuple.FindOrderById(id:player1.Base.ID);
                bool isPlayer1Starting = player1Order > 0;
                if(!isPlayer1Starting) {
                    player1Order = tuple.FindSub(id:player1.Base.ID);
                }
                int player2Order = tuple.FindOrderById(id:player2.Base.ID);
                bool isPlayer2Starting = player2Order > 0;
                if(!isPlayer2Starting) {
                    player2Order = tuple.FindSub(id:player2.Base.ID);
                }

                if(isPlayer1Starting) {
                    tuple.SetIdOfOrder(id:player2.Base.ID, order:player1Order);
                }
                else {
                    tuple.SetSubPlayerId(id:player2.Base.ID, index:player1Order);
                }

                if(isPlayer2Starting) {
                    tuple.SetIdOfOrder(id:player1.Base.ID, order:player2Order);
                }
                else {
                    tuple.SetSubPlayerId(id:player1.Base.ID, index:player2Order);
                }

                UpdateLineupTuple(tuple);
                result = ErrorType.None;
            }
            else if(!isBatter1Group1 && !isBatter2Group1) {
                result = ErrorType.Group2Change;
            }
            else  {
                tuple = GetTupleLineupBatter(teamId:player1.Stats.TeamID);

                Batter group1 = isBatter1Group1 ? player1 : player2;
                Batter group2 = isBatter1Group1 ? player2 : player1;

                if(!GameData.Group2.IsChangeable(group2.Base.ID)) {
                    result = ErrorType.Group2DayRemains;
                }
                else {
                    int playerOrder = tuple.FindOrderById(id:group1.Base.ID);
                    bool isPlayerStarting = playerOrder > 0;
                    if(!isPlayerStarting) {
                        playerOrder = tuple.FindSub(id:group1.Base.ID);
                    }

                    // Group 2 Player를 1군으로
                    BatterToGroup1(group2);

                    // Group 1 Player를 2군으로
                    BatterToGroup2(group1);

                    // Group 1의 자리를 Group 2에게
                    if(isPlayerStarting) {
                        tuple.SetIdOfOrder(id:group2.Base.ID, order:playerOrder);
                    }
                    else {
                        tuple.SetSubPlayerId(id:group2.Base.ID, index:playerOrder);
                    }

                    UpdateLineupTuple(tuple);
                    result = ErrorType.None;
                }
            }
        }
        return result;
    }

    void UpdateLineupTuple(TupleLineupBatter tuple) {
        if(GameData != null && tuple != null) {
            GameData.GameDB.UpdateData(tuple);
        }
    }

    void UpdateLineupTuple(TupleLineupPitcher tuple) {
        if(GameData != null && tuple != null) {
            GameData.GameDB.UpdateData(tuple);
        }
    }

    public ErrorType ChangePitcher(Pitcher player1, Pitcher player2) {
        ErrorType result = ErrorType.InvalidPlayer;
        if(player1 != null && player2 != null) {
            TupleLineupPitcher tuple = GetTupleLineupPitcher(teamId:player1.Stats.TeamID);
            
            if(tuple != null) {
                bool isPlayer1Group1 = player1.Stats.Group == 1;
                bool isPlayer2Group1 = player2.Stats.Group == 1;

                if(isPlayer1Group1 && isPlayer2Group1) {
                    (PitcherPositionEnum position1, int index1) = tuple.FindId(player1.Base.ID);
                    (PitcherPositionEnum position2, int index2) = tuple.FindId(player2.Base.ID);
                    
                    tuple.SetId(
                        id       : player1.Base.ID,
                        position : position2,
                        index    : index2
                    );

                    tuple.SetId(
                        id       : player2.Base.ID,
                        position : position1,
                        index    : index1
                    );

                    UpdateLineupTuple(tuple);
                    result = ErrorType.None;
                }
                else if (!isPlayer1Group1 && !isPlayer2Group1){
                    result = ErrorType.Group2Change;
                }
                else {
                    Pitcher group1 = isPlayer1Group1 ? player1 : player2;
                    Pitcher group2 = isPlayer2Group1 ? player2 : player1;

                    if(!GameData.Group2.IsChangeable(group2.Base.ID)) {
                        result = ErrorType.Group2DayRemains;
                    }
                    else {
                        (PitcherPositionEnum position1, int index1) = tuple.FindId(group1.Base.ID);

                        // Group 1 Player를 2군으로
                        PitcherToGroup2(group1);

                        // Group 2 Player를 1군으로
                        PitcherToGroup1(group2, isAutoFilled:false);

                        // Group 1의 자리를 Group 2에게
                        tuple.SetId(
                            id       : group2.Base.ID,
                            position : position1,
                            index    : index1
                        );

                        UpdateLineupTuple(tuple);
                        result = ErrorType.None;
                    }
                }
            }
        }
        return result;
    }
}
