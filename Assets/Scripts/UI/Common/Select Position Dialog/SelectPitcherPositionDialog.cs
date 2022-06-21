public class SelectPitcherPositionDialog : SelectPositionBaseDialog<PitcherPositionEnum> {
    protected override PitcherPositionEnum StartPosition {
        get { 
            return PitcherPositionEnum.STARTING; 
        }
    }

    protected override PitcherPositionEnum EndPosition {
        get {
            return PitcherPositionEnum.CLOSER; 
        } 
    }
}