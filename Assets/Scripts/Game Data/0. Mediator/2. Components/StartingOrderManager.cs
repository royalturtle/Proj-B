using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

public class StartingOrderManager
{
    /*
    GameDataService GameDB;

    public StartingOrderManager(GameDataService gameDB)
    {
        GameDB = gameDB;
    }

    int GetTeamStartingCounts(int teamId)
    {        
        Expression<Func<TupleLineupPitcher, bool>> restriction = t => (t.TeamID == teamId) && (t.Position =="STARTING");

        List<TupleLineupPitcher> search = GameDB.Select<TupleLineupPitcher>(restriction).ToList();
        int result = search.Count;

        return result;
    }

    public int GetTeamStartingOrder(int teamId)
    {
        Expression<Func<Team, bool>> restriction = t => (t.ID == teamId);
        IEnumerable<Team> searchResult = GameDB.Select<Team>(restriction);

        if (searchResult != null)
        {
            return searchResult.First().StartingOrder;
        }
        else return 1;
    }

    public void SetTeamStartingOrder(int teamId, int order=1, bool isNext=false)
    {
        int startingCount = GetTeamStartingCounts(teamId: teamId);
        Expression<Func<Team, bool>> restriction = t => (t.ID == teamId);
        IEnumerable<Team> searchResult = GameDB.Select<Team>(restriction);

        if (searchResult != null)
        {
            Team team = searchResult.First();
            int _order = order;
            if (isNext) _order = team.StartingOrder + 1;
            _order = (_order > startingCount) ? 1 : _order;
            _order = (_order < 1) ? 1 : _order;

            team.StartingOrder = _order;
            GameDB.UpdateData<Team>(team);
        }
    }
    */
}
