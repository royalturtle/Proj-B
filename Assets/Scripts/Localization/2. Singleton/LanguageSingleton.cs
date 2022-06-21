using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Threading;

public class LanguageSingleton {
    string FileName { get { return "lang - lang.tsv";}}
    string FilePath { get { return string.Format("{0}/{1}", Application.persistentDataPath , FileName);}}

    public List<string> SupportLanguageList {get; private set;}
    public int curLangIndex = 0;    // 현재 언어의 인덱스

    public Dictionary<LocalizationTypes, Dictionary<int, string>> Data {get; private set;}

    public event System.Action LocalizeChanged = () => { };
    public event System.Action LocalizeNameChanged = () => { };
    public event System.Action LocalizeSettingChanged = () => { };

    #region Singleton
    static LanguageSingleton _instance = null;
    public static LanguageSingleton Instance {
        get {
            if(_instance == null) {
                _instance = new LanguageSingleton();
            }
            return _instance;
        }
    }
    #endregion

    public string CurrentLanguage {
        get {
            string result = "English";
            if(curLangIndex == 1) {
                result = "한국어";
            }
            return result;
        }
    }

    LanguageSingleton() {
        Data = new Dictionary<LocalizationTypes, Dictionary<int, string>>();
        List<LocalizationTypes> langTypeList = Enum.GetValues(typeof(LocalizationTypes)).Cast<LocalizationTypes>().ToList();
        for(int i = 0; i < langTypeList.Count; i++) {
            Data.Add(langTypeList[i], new Dictionary<int, string>());
        }
        
        SupportLanguageList = new List<string>{"english", "korean"};

        FileUtils.DeletePersistentFile(FileName);
        FileUtils.StreammingAssetToPersistent(FileName);

        InitLang();
    }

    // InitLang 함수에서는 저장해놓은 언어 인덱스값이 있다면 가져오고 , 없다면 기본언어(영어)의 인덱스 값을 가져온다.
    public void InitLang() {
        int langIndex = PlayerPrefs.GetInt(GameConstants.PrefLanguage, -1);
        int systemIndex = SupportLanguageList.FindIndex(s => s.ToLower() == Application.systemLanguage.ToString().ToLower());
        if (systemIndex == -1) systemIndex = 0;
        int index = langIndex == -1 ? systemIndex : langIndex;

        SetLangIndex(index); // 값을 가져온 뒤 SetLangIndex의 매개변수로 넣어준다 
    }

    public void ChangeLangIndex(int index) {
        curLangIndex = index;   //initlang에서 구한 언어의 인덱스 값을 curLangIndex에 넣어줌 
        PlayerPrefs.SetInt(GameConstants.PrefLanguage, curLangIndex);  //저장
        SetLangList(curLangIndex);
        LocalizeChanged();  //텍스트들 현재 언어로 변경
        LocalizeNameChanged();
        LocalizeSettingChanged();   //드랍다운의 value변경
    }

    public void SetLangIndex(int index) {
        curLangIndex = index;   //initlang에서 구한 언어의 인덱스 값을 curLangIndex에 넣어줌 
        PlayerPrefs.SetInt(GameConstants.PrefLanguage, curLangIndex);  //저장
        LocalizeChanged();  //텍스트들 현재 언어로 변경
        LocalizeNameChanged();
        LocalizeSettingChanged();   //드랍다운의 value변경
    }
    
    [ContextMenu("언어 가져오기")]    //ContextMenu로 게임중이 아닐 때에도 실행 가능 
    public void GetLang() {
        SetLangList(curLangIndex);
        // foreach(LocalizationData item in Data.Values) { item.GetLangCo(curLangIndex); }
    }
    
    void SetLangList(int index) {
        FileInfo fileInfo = new FileInfo(FilePath);
        string tsv = "";

        if(fileInfo.Exists) {
            StreamReader reader = new StreamReader(FilePath);
            tsv = reader.ReadToEnd();
            reader.Close();
        }

        // 이차원 배열 생성
        string[] row = tsv.Split('\n'); //스페이스를 기준을 행 분류 
        int rowSize = row.Length;

        for(int i = 0; i < rowSize; i++) {
            string[] column = row[i].Split('\t');
            LocalizationTypes majorType = (LocalizationTypes)int.Parse(column[0]);
            int minorType = int.Parse(column[1]);
            string value = column[index+2];

            Dictionary<int, string> dict = Data[majorType];
            if(!dict.ContainsKey(minorType)) Data[majorType].Add(minorType, value);
            else Data[majorType][minorType] = value;
        }
    }

    public string GetGUI(int index) {
        return Instance.Data[LocalizationTypes.GUI][index].Replace("\\n", "\n");
    }

    public string GetWarning(int index) {
        return Instance.Data[LocalizationTypes.WARNING][index].Replace("\\n", "\n");
    }

    public string GetMatchScene(int index) {
        return Instance.Data[LocalizationTypes.MATCH_SCENE][index].Replace("\\n", "\n");
    }
    
    public string GetDialog(int index) {
        return Instance.Data[LocalizationTypes.DIALOG][index].Replace("\\n", "\n");
    }

    public string GetPlayer(int index) {
        return Instance.Data[LocalizationTypes.PLAYER][index].Replace("\\n", "\n");
    }

    public string GetHand(Hands hand) {
        return Instance.Data[LocalizationTypes.PLAYER][hand == Hands.LEFT ? 21 : 22].Replace("\\n", "\n");
    }

    public string GetBatterPosition(BatterPositionEnum position) {
        return Instance.Data[LocalizationTypes.BATTER_POSITION][(int)position].Replace("\\n", "\n");
    }

    public string GetBatterPosition(int position) {
        return Instance.Data[LocalizationTypes.BATTER_POSITION][position].Replace("\\n", "\n");
    }

    public string GetPitcherPosition(PitcherPositionEnum position) {
        return Instance.Data[LocalizationTypes.PITCHER_POSITION][(int)position].Replace("\\n", "\n");
    }

    public string GetPitcherPosition(int position) {
        return Instance.Data[LocalizationTypes.PITCHER_POSITION][position].Replace("\\n", "\n");
    }

    public string GetDayName(DaysEnum day) {
        return Instance.Data[LocalizationTypes.DATE][(int)day].Replace("\\n", "\n");
    }

    public string GetActionName(int index) {
        return Instance.Data[LocalizationTypes.ACTION_NAME][index].Replace("\\n", "\n");
    }

    public string GetErrorString(ErrorType errorType) {
        return Instance.Data[LocalizationTypes.ERROR_TYPE].ContainsKey((int)errorType) ?
            Instance.Data[LocalizationTypes.ERROR_TYPE][(int)errorType].Replace("\\n", "\n") : "";
    }
}
