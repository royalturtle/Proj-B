using SQLite4Unity3d;

public class TupleNewPlayer : DBBase
{
    [PrimaryKey]
    public int PlayerId { get; set; }
}
