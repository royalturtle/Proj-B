using UnityEngine;
using UnityEngine.SceneManagement;

public class NotDestroyObject : MonoBehaviour {
    public GameDataMediator GameData {get; private set;}
    bool IsStart;
    public MatchSceneArguments MatchArguments { get; private set; }
    [field:SerializeField] public TransitionObject Transition {get; private set;}
    
    void Start() {
        StartAction();
    }

    public void StartAction() {
        if(!IsStart) {
            GameConstants.DATA_PERSISTENT_PATH =  Application.persistentDataPath;
            GameConstants.DATA_PATH = Application.dataPath;
            DontDestroyOnLoad(this.gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
            BetterStreamingAssets.Initialize();
            LanguageSingleton.Instance.GetLang();
            LanguageSingleton.Instance.InitLang();
            
            // CreateInfoText(GameConstants.FileTermsConditions);
            // CreateInfoText(GameConstants.FilePrivacyPolicy);
            // CreateInfoText(GameConstants.FileAttribution);
            IsStart = true;
        }
    }

    void CreateInfoText(string fileName) {
        FileUtils.DeletePersistentFile(fileName);
        FileUtils.StreammingAssetToPersistent(fileName);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        /*
        if(scene.buildIndex == GameConstants.SCENE_START) {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            Destroy(this.gameObject);
        }*/
    }

    public void CreateNewGame(string teamName, string ownerName) {
        GameData = new GameDataMediator(isCreate:true, teamName:teamName, ownerName:ownerName);
    }

    public void LoadCurrentGame() {
        GameData = new GameDataMediator();
    }

    public void SetSceneArguments(MatchSceneArguments matchArguments = null) {
        MatchArguments = matchArguments;
    }

    public void ClearSceneArguments() {
        MatchArguments = null;
    }
    
    void CheckPlayerPrefs() {
        PlayerPrefs.SetFloat(GameConstants.PREF_BGM_SOUND, PlayerPrefs.GetFloat(GameConstants.PREF_BGM_SOUND, 0.5f));
        PlayerPrefs.SetInt(GameConstants.PREF_BGM_IS_ON, PlayerPrefs.GetInt(GameConstants.PREF_BGM_IS_ON, 1));
        PlayerPrefs.SetFloat(GameConstants.PREF_SFX_SOUND, PlayerPrefs.GetFloat(GameConstants.PREF_SFX_SOUND, 0.5f));
        PlayerPrefs.SetInt(GameConstants.PREF_SFX_IS_ON, PlayerPrefs.GetInt(GameConstants.PREF_SFX_IS_ON, 1));
    }
}
