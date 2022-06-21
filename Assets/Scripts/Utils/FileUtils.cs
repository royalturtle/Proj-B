using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FileUtils {
    public static void DeletePersistentFile(string fileName)
    {
        string filepath = string.Format("{0}/{1}", GameConstants.DATA_PERSISTENT_PATH, fileName);
        if (File.Exists(filepath)) File.Delete(filepath);
    }

    public static string StreammingAssetToPersistent(string fileName)
    {
        string filepath = string.Format("{0}/{1}", GameConstants.DATA_PERSISTENT_PATH, fileName);
        if (!File.Exists(filepath))
        {
            Debug.Log("Database not in Persistent path");
            // if it doesn't ->
            // open StreamingAssets directory and load the db ->
#if UNITY_EDITOR
            var loadDb = GameConstants.DATA_PATH + "/StreamingAssets/" + fileName;
            File.Copy(loadDb, filepath);
#elif UNITY_ANDROID
            byte[] loadDb = BetterStreamingAssets.ReadAllBytes(fileName);
            File.WriteAllBytes(filepath, loadDb);
#elif UNITY_IOS
            var loadDb = GameConstants.DATA_PATH + "/Raw/" + fileName;  // this is the path to your StreamingAssets in iOS
            // then save to Application.persistentDataPath
            File.Copy(loadDb, filepath);
#elif UNITY_WP8
            var loadDb = GameConstants.DATA_PATH + "/StreamingAssets/" + fileName;  // this is the path to your StreamingAssets in iOS
            // then save to Application.persistentDataPath
            File.Copy(loadDb, filepath);

#elif UNITY_WINRT
            var loadDb = GameConstants.DATA_PATH + "/StreamingAssets/" + fileName;  // this is the path to your StreamingAssets in iOS
            // then save to Application.persistentDataPath
            File.Copy(loadDb, filepath);
#elif UNITY_STANDALONE_OSX
            var loadDb = GameConstants.DATA_PATH + "/Resources/Data/StreamingAssets/" + fileName;  // this is the path to your StreamingAssets in iOS
            // then save to Application.persistentDataPath
            File.Copy(loadDb, filepath);
#else
            var loadDb = GameConstants.DATA_PATH + "/StreamingAssets/" + fileName;  // this is the path to your StreamingAssets in iOS
                                                                                                   // then save to Application.persistentDataPath
            File.Copy(loadDb, filepath);
#endif
        }
        return filepath;
    }

    public static void CopyFileInPersistent(string fileNameFrom, string fileNameTo)
    {
        string from = string.Format("{0}/{1}", GameConstants.DATA_PERSISTENT_PATH, fileNameFrom);
        string to = string.Format("{0}/{1}", GameConstants.DATA_PERSISTENT_PATH, fileNameTo);

        if (File.Exists(to)) File.Delete(to);
        File.Copy(sourceFileName:from, destFileName:to);
    }

    public static void CreateDirectoryInPersistent(List<string> folderList)
    {
        string path = GameConstants.DATA_PERSISTENT_PATH;

        foreach(string name in folderList)
        {
            string newPath = string.Format("{0}/{1}", path, name);
            if(!Directory.Exists(newPath))
            {
                Directory.CreateDirectory(newPath);
            }
            path = newPath;
        }
    }

    public static void CreateDirectoryInPersistent(string folder)
    {
        string newPath = string.Format("{0}/{1}", GameConstants.DATA_PERSISTENT_PATH, folder);
        if(!Directory.Exists(newPath))
        {
            Directory.CreateDirectory(newPath);
        }
    }
    
    public static bool IsExist(string pathName, string fileName)
    {
        string name = string.Format("{0}/{1}", pathName, fileName);
        return File.Exists(name);
    }
}
