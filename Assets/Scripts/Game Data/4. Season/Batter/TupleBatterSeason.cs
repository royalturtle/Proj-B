using SQLite4Unity3d;


public class TupleBatterSeason : DBBase {
    [PrimaryKey, AutoIncrement]
    public int ID { get; set; }
    public int Year { get; set; }
    public int TeamID { get; set; }

    public int PlayerID { get; set; }

    public int G { get; set; }         // ����
    public int PA { get; set; }   // Ÿ��
    // Ÿ��
    public int AB { 
        get {
            return PA - BB - IBB - HBP - SH - SF;
        }
    }

    public int R { get; set; }         // ����
    public int H { get; set; }               // ��Ÿ
    public int H2 { get; set; }              // 2��Ÿ
    public int H3 { get; set; }              // 3��Ÿ

    public int HR { get; set; }
    public int RBI { get; set; }                // Ÿ��

    public int SB { get; set; }        // ����
    public int CS { get; set; }     // ���� ����

    public int BB { get; set; }                 // ����
    public int SO { get; set; }                 // ��Ʈ����ũ �ƿ�

    public int GIDP { get; set; }                // ����Ÿ
    public int HBP { get; set; }                // ���� �´� ��

    public int SH { get; set; }                 // �����Ʈ
    public int SF { get; set; }                 // ��� �ö���
    public int IBB { get; set; }                // ���� �籸

    public int E { get; set; }              // ����

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
