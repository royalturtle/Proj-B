using SQLite4Unity3d;

public class TupleBatterStats : TupleLivePlayer {
    public double Hit { get; set; }
    public double Eye { get; set; }
    public double Power { get; set; }
    public double GapPower { get; set; }
    public double AvoidK { get; set; }
    public double Speed { get; set; }
    public double DefenseC { get; set; }
    public double Defense1B { get; set; }
    public double Defense2B { get; set; }
    public double Defense3B { get; set; }
    public double DefenseSS { get; set; }
    public double DefenseOF { get; set; }

    public int Push { get; set; }

    public double Hit_MAX { get; set; }
    public double Eye_MAX { get; set; }
    public double Power_MAX { get; set; }
    public double GapPower_MAX { get; set; }
    public double AvoidK_MAX { get; set; }
    public double Speed_MAX { get; set; }
    public double DefenseC_MAX { get; set; }
    public double Defense1B_MAX { get; set; }
    public double Defense2B_MAX { get; set; }
    public double Defense3B_MAX { get; set; }
    public double DefenseSS_MAX { get; set; }
    public double DefenseOF_MAX { get; set; }

    public double Attack {
        get { return Hit + GapPower + Eye + Power + AvoidK; }
    }

    public double LeadOffAttacks {
        get { return Hit + Eye + AvoidK; }
    }

    public BatterPositionEnum MainDefense {
        get {
            BatterPositionEnum position = BatterPositionEnum.C;
            BatterPositionEnum result = position;
            double maxValue = GetDefenseAbility(position);

            while(++position < BatterPositionEnum.CF) {
                double newValue = GetDefenseAbility(position);
                if(newValue > maxValue) {
                    maxValue = newValue;
                    result = position;
                }
            }
            return result;
        }
    }

    public static int StatCount() { return 12; }

    public void SetStatByIndex(int index, double value) {
        switch(index) {
            case 0 : Hit = value; break;
            case 1 : Eye = value; break;
            case 2 : Power = value; break;
            case 3 : GapPower = value; break;
            case 4 : AvoidK = value; break;
            case 5 : Speed = value; break;
            case 6 : DefenseC = value; break;
            case 7 : Defense1B = value; break;
            case 8 : Defense2B = value; break;
            case 9 : Defense3B = value; break;
            case 10 : DefenseSS = value; break;
            case 11 : DefenseOF = value; break;
        }
    }

    public void SetMaxStatByIndex(int index, double value) {
        switch(index) {
            case 0 : Hit_MAX = value; break;
            case 1 : Eye_MAX = value; break;
            case 2 : Power_MAX = value; break;
            case 3 : GapPower_MAX = value; break;
            case 4 : AvoidK_MAX = value; break;
            case 5 : Speed_MAX = value; break;
            case 6 : DefenseC_MAX = value; break;
            case 7 : Defense1B_MAX = value; break;
            case 8 : Defense2B_MAX = value; break;
            case 9 : Defense3B_MAX = value; break;
            case 10 : DefenseSS_MAX = value; break;
            case 11 : DefenseOF_MAX = value; break;
        }
    }

    public double GetDefenseAbility(BatterPositionEnum position) {
        switch(position) {
            case BatterPositionEnum.C: return DefenseC;
            case BatterPositionEnum.B1: return Defense1B;
            case BatterPositionEnum.B2: return Defense2B;
            case BatterPositionEnum.B3: return Defense3B;
            case BatterPositionEnum.SS: return DefenseSS;
            case BatterPositionEnum.LF: return DefenseOF;
            case BatterPositionEnum.CF: return DefenseOF;
            case BatterPositionEnum.RF: return DefenseOF;
            default: return 0;
        }
    }
}
