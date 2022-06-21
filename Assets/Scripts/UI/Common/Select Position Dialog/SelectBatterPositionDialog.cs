public class SelectBatterPositionDialog : SelectPositionBaseDialog<BatterPositionEnum> {
    protected override BatterPositionEnum StartPosition {
        get { 
            return BatterPositionEnum.C; 
        }
    }

    protected override BatterPositionEnum EndPosition {
        get {
            return BatterPositionEnum.DH; 
        } 
    }
}
