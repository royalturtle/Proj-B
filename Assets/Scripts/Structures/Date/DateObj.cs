public class DateObj {
    public int Turn { get; private set; }
    public int Year { get; private set; }
    public int Month { get; private set; }
    public int Day { get; private set; }

    public DaysEnum DayName { get; private set; }

    // public int DayNameLangIndex { get { return 13 + (int)DayName; } }

    public DateObj(int year, int turn) {
        Year = VerifyYear(year:year);
        Turn = VerifyTurn(year:Year, turn:turn);
        SetMonthDaysByYearTurn(year: Year, turn: turn);
    }

    public DateObj(int year, int month, int day=1, bool isEndMonth=false) {
        Year = VerifyYear(year:year);
        Month = VerifyMonth(month:month);

        if(!isEndMonth) { Day = VerifyDay(year:Year, month:Month, day:day); }
        else { Day = GetDaysInMonth(year: Year, month: Month); }

        Turn = GetTurnByDate(year: Year, month: Month, day: Day);
        DayName = GetDaysNameByYearTurn(year: Year, turn: Turn);
    }

    public DateObj(int year, int month, DaysEnum dayName, int week) {
        Year = VerifyYear(year:year);
        Month = VerifyMonth(month:month);

        Turn = 0;
        for (int i = 1; i < Month; i++) { Turn += GetDaysInMonth(year: Year, month: i); }

        int MaxTurn = Turn + GetDaysInMonth(year: Year, month: Month);
        
        for (int i = 0; i < 7; i++) {
            DayName = GetDaysNameByYearTurn(year: Year, turn: Turn);
            if (DayName == dayName) { break; }
            else { Turn++; }
        }

        for(int i = 1; i < week; i++) {
            if (Turn + 7 > MaxTurn) { break; }
            else { Turn += 7; }
        }

        SetMonthDaysByYearTurn(year: Year, turn: Turn, isSetMonth:false, isSetDayName:false);
    }

    void SetMonthDaysByYearTurn(int year, int turn, bool isSetMonth=true, bool isSetDay=true, bool isSetDayName=true) {
        int _turn = VerifyTurn(year: year, turn: turn);
        int count = 0;

        for(int m = 1; m <= 12; m++) {
            count += GetDaysInMonth(year:year, month:m);
            // if (count > turn)
            if (count > turn - 1) {
                if (isSetMonth) { Month = m; }
                // if (isSetDay) Day = (m > 1) ? turn - (count - GetDaysInMonth(year:year, month:m)) + 1: turn;
                if (isSetDay) { Day = turn - (count - GetDaysInMonth(year: year, month: m)); }
                if (isSetDayName) { DayName = GetDaysNameByYearTurn(year:year, turn:turn); }
                break;
            } 
        }
    }

    int GetTurnByDate(int year, int month, int day) {
        int _year = VerifyYear(year: year);
        int _month = VerifyMonth(month: month);
        int _day = VerifyDay(year: year, month: month, day: day);

        int result = 0;
        for (int i = 1; i < _month; i++) { result += GetDaysInMonth(year: _year, month: i); }

        result += day;
        return result;
    }

    DaysEnum GetDaysNameByYearTurn(int year, int turn) {
        int _year = VerifyYear(year: year);
        int _turn = VerifyTurn(year: year, turn: turn);

        return (DaysEnum)(((int)FirstDayName(year: _year) + (_turn - 1)) % 7);
    }

    DaysEnum FirstDayName(int year) {
        return (DaysEnum)((year - 1 + year / 4 - year / 100 + year / 400) % 7);
    }

    static bool isLeapYear(int year) {
        return year % 4 == 0 && (year % 100 != 0 || year % 400 == 0);
    }

    static int VerifyDay(int year, int month, int day) {
        return Utils.GetSafeIndex(
            value: day,
            count: GetDaysInMonth(year: year, month: month),
            min: 1
        );
    }

    static int VerifyMonth(int month) {
        return Utils.GetSafeIndex(value:month, count:12, min:1);
    }

    static int VerifyYear(int year) {
        return (year < 1) ? 0 : year;
    }

    static int VerifyTurn(int year, int turn) {
        return Utils.GetSafeIndex(
            value: turn,
            count: isLeapYear(year: VerifyYear(year: year)) ? 366 : 365,
            min : 1
        );
    }

    public static int GetDaysInMonth(int year, int month) {
        switch(VerifyMonth(month: month)) {
            case 1 : return 31;
            case 2 : return isLeapYear(year) ? 29 : 28;
            case 3 : return 31;
            case 4 : return 30;
            case 5 : return 31;
            case 6 : return 30;
            case 7 : return 31;
            case 8 : return 31;
            case 9 : return 30;
            case 10: return 31;
            case 11: return 30;
            case 12: return 31;
            default: return 30;
        }
    }
    #region ToString

    public string MonthString2 {
        get { return ((Month < 10) ? "0" : "") + Month.ToString(); }
    }

    public string DayString2 {
        get { return ((Day < 10) ? "0" : "") + Day.ToString(); }
    }

    public string YYYYMMDDString {
        get { return Year.ToString() + MonthString2 + DayString2; }
    }
    
    public string MMaDDString {
        get { return MonthString2 + "/" + DayString2; }
    }

    public string YYYYAMMADDString {
        get { return Year.ToString() + "/" + MonthString2 + "/" + DayString2; }
    }

    public string YYYYAMADDString {
        get { return Year.ToString() + "/" + Month.ToString() + "/" + DayString2; }
    }

    #endregion ToString
}

