using System;
using System.Linq;
using System.Linq.Expressions;
using static LanguageSingleton;
using UnityEngine;

// public class NameDataService : DataService {
class NameDataService : DataService {
    public NameDataService() : base(isClear:true, isCopy:true) {}

    protected override string FileName {get {return GameConstants.DB_NAMES;}}

    public void ChangeNameLocalize(TuplePlayerBase player) {
        if(player.Nation == NationTypes.USA) {
            ChangeByLangIndex<TupleNameUsa>(player);
        }
        else if (player.Nation == NationTypes.JAPAN) {
            ChangeByLangIndex<TupleNameJapan>(player);
        }
        else {
            ChangeByLangIndex<TupleNameKorea>(player);
        }
    }

    void ChangeByLangIndex<T>(TuplePlayerBase player) where T: TupleName, new() {
        int langIndex = Instance.curLangIndex;
        string result = "";
        Expression<Func<T, bool>> restriction = n => n.ID == player.NameIndex;
        var nameData = Select(restriction);

        // English
        if(langIndex == 0) {
            result = nameData.First().English;
        }
        // 한국어
        else if(langIndex == 1) {
            result = nameData.First().Korean;
        }
        /*
        // 일본어
        else
        {

        }*/
        player.Name = result;
    }
}
