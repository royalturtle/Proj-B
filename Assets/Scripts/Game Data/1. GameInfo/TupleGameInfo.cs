using SQLite4Unity3d;

public class TupleGameInfo : DBBase {
    [PrimaryKey, AutoIncrement]
    public int ID { get; set; }
    public int MyTeamIndex { get; set; }
    public int Year { get; set; }
    public int Turn { get; set; }
    public int SituationIndex { get; set; }
    public string OwnerName { get; set; }
    public int GameMajorVersion { get; set; }
    public NationTypes LeagueNation { get; set; }
    public int SeasonTurnStart { get; set; }
    public int SeasonTurnLast { get; set; }

    public int IsAutoSave {get; set;}
    public int SaveId {get; set;}

    public int Money {get; set;}
}
