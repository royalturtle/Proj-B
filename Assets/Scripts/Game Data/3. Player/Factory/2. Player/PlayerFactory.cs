using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFactory {
    protected System.Random randomCreator;
    static readonly int GRADE_COUNT = 5, STAT_MIN = 35, STAT_RANGE = 100 - STAT_MIN, STAT_UNIT = STAT_RANGE / GRADE_COUNT;

    public PlayerFactory() {
        randomCreator = new System.Random();
        #region 나이
        foreach (int i in ageProbabilityDistribution) agePDSum += i;
        #endregion 나이
    }

    public virtual void CreaetMercenary() { }
    public virtual void CreaetYoungPlayer() { }

    protected int GetRandomNameIndex(NationTypes nationType) {
        int result = 1;

        if(nationType == NationTypes.USA) {
            result = randomCreator.Next(1, GameConstants.NAME_USA_COUNT);
        }
        else if(nationType == NationTypes.JAPAN) {
            result = randomCreator.Next(1, GameConstants.NAME_JAPAN_COUNT);
        }
        else {
            result = randomCreator.Next(1, GameConstants.NAME_KOREA_COUNT);
        }
        return result;
    }

    #region 나이
    int[] ageProbabilityDistribution = new int[] { 30, 25, 24, 23, 22, 22, 21, 21, 20, 20, 19, 18, 17, 17, 16, 16, 12, 10, 8, 5, 3, 1 };
    int agePDSum = 0;

    // protected int GetRandomBirthYearAtStart()
    protected int GetRandomAge(bool isYoung=false, bool isMercenary=false) {
        // 나이 정보
        int ageRnd = randomCreator.Next(1, agePDSum + 1);
        int age = GameConstants.BASE_AGE;

        if(!isYoung) {
            int count = ageProbabilityDistribution[0];
        
            while (count < ageRnd) {
                age++;
                count += ageProbabilityDistribution[age - GameConstants.BASE_AGE];
            }
            if(isMercenary && age <= 25) {
                age += (age - GameConstants.BASE_AGE);
            }
        }
        // 대졸 계산
        else {
            int randomValue = randomCreator.Next(0, 5);
            if(randomValue == 4) { age += 4; }
        }

        // return GameConstants.GAME_START_YEAR - age;
        return age;
    }

    #endregion 나이

    protected Hands[] GetRandomHands(DefensePosition defensePosition = DefensePosition.OF) {
        Hands[] result = new Hands[2];

        if((defensePosition == DefensePosition.OF) && (RandomSelected(new int[] { 40, 100 }) == 0)) result[0] = Hands.LEFT;
        else result[0] = Hands.RIGHT;

        if ((RandomSelected(new int[] { 25, 100 }) == 0)) result[1] = Hands.LEFT;
        else result[1] = Hands.RIGHT;

        return result;
    }

    protected int RandomSelected(int[] seeds)
    {
        int random = randomCreator.Next(1, seeds[seeds.Length - 1] + 1), index = 0;
        while (seeds[index] < random)
        {
            index++;
        }

        return index;
    }

    protected int GetExactStatByGrade(int grade)
    {
        return STAT_MIN + (grade - 1) * STAT_UNIT + randomCreator.Next(1, STAT_UNIT + 1);
    }

    protected PlayerGrowthType GetRandomGrowthType(int potential)
    {
        PlayerGrowthType result = PlayerGrowthType.NORMAL;
        int random;

        switch(potential)
        {
            case 5:
                random = randomCreator.Next(0, 2);
                if(random == 0) result = PlayerGrowthType.GENIUS;
                else result = PlayerGrowthType.ENLIGHT;
                break;
            case 4:
                random = randomCreator.Next(0, 4);
                if(random == 0) result = PlayerGrowthType.GENIUS;
                else if(random == 1) result = PlayerGrowthType.ENLIGHT;
                else result = PlayerGrowthType.NORMAL;
                break;
            case 3:
                random = randomCreator.Next(0, 4);
                if(random == 0) result = PlayerGrowthType.BUBBLE;
                else if(random == 1) result = PlayerGrowthType.ENLIGHT;
                else result = PlayerGrowthType.NORMAL;
                break;
            case 2:
                random = randomCreator.Next(0, 4);
                if(random == 0) result = PlayerGrowthType.BUBBLE;
                else if(random == 1) result = PlayerGrowthType.LIMITED;
                else result = PlayerGrowthType.NORMAL;
                break;
            default:
                random = randomCreator.Next(0, 2);
                if(random == 0) result = PlayerGrowthType.LIMITED;
                else result = PlayerGrowthType.NORMAL;
                break;
        }
        return result;
    }

    protected float GetInitialStatValue(
        int maxValue,
        int age,
        int age_decay,
        int grade,
        PlayerGrowthType growthType)
    {
        float result = 0.0f;

        int max_percentage = 0, min_percentage=0;

        // 신인
        int youngAge = 24;
        if(age <= youngAge) {
            int percMin, percMax;
            switch(growthType) {
                case PlayerGrowthType.LIMITED:
                    percMin = 75; percMax = 85; break;
                case PlayerGrowthType.GENIUS:
                    percMin = 90; percMax = 100; break;
                case PlayerGrowthType.ENLIGHT:
                    percMin = 70; percMax = 75; break;
                case PlayerGrowthType.BUBBLE:
                    percMin = 85; percMax = 100; break;
                default:
                // case PlayerGrowthType.NORMAL:
                    percMin = 75; percMax = 85; break;
            }
            min_percentage = percMin - (youngAge - age);
            max_percentage = percMax - (youngAge - age);
        }
        // 중간
        else if(age < age_decay) {
            max_percentage = 100;
            switch(growthType) {
                case PlayerGrowthType.LIMITED:
                    min_percentage = 75; break;
                case PlayerGrowthType.GENIUS:
                    min_percentage = 95; break;
                case PlayerGrowthType.ENLIGHT:
                    min_percentage = 75; break;
                case PlayerGrowthType.BUBBLE:
                    min_percentage = 85; break;
                // case PlayerGrowthType.NORMAL:
                default:
                    min_percentage = 85; break;
            }
        }
        // 나중 선수
        else {
            int percMin, percMax;
            int ageDiff = age - age_decay;
            switch(growthType) {
                case PlayerGrowthType.LIMITED:
                    percMin = 80; percMax = 90; break;
                case PlayerGrowthType.GENIUS:
                    percMin = 90; percMax = 100; break;
                case PlayerGrowthType.ENLIGHT:
                    percMin = 80; percMax = 95; break;
                case PlayerGrowthType.BUBBLE:
                    percMin = 75; percMax = 85; break;
                // case PlayerGrowthType.NORMAL:
                default:
                    percMin = 85; percMax = 95; break;
            }

            min_percentage = percMin - ageDiff;
            max_percentage = percMax - ageDiff;
        }
        int min_base = 30;
        min_percentage = (min_percentage <  min_base) ? min_base : min_percentage;
        max_percentage = (max_percentage <  min_base) ? min_base : max_percentage;

        int random = randomCreator.Next(min_percentage, max_percentage + 1);
        maxValue = Mathf.Clamp(maxValue, 0, 100);
        result = STAT_MIN + ((maxValue - STAT_MIN) * (random / 100.0f));
        result = Mathf.Clamp(result, 0.0f, 100.0f);
        // if(result > 100.0f ||result < 0.0f ) Debug.Log("UPUP" + result.ToString());
        return result;
    }

    protected PlayerGrowthDetailType GetRandomGrowthDetailType()
    {
        return (PlayerGrowthDetailType)randomCreator.Next(0, 10);
    }
}