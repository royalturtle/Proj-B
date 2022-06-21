using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SQLite4Unity3d;

public class BatterFactory : PlayerFactory {
    public void CreateTestPlayer(
        GameDataService dataService, 
        int             teamId, 
        NationTypes     nationType, 
        DefensePosition defensePosition, 
        int             grade, 
        bool            isYoung = false, 
        bool            isMercenary=false
    ) {
        int nameIndex = GetRandomNameIndex(nationType);
        Hands[] hand  = GetRandomHands(defensePosition);

        int age = GetRandomAge(isYoung);

        TuplePlayerBase player = new TuplePlayerBase() {
            NameIndex = nameIndex,
            Nation    = nationType,
            BirthYear = GameConstants.GAME_START_YEAR - age,
            IsRetired = 0,
            ThrowHand = hand[0],
            HitHand   = hand[1]
        };

        dataService.Insert(player);

        PlayerGrowthType growthType = GetRandomGrowthType(potential:grade);
        PlayerGrowthDetailType growthDetailType = GetRandomGrowthDetailType();

        int[] max_stats = GetStatsByGrade(grade, defensePosition);

        TupleBatterStats batter = new TupleBatterStats() {
            PlayerID         = player.ID,
            TeamID           = teamId, 
            Age              = age,
            Potential        = grade,
            FAYear           = 4,
            Energy           = 100,
            Condition        = 20, 
            Fatigue          = 0, 
            Group            = 2,
            Salary           = 100,
            GrowthType       = growthType,
            GrowthDetailType = growthDetailType,
        };

        for(int i = 0; i < TupleBatterStats.StatCount(); i++) {
            batter.SetMaxStatByIndex(i, max_stats[i]);
        }
        
        // Defense
        int defenseMaxIndex = RunningStatIndex + 1;
        float maxDefense = max_stats[defenseMaxIndex];
        for(int i = RunningStatIndex + 2; i < max_stats.Length; i++) {
            if(max_stats[i] > maxDefense) {
                maxDefense = max_stats[i];
                defenseMaxIndex = i;
            }
        }

        float[] current_stats = new float[max_stats.Length];
        for(int i = 0; i < current_stats.Length; i++) {
            current_stats[i] = GetInitialStatValue(
                maxValue   : max_stats[i],
                age        : batter.Age,
                age_decay  : batter.AgeDecay,
                grade      : batter.Potential,
                growthType : batter.GrowthType
            );
        }

        current_stats[defenseMaxIndex] = maxDefense;

        for(int i = 0; i < TupleBatterStats.StatCount(); i++) {
            batter.SetStatByIndex(i, current_stats[i]);
        }

        if(batter.IsAgeDecaying) {
            for(int i = 0; i < TupleBatterStats.StatCount(); i++) {
                batter.SetMaxStatByIndex(i, current_stats[i]);
            }
        }

        dataService.Insert(batter);

        if(isYoung) {
            dataService.Insert(new TupleNewPlayer{PlayerId=player.ID});
        }

        // dataService.Insert(new TupleBatterSeason {Year = dataService.GetGameInfo().Year, TeamID = teamId, PlayerID = player.ID});
    }

    const int RunningStatIndex = 5;

    int[] GetStatsByGrade(int grade, DefensePosition defensePosition) {
        int[] result = new int[TupleBatterStats.StatCount()];

        // Grade Total
        int gradeTotal = GetGradeTotal(grade:grade);

        // 주력 관련
        result[RunningStatIndex] = GetSpeedStat();
        if (result[RunningStatIndex] > 3) {
            gradeTotal -= result[RunningStatIndex] - 3;
        }

        // 수비 관련
        int[] defenseStats = GetDefenseStats(defensePosition);
        int maxScore = 0;
        for (int i = 0; i < defenseStats.Length; i++) {
            if(maxScore < defenseStats[i]) {
                maxScore = defenseStats[i];
            }
            result[i + RunningStatIndex + 1] = defenseStats[i];
        }

        if (maxScore < 3) {
            gradeTotal++;
        }
        else if(maxScore > 3) {
            gradeTotal--;
        }

        // 배팅 능력치
        List<int> remainIndexList = new List<int>();
        for(int i = 0; i < RunningStatIndex; i++) {
            remainIndexList.Add(i);
        }

        for (int i = 0; i < 4; i++) {
            result[i] = 1;
        }

        int maxPoint = (grade >= 4) ? 5 : 4;
        while (gradeTotal > 0) {
            int remainIndex = remainIndexList[randomCreator.Next(0, remainIndexList.Count)];
            result[remainIndex] += 1;

            // Grade가 5면 후보에서 제거
            if (result[remainIndex] >= maxPoint) {
                remainIndexList.Remove(remainIndex);
            }
            gradeTotal--;
        }

        // 능력치로 환산
        for (int i = 0; i < result.Length; i++) {
            result[i] = GetExactStatByGrade(result[i]);
        }

        return result;
    }

    int GetGradeTotal(int grade) {
        int[] battingAllList;

        if (grade == 5) {
            battingAllList = new int[] { 17, 16, 15 };
        }
        else if (grade == 4) {
            battingAllList = new int[] { 14, 13, 12, 11 };
        }
        else if (grade == 3) {
            battingAllList = new int[] { 10, 9 };
        }
        else if (grade == 2) {
            battingAllList = new int[] { 8, 7 };
        }
        else {
            battingAllList = new int[] { 6 };
        }

        return battingAllList[randomCreator.Next(0, battingAllList.Length)];
    }

    int GetSpeedStat() {
        return RandomSelected(new int[] { 5, 39, 89, 97, 100 }) + 1;
    }

    int[] GetDefenseStats(DefensePosition defensePosition) {
        // 일단 최고점
        int[] gradeBaseLine = new int[] { 5, 15, 85, 95, 100 };
        int maxScore = RandomSelected(gradeBaseLine) + 1;

        int index;
        int[] result = new int[] { 1, 1, 1, 1, 1, 1 };
        if(maxScore > 2) { 
            switch (defensePosition) {
                case DefensePosition.C:
                    index = RandomSelected(new int[] { 7, 100 });
    
                    switch(index) {
                        case 0:     // 포수, 1루수
                            result = new int[] { maxScore, (maxScore > 1) ? 2 : 1 , 1, 1, 1, 1 };
                            break;
                        default:    // 포수
                            result = new int[] { maxScore, 1, 1, 1, 1, 1 };
                            break;
                    } 
                    break;
                case DefensePosition.B1:
                    result[1] = maxScore;                                                       // 1루 (기본)
                    result[2] = GetDefenseMinus(maxScore, new int[] { 0, 5, 10, 15, 100 });     // 2루
                    result[3] = GetDefenseMinus(maxScore, new int[] { 0, 20, 45, 70, 100 });    // 3루
                    result[4] = GetDefenseMinus(maxScore, new int[] { 0, 1, 2, 3, 100 });    // 유격
                    result[5] = GetDefenseMinus(maxScore, new int[] { 0, 20, 45, 70, 100 });    // 외야
                    break;
                case DefensePosition.B2:
                    result[1] = GetDefenseMinus(maxScore, new int[] { 0, 50, 80, 90, 100 });    // 1루
                    result[2] = maxScore;                                                       // 2루 (기본)
                    result[3] = GetDefenseMinus(maxScore, new int[] { 0, 40, 80, 90, 100 });    // 3루
                    result[4] = GetDefenseMinus(maxScore, new int[] { 0, 50, 80, 90, 100 });    // 유격
                    result[5] = GetDefenseMinus(maxScore, new int[] { 0, 20, 45, 70, 100 });    // 외야
                    break;
                case DefensePosition.B3:
                    result[1] = GetDefenseMinus(maxScore, new int[] { 0, 50, 80, 90, 100 });    // 1루
                    result[2] = GetDefenseMinus(maxScore, new int[] { 0, 30, 60, 80, 100 });    // 2루
                    result[3] = maxScore;                                                       // 3루 (기본)
                    result[4] = GetDefenseMinus(maxScore, new int[] { 0, 40, 70, 85, 100 });    // 유격
                    result[5] = GetDefenseMinus(maxScore, new int[] { 0, 15, 35, 60, 100 });    // 외야
                    break;
                case DefensePosition.SS:
                    result[1] = GetDefenseMinus(maxScore, new int[] { 0, 50, 80, 90, 100 });    // 1루
                    result[2] = GetDefenseMinus(maxScore, new int[] { 0, 60, 90, 95, 100 });    // 2루
                    result[3] = GetDefenseMinus(maxScore, new int[] { 0, 60, 90, 95, 100 });    // 3루
                    result[4] = maxScore;                                                       // 유격 (기본)
                    result[5] = GetDefenseMinus(maxScore, new int[] { 0, 15, 35, 60, 100 });    // 외야
                    break;
                case DefensePosition.OF:
                    result[1] = GetDefenseMinus(maxScore, new int[] { 0, 40, 80, 90, 100 });    // 1루
                    result[2] = GetDefenseMinus(maxScore, new int[] { 0, 5, 10, 15, 100 });    // 2루
                    result[3] = GetDefenseMinus(maxScore, new int[] { 0, 5, 10, 15, 100 });    // 3루
                    result[4] = GetDefenseMinus(maxScore, new int[] { 0, 5, 10, 15, 100 });    // 유격
                    result[5] = maxScore;                                                       // 외야 (기본)
                    break;
            }
        } 
        else {
            result[(int)defensePosition] = maxScore;
        }

        return result;
    }

    int GetDefenseMinus(int maxScore, int[] seeds) {
        int temp = maxScore - RandomSelected(seeds);
        return (temp > 1) ? temp : 1;
    }
}