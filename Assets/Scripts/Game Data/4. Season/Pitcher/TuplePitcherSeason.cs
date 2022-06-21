using SQLite4Unity3d;

public class TuplePitcherSeason : DBBase {
    [PrimaryKey, AutoIncrement]
    public int ID { get; set; }

    public int Year { get; set; }
    public int TeamID { get; set; }

    public int PlayerID { get; set; }

    public int G { get; set; }              // ³ª¿Â °ÔÀÓ
    public int GS { get; set; }            // ¼±¹ß
    public int CG { get; set; }           // ¿ÏÅõ
    public int SHO { get; set; }               // ¿ÏºÀ

    public int OUT { get; set; }              // ¾Æ¿ô °¹¼ö   

    public int W { get; set; }
    public int L { get; set; }
    public int SV { get; set; }
    // public int Holds { get; set; }

    public int R { get; set; }             // ½ÇÁ¡
    public int ER { get; set; }      // ÀÚÃ¥

    public int BF { get; set; }

    public int H{  get; set; }
    public int H2 { get; set; }
    public int H3 { get; set; }
    public int HR { get; set; }

    public int BB { get; set; }           // 4º¼
    public int IBB { get; set; }        // °íÀÇ»ç±¸

    public int SO { get; set; }                     // »ïÁø
    public int HBP { get; set; }                    // »ç±¸
    public int WP { get; set; }                     // ÆøÅõ

    public int GB { get; set; }             // ¶¥º¼ °¹¼ö

    public double IP {
        get {
            return (OUT / 3) + (0.1 * (OUT % 3));
        }
    }

    public double ERA { 
        get { return (IP <= 0) ? 0 : (9.0 * ER) / IP; } 
    }

    public double WHIP {
        get { return (IP <= 0) ? 0 : (1.0 * H+ BB) / IP; }
    }

    public double FIP {
        get { 
            return (IP <= 0) ? 0 : 
                (13.0 * HR + 3.0 * (BB - IBB + HBP) - 2.0 * SO) / IP + 3.2; 
        }
    }

    public double GBRate {
        get {
            int below = BF - H- BB - HBP;
            return (below <= 0) ? 0 : (GB * 100.0) / (below * 1.0);
        }
    }
}
