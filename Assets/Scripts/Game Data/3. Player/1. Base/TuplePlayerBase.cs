using SQLite4Unity3d;

public class TuplePlayerBase : DBBase {
    [PrimaryKey, AutoIncrement, Indexed]
    public int ID { get; set; }
    public int NameIndex { get; set; }
    public string Name { get; set; }
    public NationTypes Nation { get; set; }
    public int BirthYear { get; set; }
    // public int Potential { get; set; }
    public int IsRetired { get; set; }

    public Hands ThrowHand {get; set;}
    public Hands HitHand {get; set;}
}
