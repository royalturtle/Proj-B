using SQLite4Unity3d;

public class TupleLivePlayer : DBBase {
    [PrimaryKey, Indexed]
    public int PlayerID { get; set; }
    public int TeamID { get; set; }
    public int Age { get; set; }
    public int FAYear { get; set; }
    public double Energy { get; set; }
    public double Condition { get; set; }
    public int Potential { get; set; }

    public int Group { get; set; }
    public double Fatigue { get; set; }

    public int Salary {get; set;}

    public PlayerGrowthType GrowthType {get; set;}
    public float Exp {get; set;}
    public PlayerGrowthDetailType GrowthDetailType {get; set;}

    public bool IsAgeDecaying {
        get {return Age > AgeDecay;}
    }

    public int AgeDecay  {
        get {
            int result = 35 - (5 - Potential) * 2;
            switch(GrowthDetailType) {
                case PlayerGrowthDetailType.EARLY_EARLY:
                case PlayerGrowthDetailType.ZERO_EARLY:
                case PlayerGrowthDetailType.LATE_EARLY:
                    result--;
                    break;
                case PlayerGrowthDetailType.EARLY_LATE:
                case PlayerGrowthDetailType.ZERO_LATE:
                case PlayerGrowthDetailType.LATE_LATE:
                    result++;
                    break;
            }
            return result;
        }
    }

    public int AgeGrowSlow {
        get {
            return 1;
        }
    }
}
