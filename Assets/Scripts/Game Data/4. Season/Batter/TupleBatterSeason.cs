using SQLite4Unity3d;


public class TupleBatterSeason : DBBase {
    [PrimaryKey, AutoIncrement]
    public int ID { get; set; }
    public int Year { get; set; }
    public int TeamID { get; set; }

    public int PlayerID { get; set; }

    public int G { get; set; }         // 경기수
    public int PA { get; set; }   // 타석
    // 타수
    public int AB { 
        get {
            return PA - BB - IBB - HBP - SH - SF;
        }
    }

    public int R { get; set; }         // 득점
    public int H { get; set; }               // 안타
    public int H2 { get; set; }              // 2루타
    public int H3 { get; set; }              // 3루타

    public int HR { get; set; }
    public int RBI { get; set; }                // 타점

    public int SB { get; set; }        // 도루
    public int CS { get; set; }     // 도루 실패

    public int BB { get; set; }                 // 볼넷
    public int SO { get; set; }                 // 스트라이크 아웃

    public int GIDP { get; set; }                // 병살타
    public int HBP { get; set; }                // 몸에 맞는 공

    public int SH { get; set; }                 // 희생번트
    public int SF { get; set; }                 // 희생 플라이
    public int IBB { get; set; }                // 고의 사구

    public int E { get; set; }              // 에러

    public float AVG {
        get { return (AB <= 0) ? 0 : (H * 1.0f) / (AB * 1.0f); }
    }
    
    public float OBP {
        get {
            int below = AB + BB + HBP + SF;
            return (below <= 0) ? 0 : ((H + BB + HBP) * 1.0f) / (below * 1.0f);
        }
    }

    public float SLG {
        get {
            return (AB <= 0) ? 0 : ((H + H2 + H3 * 2 + HR * 3) * 1.0f) / (AB * 1.0f);
        }
    }

    public float OPS {
        get {
            return OBP + SLG;
        }
    }

    /*
    public float wOBA
    {
        get
        {
            int below = PlateAppearances - IBB;
            return (below <= 0) ? 0 : (0.72 * (BB - IBB) 
                + 0.75 * HBP
                + 0.9 *);
        }
    }*/
    public float wOBA { get { return 10.0f; } }
}
