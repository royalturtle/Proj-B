public class MatchInfo {
    public PlayerStatusInMatch PlayerStatus { get; private set; }
    public TupleMatch MatchData { get; private set; }
    public Team HomeTeam {get; private set;}
    public Team AwayTeam {get; private set;}

    public MatchInfo(
        PlayerStatusInMatch playerStatus,
        TupleMatch matchData,
        Team homeTeam,
        Team awayTeam
    ) {
        PlayerStatus = playerStatus;
        MatchData = matchData;
        HomeTeam = homeTeam;
        AwayTeam = awayTeam;
    }

    public Team GetTeam(PlayerStatusInMatch status, bool isOpp = false) {
        switch(status) {
            case PlayerStatusInMatch.HOME: return !isOpp ? HomeTeam : AwayTeam;
            case PlayerStatusInMatch.AWAY: return !isOpp ? AwayTeam : HomeTeam;
            default: return null;
        }
    }

    public Team GetOppTeam() {
        switch(PlayerStatus) {
            case PlayerStatusInMatch.HOME: return AwayTeam;
            case PlayerStatusInMatch.AWAY: return HomeTeam;
            default: return null;
        }
    }
}
