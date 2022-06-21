using SQLite4Unity3d;

public class TupleName : DBBase {
    [PrimaryKey, AutoIncrement]
    public int ID { get; set; }
    public string English { get; set; }
    public string Korean { get; set; }
}

public class TupleNameKorea : TupleName { }
public class TupleNameUsa : TupleName { }
public class TupleNameJapan : TupleName { }

