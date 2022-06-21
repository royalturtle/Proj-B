using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using UnityEngine;
using System.IO;
using SQLite4Unity3d;
using System;

public class SaveManager {
    public static List<SaveData> GetSaveList() {
        List<SaveData> result = new List<SaveData>();

        for(int i = 1; i <= GameConstants.SAVE_DATA_COUNT; i++) {
            result.Add(GetSaveData(i));
        }
        return result;
    }

    public static bool IsSaveExist(int index) {
        return File.Exists(GetFilePath(index));
    }

    public static bool IsCurrentSaveExist() {
        return File.Exists(string.Format("{0}/{1}", Application.persistentDataPath, GameConstants.DB_NOWGAME));
    }

    public static TupleGameInfo GetGameInfo(int index) {
        SQLiteConnection connection;
        TupleGameInfo result = null;
        string fileName = GetFilePath(index);
        if (File.Exists(fileName)) {
            connection = new SQLiteConnection(fileName, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
            if(connection != null) {
                IEnumerable<TupleGameInfo> search = connection.Table<TupleGameInfo>();
                List<TupleGameInfo> searchResult = search.ToList();
                if(searchResult != null && searchResult.Count > 0) {
                    result = searchResult[0];
                }
                connection.Close();
            }        
        }
        return result;
    }

    public static SaveData GetSaveData(int index) {
        TupleGameInfo tuple = GetGameInfo(index);
        return tuple == null ? null : new SaveData(manager:tuple.OwnerName, year:tuple.Year, turn:tuple.Turn);
    }

    public static bool SaveData(int index) {
        bool result = false;
        if(CheckValidSaveIndex(index)) {
            result = true;
            FileUtils.CopyFileInPersistent(
                fileNameFrom : GameConstants.DB_NOWGAME,
                fileNameTo   : GameConstants.DB_SAVEDATA(index)
            );
        }
        return result;
    }

    public static bool LoadData(int index) {
        bool result = false;
        if(CheckValidSaveIndex(index)) {
            result = true;
            FileUtils.CopyFileInPersistent(
                fileNameFrom : GameConstants.DB_SAVEDATA(index),
                fileNameTo   : GameConstants.DB_NOWGAME
            );
            
        }
        return result;
    }

    public static bool DeleteData(int index) {
        bool result = false;
        if(CheckValidSaveIndex(index)) {
            result = true;
            FileUtils.DeletePersistentFile(GameConstants.DB_SAVEDATA(index));
        }
        return result;
    }

    public static void DeleteCurrentData() {
        FileUtils.DeletePersistentFile(GameConstants.DB_NOWGAME);
    }

    public static string GetFilePath(int index) {
        return string.Format("{0}/{1}", Application.persistentDataPath, GameConstants.DB_SAVEDATA(index));
    }

    
    static bool CheckValidSaveIndex(int index) {
        return 1 <= index && index <= GameConstants.SAVE_DATA_COUNT;
    }
}
