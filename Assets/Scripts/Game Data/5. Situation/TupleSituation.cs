using SQLite4Unity3d;

public class TupleSituation : DBBase {
    public int Turn { get; set; }
    public string Type { get; set; }
    public bool IsBeforeMatch { get; set; }
}
