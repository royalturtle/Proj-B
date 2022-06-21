using SQLite4Unity3d;

public class Team : DBBase {
    [PrimaryKey, AutoIncrement]
    public int ID { get; set; }
    public string Name { get; set; }
    public string LogoName { get; set; }
    public int Div1 { get; set; }
    public int Div2 { get; set; }
    public int StartingOrder { get; set; }
}
