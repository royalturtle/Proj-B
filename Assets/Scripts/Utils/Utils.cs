using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils {
    public static int GetLoopableIndex<T>(List<T> data, int index) {
        int result = GameConstants.NULL_INT;
        if(data != null && data.Count > 0) {
            if(index < 0) {
                result = data.Count - 1;
            }
            else if(index >= data.Count) {
                result = 0;
            }
            else {
                result = index;
            }
        }
        return result;
    }

    public static int GetSafeIndex(int value, int count, int min = 0) {
        return (value < min || count < 1) ? min : 
            ((value >= (min + count)) ? min + count - 1 : value);
    }

    public static bool Int2Bool(int value) {
        return value != 0;
    }

    public static int Bool2Int(bool value) {
        return value ? 1 : 0;
    }

    public static float Float2Sound(float value) {
        return Mathf.Log10(value) * 20;
    }

    public static bool IsValidIndex<T>(List<T> data, int index) {
        bool result = false;
        if(data == null) {
            Debug.Log("Invalid Index : Data is null.");
        }
        else if(index < 0 || index >= data.Count) {
            Debug.Log("Invalid Index : " + index.ToString() + " / " + data.Count.ToString());
        }
        else {
            result = true;
        }
        return result;
    }

    public static bool IsBetween<T>(this T value, T minValue, T maxValue) {
        return Comparer<T>.Default.Compare(value, minValue) >= 0
            && Comparer<T>.Default.Compare(value, maxValue) <= 0;
    }

    public static List<T> ShuffleList<T>(List<T> list) {
        System.Random random = new System.Random();
        int random1, random2;
        T temp;

        for (int i = 0; i < list.Count; ++i) {
            // random1 = UnityEngine.Random.Range(0, list.Count);
            // random2 = UnityEngine.Random.Range(0, list.Count);
            random1 = random.Next(0, list.Count);
            random2 = random.Next(0, list.Count);

            temp = list[random1];
            list[random1] = list[random2];
            list[random2] = temp;
        }

        return list;
    }

	public static string floatToString(float value, int round) {
		string result = System.Math.Round(value, round).ToString();
		int dot = result.IndexOf(".");
		int loop;
		if(dot < 0) {
			result += ".";
			loop = round;
		}
		else {
			loop = round - (result.Length - dot - 1);
		}
		
		for(int i = 0; i < loop; i++) {
			result += "0";
		}
		
		return result;
	}

    public static string doubleToString(double value, int round) {
		string result = System.Math.Round(value, round).ToString();
		int dot = result.IndexOf(".");
		int loop;
		if(dot < 0) {
			result += ".";
			loop = round;
		}
		else {
			loop = round - (result.Length - dot - 1);
		}
		
		for(int i = 0; i < loop; i++) {
			result += "0";
		}
		
		return result;
    }

    public static bool NotNull<A>(A data) {
        bool result = data != null; 
        if(!result) {
            Debug.Log("Value is null");
        }
        return result;
    }

    public static bool NotNull<A,B>(A a, B b) {
        return NotNull(a) && NotNull(b);
    }

    public static bool NotNull<A,B,C>(A a, B b, C c) {
        return NotNull(a) && NotNull(b) && NotNull(c);
    }

    public static bool NotNull<A,B,C,D>(A a, B b, C c, D d) {
        return NotNull(a) && NotNull(b) && NotNull(c) && NotNull(d);
    }

    public static bool NotNull<A,B,C,D,E>(A a, B b, C c, D d, E e) {
        return NotNull(a) && NotNull(b) && NotNull(c) && NotNull(d) && NotNull(e);
    }

    public static bool NotNull<A,B,C,D,E,F>(A a, B b, C c, D d, E e, F f) {
        return NotNull(a) && NotNull(b) && NotNull(c) && NotNull(d) && NotNull(e) && NotNull(f);
    }

    public static string GetStringOfGrade(int grade) {
        switch(grade) {
            case 5 : return "S";
            case 4 : return "A";
            case 3 : return "B";
            case 2 : return "C";
            default : return "D";
        }
    }

    public static void CheckList<T>(List<T> list, int index) {
        Debug.Log("List : " + list.Count + " / " + index.ToString());
    }

    public static void CheckArray<T>(T[] array, int index) {
        Debug.Log("Array : " + array.Length + " / " + index.ToString());
    }
}