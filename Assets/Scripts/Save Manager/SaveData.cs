public class SaveData {
    public string ManagerName {get; private set;}
    public int Year {get; private set;}
    public int Turn {get; private set;}

    public SaveData(string manager, int year, int turn) {
        ManagerName = manager;
        Year = year;
        Turn = turn;
    }
}
