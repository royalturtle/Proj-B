public class BaseSingleStatus {
    public Batter Runner { get; private set; }
    public Pitcher ResponsivePitcher { get; private set; }
    public bool IsByError { get; private set; }

    public BaseSingleStatus(Batter runner, Pitcher pitcher, bool isByError=false) {
        SetData(runner: runner, pitcher: pitcher, isByError: isByError);
    }

    public void SetIsByError(bool value) {
        IsByError = value;
    }

    public void SetData(Batter runner, Pitcher pitcher, bool isByError = false) {
        if(runner != null && pitcher != null) {
            Runner = runner;
            ResponsivePitcher = pitcher;
            IsByError = isByError;
        }
    }
}
