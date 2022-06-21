using SQLite4Unity3d;

public class TupleLineupGroup2 : DBBase
{
    [PrimaryKey]
    public int PlayerID { get; set; }
    // public int RemainDays { get; set; }

    public int Year {get; set;}
    public int Turn {get; set;}

    public int RemainDays(int year, int turn)
    {
        if(year != Year) return 0;
        else return Turn + 10 - turn;
    }
}
