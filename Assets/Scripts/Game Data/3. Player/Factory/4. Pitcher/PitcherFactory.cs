using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitcherFactory : PlayerFactory {
    public void CreateTestPlayer(GameDataService dataService, int teamId, NationTypes nationType, PitcherTypes pitcherType, int grade, bool isYoung = false, bool isMercenary=false) {
        int nameIndex = GetRandomNameIndex(nationType);
        Hands[] hand = GetRandomHands();

        int age = GetRandomAge(isYoung);

        TuplePlayerBase player = new TuplePlayerBase() {
            NameIndex = nameIndex,
            Nation = nationType,
            BirthYear = GameConstants.GAME_START_YEAR - age,
            IsRetired = 0,
            ThrowHand = hand[0],
            HitHand = hand[1]
        };

        dataService.Insert(player);

        PlayerGrowthType growthType = GetRandomGrowthType(potential:grade);
        PlayerGrowthDetailType growthDetailType = GetRandomGrowthDetailType();

        int[] max_stats = GetStatsByGrade(grade, pitcherType, hand[0]);

        TuplePitcherStats pitcher = new TuplePitcherStats()
        {
            PlayerID = player.ID,
            TeamID = teamId,
            Age = age,
            Potential = grade,
            FAYear = 4,
            Energy = 100,
            Condition = 20,
            Fatigue = 0,
            Group = 2,

            Salary = 100,

            // Growth
            GrowthType = growthType,
            GrowthDetailType = growthDetailType,

            Stamina_MAX = max_stats[0],
            Velocity_MAX = max_stats[1],
            Stuff_MAX = max_stats[2],
            KMov_MAX = max_stats[3],
            GMov_MAX = max_stats[4],
            Control_MAX = max_stats[5],
            // Location = max_stats[6],
            Composure_MAX = max_stats[7],
            LOpp_MAX = max_stats[8],
            ROpp_MAX = max_stats[9]
        };

        float[] current_stats = new float[max_stats.Length];
        for(int i = 0; i < current_stats.Length; i++)
        {
            current_stats[i] = GetInitialStatValue(
                maxValue:max_stats[i],
                age:pitcher.Age,
                age_decay:pitcher.AgeDecay,
                grade:pitcher.Potential,
                growthType:pitcher.GrowthType
            );
        }

        pitcher.Stamina = current_stats[0];
        pitcher.Velocity = current_stats[1];
        pitcher.Stuff = current_stats[2];
        pitcher.KMov = current_stats[3];
        pitcher.GMov = current_stats[4];
        pitcher.Control = current_stats[5];
        // pitcher.Location = current_stats[6];
        pitcher.Composure = current_stats[7];
        pitcher.LOpp = current_stats[8];
        pitcher.ROpp = current_stats[9];

        if(pitcher.IsAgeDecaying) {
            pitcher.Stamina_MAX = current_stats[0];
            pitcher.Velocity_MAX = current_stats[1];
            pitcher.Stuff_MAX = current_stats[2];
            pitcher.KMov_MAX = current_stats[3];
            pitcher.GMov_MAX = current_stats[4];
            pitcher.Control_MAX = current_stats[5];
            // pitcher.Location = current_stats[6];
            pitcher.Composure_MAX = current_stats[7];
            pitcher.LOpp_MAX = current_stats[8];
            pitcher.ROpp_MAX = current_stats[9];
        }

        dataService.Insert(pitcher);

        if(isYoung) dataService.Insert(new TupleNewPlayer{PlayerId=player.ID});
        else if(isMercenary) dataService.Insert(new TupleMercenary{PlayerId=player.ID});

        dataService.Insert(new TuplePitcherSeason {Year = dataService.GetGameInfo().Year, TeamID = teamId, PlayerID = player.ID});
    }

    private int[] GetStatsByGrade(int grade, PitcherTypes pitcherType, Hands throwHand)
    {
        int[] result = new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };

        int[] totalGradesList;
        if (grade == 5) totalGradesList = new int[] { 20, 19, 18, 17 };
        else if (grade == 4) totalGradesList = new int[] { 16, 15, 14, 13 };
        else if (grade == 3) totalGradesList = new int[] { 12, 11, 10, 9 };
        else if (grade == 2) totalGradesList = new int[] { 8, 7, 6, 5};
        else totalGradesList = new int[] { 4, 3 };

        int totalGrade = totalGradesList[randomCreator.Next(0, totalGradesList.Length)];

        // 체력 관련
        int stamina;
        if (pitcherType == PitcherTypes.STARTING)
        {
            if (grade > 3) stamina = RandomSelected(new int[] { 15, 55, 100 }) + 3;
            else stamina = randomCreator.Next(3, 5);
        }
        else
        {
            stamina = randomCreator.Next(1, 4);
        }
        result[0] = stamina;

        // 구속 관련
        int velocity;
        if (grade >= 3) velocity = randomCreator.Next(2, 6);
        else
        {
            int maximumVelocity = (totalGrade > 5) ? 5 : totalGrade;
            velocity = randomCreator.Next(1, maximumVelocity + 1);
        }
        result[1] = velocity;
        totalGrade -= velocity - 1;

        // 좌우 관련
        int strongOpp, weakOpp;
        if (throwHand == Hands.LEFT) {
            strongOpp = 8;
            weakOpp = 9;
        }
        else
        {
            strongOpp = 9;
            weakOpp = 8;
        }
        result[strongOpp] = 3;
        int weakOppGrade = RandomSelected(new int[] { 40, 90, 100 });
        weakOppGrade = weakOppGrade > totalGrade ? totalGrade : weakOppGrade;
        result[weakOpp] = weakOppGrade + 1;
        totalGrade -= weakOppGrade;

        // 구위 삼진 땅볼 컨트 로케 침착
        int maxPoint = (grade > 4) ? 5 : 4;
        List<int> remainIndexList = new List<int> { 2, 3, 4, 5, 6, 7 };
        while (totalGrade > 0)
        {
            int remainIndex = remainIndexList[randomCreator.Next(0, remainIndexList.Count)];
            result[remainIndex] += 1;
            if (result[remainIndex] >= maxPoint)
            {
                remainIndexList.Remove(remainIndex);
            }
            totalGrade--;
        }

        // 능력치로 환산
        for (int i = 0; i < result.Length; i++) result[i] = GetExactStatByGrade(result[i]);

        return result;
    }

    public void CreateRandomPlayer()
    {

    }
}
