using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorConstants {
    public Color32 White {get; private set;}
    public Color32 Green {get; private set;}
    public Color32 Red {get; private set;}

    public Color32 PlayerBackgroundNormal {get; private set;}
    public Color32 PlayerBackgroundSelected {get; private set;}

    public Color32 MainLabelColorMain {get; private set;}
    public Color32 MainLabelColorSub {get; private set;}
    public Color32 MainLabelColorGroup2 {get; private set;}

    List<Color32> StatColorList;

    static ColorConstants _instance = null;
    public static ColorConstants Instance {
        get {
            if(_instance == null) {
                _instance = new ColorConstants();
            }
            return _instance;
        }
    }

    ColorConstants() {
        White = new Color32(255, 255, 255, 255);
        Green = new Color32(145, 219, 105, 255);
        Red = new Color32(245, 125, 74, 255);

        PlayerBackgroundNormal = new Color32(79, 79, 79, 255);
        PlayerBackgroundSelected = new Color32(78, 183, 98, 255);

        MainLabelColorMain   = new Color32(77, 155, 230, 255);
        MainLabelColorSub    = new Color32(107, 62, 117, 255);
        MainLabelColorGroup2 = new Color32(26, 26, 26, 255);

        StatColorList = new List<Color32> {
            new Color32(254, 255, 134, 255),
            new Color32(245, 125, 74, 255),
            new Color32(195, 36, 84, 255)
        };
    }

    public Color32 GetColorOfStat(double value) {
        Color32 result = White;
        for(int i = 0; i < StatColorList.Count; i++) {
            if(value >= 90.0 - i * 10.0) {
                result = StatColorList[StatColorList.Count - 1 - i];
                break;
            }
        }
        return result;
    }

    public Color32 GetColorOfGrade(int value) {
        switch(value) {
            case 5 :
            case 4 :
            case 3 :
                return StatColorList[value - 3];
            case 2 : 
                return Green;
            default: 
                return White;
        }
    }
}
