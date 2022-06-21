using UnityEngine;

public static class GameConstants {
    public static string DATA_PERSISTENT_PATH = "";
    public static string DATA_PATH="";

    public readonly static string TAG_NOT_DESTROY_OBJECT = "NotDestroyObject";
    public readonly static string TAG_BALL = "Ball";
    
    public static readonly string PREF_BGM_IS_ON = "BGMIsOn";
    public static readonly string PREF_BGM_SOUND = "BGMSound";
    public static readonly string PREF_SFX_IS_ON = "SFXIsOn";
    public static readonly string PREF_SFX_SOUND = "SFXSound";
    public static readonly string PrefLanguage = "LangIndex";

    /*
    public static readonly string FileTermsConditions = "TermsConditions.txt";
    public static readonly string FilePrivacyPolicy = "PrivacyPolicy.txt";
    public static readonly string FileAttribution = "Attribution.txt";
    */
   
    public static readonly int NULL_INT = -1;

    public static readonly string DB_NOWGAME = "nowgame.db";
    public static string DB_SAVEDATA(int i) { return "save" + i.ToString() + ".db"; }
    public static readonly int SAVE_DATA_COUNT = 5;
    public static readonly string DB_NAMES = "names.db";

    public static readonly double BASEBALL_FIP_C = 3.2;

    // Check
    public static readonly int FALSE_INT = 0;
    public static readonly int TRUE_INT = 1;

    public static readonly int NAME_KOREA_COUNT = 4881;
    public static readonly int NAME_USA_COUNT = 300;
    public static readonly int NAME_JAPAN_COUNT = 0;

    public static readonly string FILE_MATCH_KOREA = "MatchKOREA.tsv";

    public static readonly int BASEBALL_ENTRY_1_NUM = 26;

    public static readonly int GAME_START_YEAR = 2022;
    public static readonly int BASE_AGE = 19;

    // Settings
    public static readonly int RESOURCE_LOGO_COUNT = 18;

    public static readonly int START_MONEY = 30000;

    public static readonly int BATTER_POSITION_STRING_START = 31;

    public static int Group1RegisterLimitation(NationTypes nation, int year = -1, int turn = -1) {
        DateObj date = null;
        if(year != GameConstants.NULL_INT && turn != GameConstants.NULL_INT) {
            date = new DateObj(year:year, turn:turn);
        }
        bool isExpansionEntry = date != null && date.Month >= 9;

        switch(nation) {
            case NationTypes.KOREA: 
                return isExpansionEntry ? 33 : 28;
            case NationTypes.USA: 
                return isExpansionEntry ? 28 : 26;
            case NationTypes.JAPAN:
                return 29;
            default:
                return 26;
        }
    }

    public static int Group1PitcherLimitation(NationTypes nation) {
        switch(nation) {
            case NationTypes.USA:
                return 13;
            default:
                return 200;
        }
    }

    public static int Group1PlayingLimitation(NationTypes nation, int year = -1, int turn = -1) {
        DateObj date = null;
        if(year != GameConstants.NULL_INT && turn != GameConstants.NULL_INT) {
            date = new DateObj(year:year, turn:turn);
        }

        bool isExpansionEntry = date != null && date.Month >= 9;

        switch(nation) {
            case NationTypes.KOREA: 
                return isExpansionEntry ? 31 : 26;
            case NationTypes.JAPAN:
                return 25;
            default:
                return Group1RegisterLimitation(nation:nation, year:year, turn:turn);
        }
    }

    public static int MercenaryLimitation(NationTypes nation) {
        switch(nation) {
            case NationTypes.KOREA:
                return 3;
            case NationTypes.JAPAN:
                return 4;
            default:
                return 0;
        }
    }
}
