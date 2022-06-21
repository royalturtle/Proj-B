using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using SQLite4Unity3d;
using UnityEngine;
using System.Collections;
using System.IO;
using System.Threading.Tasks;

public abstract class DataService {
    protected SQLiteConnection _connection;

    public DataService(bool isClear=false, bool isCopy = false) {
        // Delete Existed the File
        if (isClear && File.Exists(FilePath)) {
            File.Delete(FilePath);
        }

        // Create the File
        if(!File.Exists(FilePath)) {
            if (isCopy)  {
                FileUtils.StreammingAssetToPersistent(FileName);
            }
            else {
                _connection = new SQLiteConnection(FilePath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
                CreateNewDB();
                _connection.Close();
            }
        }
    }

    ~DataService() {
        if(_connection != null) {
            _connection.Close();
        }
    }

    protected void Connect() {
        _connection = new SQLiteConnection(FilePath, SQLiteOpenFlags.ReadWrite);
    }

    protected void Close() {
        _connection.Close();
    }

    protected virtual void CreateNewDB() {}

    protected abstract string FileName {get;}
    protected string FilePath {
        get { return string.Format("{0}/{1}", GameConstants.DATA_PERSISTENT_PATH, FileName); }
    }
    public bool IsFileExist { get { return FileUtils.IsExist(GameConstants.DATA_PERSISTENT_PATH, FileName); } }

    #region Table

    public void CreateTable<T>() where T : DBBase {
        Connect();
        _connection.CreateTable<T>();
        Close();
    }

    public void DropTable<T>() where T : DBBase {
        Connect();
        _connection.DropTable<T>();
        Close();
    }

    #endregion

    #region Row(Record)
    public int Insert<T>(T t) where T : DBBase {
        Connect();
        int result = _connection.Insert(t);
        Close();
        return result;
    }

    public int InsertAll<T>(T[] objects) where T : DBBase {
        Connect();
        int result = _connection.InsertAll(objects);
        Close();
        return result;
    }

    public int Delete<T>(T t) where T : DBBase {
        Connect();
        int result = _connection.Delete(t);
        Close();
        return result;
    }

    public int DeleteOne<T>(int pk) where T : DBBase, new() {
        Connect();
        T t = Select<T>(pk:pk);
        int result = _connection.Delete(t);
        Close();
        return result;
    }
    
    public int DeleteAll<T>(T[] objects) where T : DBBase {
        Connect();
        int result = 0;
        foreach(T t in objects) {
            result = _connection.Delete(t);
        }
        Close();
        return result;
    }
    
    public int DeleteAll<T>(List<T> objects) where T : DBBase {
        Connect();
        int result = 0;
        foreach(T t in objects) {
            result = _connection.Delete(t);
        }
        Close();
        return result;
    }

    public List<T> Select<T>(Expression<Func<T, bool>> restriction = null) where T : DBBase, new() {
        Connect();
        IEnumerable<T> search = restriction != null ? _connection.Table<T>().Where(restriction) : _connection.Table<T>();
        List<T> result = search.ToList();
        Close();
        return result;
    }

    public T Select<T>(int pk) where T : DBBase, new() {
        Connect();
        T result = _connection.Get<T>(pk);
        Close();
        return result;
    }

    public T SelectOne<T>(Func<T, bool> predicate) where T : DBBase, new() {
        Connect();
        T result = _connection.Get<T>(predicate);
        Close();
        return result;
    }

    public void UpdateData<T>(T data) where T : DBBase {
        Connect();
        _connection.RunInTransaction(() => {
            _connection.Update(data);
        });
        Close();
    }

    public void UpdateAll<T>(List<T> data) where T : DBBase {
        Connect();
        _connection.RunInTransaction(() => {
            foreach(T d in data) {
                _connection.Update(d);
            }
        });
        Close();
    }

    #endregion

    public void Execute(string value) {
        Connect();
        var result = _connection.Execute(value);
        Debug.Log(result);
        Close();
    }
}
