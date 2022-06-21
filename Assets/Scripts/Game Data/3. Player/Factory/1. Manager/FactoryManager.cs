using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryManager {
    System.Random randomCreator;
    BatterFactory batterFactory;
    PitcherFactory _pitcherFactory;
    NationTypes _leagueType;

    const int START_PITCHER_COUNT = 17;
    const int START_CATCHER_COUNT = 4;
    const int START_INFIELDER_COUNT = 9;
    const int START_OUTFIELDER_COUNT = 7;

    const int BASE_GRADES_SUM = 19;
    const int DEFAULT_GRADES_SUM = 27;
    
    public FactoryManager(NationTypes leagueType) {
        randomCreator = new System.Random();
        _leagueType = leagueType;

        batterFactory = new BatterFactory();
        _pitcherFactory = new PitcherFactory();
    }

    public void CreateFirstPlayers(GameDataService dataService, double startPercentage = 0.0, double endPercentage = 0.0) {
        int teamCount=0;
        int[] batterSubsGrades;
        int[] elsePitcherGrades;

        switch (_leagueType) {
            case NationTypes.USA:
                teamCount = 30;
                batterSubsGrades = new int[] { 2, 2, 2, 2, 2, 1, 1, 1, 1, 1 };
                elsePitcherGrades = new int[] { 2, 2, 2, 2, 1, 1, 1, 1, 1, 1 };
                break;
            case NationTypes.JAPAN:
                teamCount = 12;
                batterSubsGrades = new int[] { 2, 2, 2, 2, 1, 1, 1, 1, 1 };
                elsePitcherGrades = new int[] { 2, 2, 2, 1, 1, 1, 1, 1, 1 };
                break;
            default:
                teamCount = 10;
                batterSubsGrades = new int[] { 2, 2, 2, 2, 1, 1, 1, 1, 1 };
                elsePitcherGrades = new int[] { 2, 2, 2, 1, 1, 1, 1, 1, 1 };
                break;
        }

        // �� ���� ����ġ�� ������
        int[] teamStrengthList = GetTeamStrength();
        
        double unitPercentage = (endPercentage - startPercentage) / (teamCount * 2.0);

        for (int i = 1; i <= teamCount; i++) {
            // ���� ������ ����
            int teamStrength = teamStrengthList[i - 1];

            #region Batter
            // Ÿ�� ����
            int[] batterMainGrades = CreateGrades(teamStrength:teamStrength);

            int[] positionCount = new int[] { 1, 1, 1, 1, 1, 3 };
            int[] positionProbability = new int[] { 3, 5, 1, 4, 1, 6 };
            int[] cumPosProb = new int[] { 0, 0, 0, 0, 0, 0 };

            // DH �÷���
            positionCount[randomCreator.Next(0, positionCount.Length)]++;

            #region Main
            for (int j = 0; j < batterMainGrades.Length; j++) {
                int count = 0;
                for(int k = 0; k < positionProbability.Length; k++) {
                    count += positionProbability[k];
                    cumPosProb[k] = count;
                }

                int random = randomCreator.Next(1, count + 1);
                int posIndex = 0;

                while(random > cumPosProb[posIndex]) posIndex++;

                // ���� ����
                batterFactory.CreateTestPlayer(
                    dataService     : dataService,
                    teamId          : i,
                    nationType      : _leagueType,
                    defensePosition : (DefensePosition)posIndex,
                    grade           : batterMainGrades[j]);

                // ���� ������ �����Ű��
                positionCount[posIndex]--;
                if(positionCount[posIndex] <= 0) {
                    positionProbability[posIndex] = 0;
                }
                // positionProbability[posIndex] = (positionCount[posIndex] <= 0) ? 0 : positionProbability[posIndex] - 2;
            }
            #endregion

            #region Subs
            // ���� �߰��� �������� ���� ������
            List<DefensePosition> subsPositionList = new List<DefensePosition>{
                DefensePosition.C, DefensePosition.B1, DefensePosition.B2, DefensePosition.B3, DefensePosition.SS, DefensePosition.OF, DefensePosition.OF, DefensePosition.OF
            };
            
            // SUB���� Defense List�� Grade List���� ������ �����ϰ� ä�����´�.
            while(subsPositionList.Count < batterSubsGrades.Length) {
                subsPositionList.Add((DefensePosition)randomCreator.Next(0, Enum.GetNames(typeof(DefensePosition)).Length));
            }
            subsPositionList = Utils.ShuffleList<DefensePosition>(subsPositionList);

            // SUB �߼� ����
            for (int j = 0; j < batterSubsGrades.Length; j++) {
                batterFactory.CreateTestPlayer(
                    dataService     : dataService, 
                    teamId          : i, 
                    nationType      : _leagueType, 
                    defensePosition : subsPositionList[j], 
                    grade           : batterSubsGrades[j]);
            }
            #endregion
            startPercentage += unitPercentage;
            TransitionObject.SetPercentage(startPercentage);
            #endregion

            #region Pitcher
            // ���� ����
            int[] pitcherGrades = CreateGrades(teamStrength:teamStrength);

            // [TODO] ����, �߰����� ����
            int starting = 9;
            int[] pitcherTypePercentage = new int[] { 80, 100 };
            PitcherTypes _pitcherType;
            for (int j = 0; j < pitcherGrades.Length; j++) {
                if ((starting > 0) && (RandomSelected(pitcherTypePercentage) == 0)) {
                    _pitcherType = PitcherTypes.STARTING;
                    starting--;
                }
                else {
                    _pitcherType = PitcherTypes.RELIEF;
                }

                _pitcherFactory.CreateTestPlayer(
                    dataService, 
                    i, 
                    _leagueType, 
                    pitcherType : _pitcherType, 
                    grade       : pitcherGrades[j]);
            }
    
            // ��Ÿ ���� ����
            for (int j = 0; j < elsePitcherGrades.Length; j++) {
                if ((starting > 0) && (RandomSelected(pitcherTypePercentage) == 0)) {
                    _pitcherType = PitcherTypes.STARTING;
                    starting--;
                }
                else {
                    _pitcherType = PitcherTypes.RELIEF;
                }
                _pitcherFactory.CreateTestPlayer(
                    dataService, 
                    i, 
                    _leagueType, 
                    pitcherType : _pitcherType, 
                    grade       : elsePitcherGrades[j]);
            }
            startPercentage += unitPercentage;
            TransitionObject.SetPercentage(startPercentage);
            #endregion
        }
    }

    int[] CreateGrades(int teamStrength) {
        int[] result = new int[] { 3, 3, 3, 3, 3, 3, 3, 3, 3 };
        
        result = AdaptTeamStrengths(result, teamStrength);
        result = ShakeGradesList(result);
        Array.Sort(result);
        Array.Reverse(result);

        return result;
    }

    int TeamCount {
        get {
            switch(_leagueType) {
                case NationTypes.USA: return 30;
                case NationTypes.JAPAN: return 12;
                default: return 10;
            }
        }
    }

    #region ����
    public void CreateYoungPlayers(GameDataService dataService) {
        int[] gradeProbabilities = new int[] { 20, 60, 90,  110, 120 },
              batterPositionProb = new int[] { 10, 30, 45, 60, 70, 100};

        int startingPositionProb = 80,
            teamCount = TeamCount,
            playerCount = 5,
            totalBatters = teamCount * playerCount + randomCreator.Next(-10, 11),
            totalPitchers = teamCount * playerCount - totalBatters;

        for(int i = 0; i < totalBatters; i++) {
            int grade = GetIndex(prob:gradeProbabilities) + 1;      // grade ���
            DefensePosition batterPosition = (DefensePosition)GetIndex(prob:batterPositionProb); // position ����

            batterFactory.CreateTestPlayer(
                dataService, 
                0, 
                _leagueType, 
                batterPosition, 
                grade: grade, 
                isYoung:true);
        }

        for(int i = 0; i < totalPitchers; i++) {
            int grade = GetIndex(prob:gradeProbabilities) + 1;      // grade ���
            PitcherTypes pitcherType = (randomCreator.Next(0,100) >= startingPositionProb) ? PitcherTypes.STARTING : PitcherTypes.RELIEF;

            _pitcherFactory.CreateTestPlayer(
                dataService, 
                0, 
                _leagueType, 
                pitcherType : pitcherType, 
                grade       : grade, 
                isYoung     :true);
        }
    }

    int GetIndex(int[] prob) {
        int result, randomValue = randomCreator.Next(1, prob[prob.Length - 1] + 1);
        for(result = 0; result < prob.Length; result++) {
            if(randomValue <= prob[result]) {
                break;
            }
        }
            
        return result;
    }

    #endregion ����

    #region �뺴
    public void CreateMercenaryPlayers(GameDataService dataService) {
        int[] gradeProbabilities = new int[] { 10, 30, 70,  100, 120 };
        int[] batterPositionProb = new int[] { 10, 30, 45, 60, 70, 100};
        int startingPositionProb = 80;
        
        int playerCount = 5;

        int teamCount;
        NationTypes nation = NationTypes.USA;
        if(_leagueType == NationTypes.USA)        teamCount = 0;
        else if(_leagueType == NationTypes.JAPAN) teamCount = 12;
        else teamCount = 10;

        // Batter
        int totalBatters = teamCount * playerCount + randomCreator.Next(-10, 11);

        for(int i = 0; i < totalBatters; i++) {
            int grade = GetIndex(prob:gradeProbabilities) + 1;      // grade ���
            DefensePosition batterPosition = (DefensePosition)GetIndex(prob:batterPositionProb); // position ����

            batterFactory.CreateTestPlayer(
                dataService, 
                0, 
                nation, 
                batterPosition, 
                grade: grade, 
                isMercenary:true);
        }

        // Pitcher
        int totalPitchers = teamCount * playerCount - totalBatters;

        for(int i = 0; i < totalPitchers; i++) {
            int grade = GetIndex(prob:gradeProbabilities) + 1;      // grade ���
            PitcherTypes pitcherType = (randomCreator.Next(0,100) >= startingPositionProb) ? PitcherTypes.STARTING : PitcherTypes.RELIEF;

            _pitcherFactory.CreateTestPlayer(
                dataService, 
                0, 
                nation, 
                pitcherType: pitcherType, 
                grade: grade, 
                isMercenary:true);
        }
    }
    #endregion �뺴

    #region ���Ǽ���

    int GetGradesSumByStrength(int strength) {
        return BASE_GRADES_SUM + strength * 2;
    }

    int[] AdaptTeamStrengths(int[] gradeList, int teamStrength) {
        int diff = DEFAULT_GRADES_SUM - GetGradesSumByStrength(teamStrength);
        int addDirection = (diff > 0) ? 1 : -1;
        
        for (int i = 0; i < diff*addDirection; i++) {
            gradeList[i] += addDirection;
        }
        return gradeList;
    }

    #endregion ���Ǽ���

    // Grade List�� ������ ���������.
    int[] ShakeGradesList(int[] gradeList) {
        // list�� ������ item���� �� ���� üũ
        for(int from = 0; from < gradeList.Length; from++) {
            // 50% Ȯ���� ��ȭ�� �ش�.
            int random = randomCreator.Next(0, 4);
            if(random >= 2) {
                int to = randomCreator.Next(0, 8);
                to = (to >= from) ? to + 1 : to;

                // from�� ���
                // if(random == 2 && gradeList[from] < 5 && gradeList[to] > 2)
                if(random == 3) {
                    if(gradeList[from] < 5 && gradeList[to] > 2) {
                        gradeList[from]++;
                        gradeList[to]--;
                    }
                }
                // from�� �ϰ�
                else if(gradeList[from] > 2 && gradeList[to] < 5) {
                    gradeList[from]--;
                    gradeList[to]++;
                }
            }
        }
        return gradeList;
    }

    int[] GetTeamStrength() {
        int randomValue;
        switch (_leagueType) {
            case NationTypes.USA:
                return null;
            case NationTypes.JAPAN:
                return null;
            default:
                randomValue = randomCreator.Next(0, 9);
                switch(randomValue) {
                    case 0 : return new int[] { 1, 2, 2, 5, 5, 5, 5, 5, 5, 5 };
                    case 1 : return new int[] { 1, 2, 3, 3, 4, 4, 5, 6, 6, 6 };
                    case 2 : return new int[] { 2, 2, 3, 3, 4, 4, 5, 5, 5, 7 };
                    case 3 : return new int[] { 1, 2, 3, 4, 4, 4, 5, 5, 6, 6 };
                    case 4 : return new int[] { 1, 3, 3, 3, 4, 4, 4, 5, 6, 7 };
                    case 5 : return new int[] { 1, 3, 3, 3, 4, 4, 5, 5, 6, 6 };
                    case 6 : return new int[] { 2, 2, 3, 3, 3, 4, 4, 5, 7, 7 };
                    case 7 : return new int[] { 1, 2, 2, 3, 4, 5, 5, 5, 6, 7 };
                    default: return new int[] { 1, 1, 4, 4, 4, 5, 5, 5, 5, 6 };
                }
        }
    }

    private int RandomSelected(int[] seeds) {
        int random = randomCreator.Next(1, seeds[seeds.Length - 1] + 1), index = 0;
        while (seeds[index] < random) {
            index++;
        }

        return index;
    }
}
