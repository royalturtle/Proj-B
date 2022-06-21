public class TagUpStatus {
    public BatterPositionEnum Defense1 {get; private set;}
    public BatterPositionEnum Defense2 {get; private set;}
    public bool IsOut {get; private set;}

    public TagUpStatus(
        bool isOut,
        BatterPositionEnum defense1, 
        BatterPositionEnum defense2 = BatterPositionEnum.NONE) {
        Defense1 = defense1;
        Defense2 = defense2;
        IsOut = isOut;
    }
}
