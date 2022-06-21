using SQLite4Unity3d;

public class TupleMatch : DBBase {
    [PrimaryKey, AutoIncrement]
    public int ID { get; set; }
    public int Year { get; set; }
    public int Turn { get; set; }
    public int HomeTeamID { get; set; }
    public int AwayTeamID { get; set; }
    public int IsFinished { get; set; }
    public int HomeScore { get; set; }
    public int AwayScore { get; set; }
    public MatchTypes MatchType { get; set; }

    public PlayerStatusInMatch IsTeamIDExist(int id) {
        if (id == HomeTeamID) {
            return PlayerStatusInMatch.HOME;
        }
        else if(id == AwayTeamID) {
            return PlayerStatusInMatch.AWAY;
        }
        else {
            return PlayerStatusInMatch.NONE;
        }
    }

    public bool IsTeamContained(int id) {
        return HomeTeamID == id || AwayTeamID == id;
    }

    public bool IsEnded {get {return IsFinished != 0;}}

    public bool IsTeamDraw(int id) {
        return (!IsEnded) ? false : HomeScore == AwayScore;
    }

    public bool IsTeamWin(int id) {
        if(!IsEnded) {
            return false;
        }

        PlayerStatusInMatch state = IsTeamIDExist(id);

        // IF None Team
        if(state == PlayerStatusInMatch.NONE) {
            return false;
        }

        return ((HomeScore > AwayScore && state == PlayerStatusInMatch.HOME)) ||
            ((HomeScore < AwayScore && state == PlayerStatusInMatch.AWAY));
    }
}
