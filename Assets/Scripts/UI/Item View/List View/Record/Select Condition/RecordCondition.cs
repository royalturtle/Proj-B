using System;

public class RecordCondition {
    public int Type {get; private set;}
    public NumberType NumberType {get; private set;}

    public double MinValue {get; private set;}
    public double MaxValue {get; private set;}

    public RecordCondition(
        int type, 
        NumberType numberType,
        double minValue = Int32.MinValue,
        double maxValue = Int32.MaxValue
    ) {
        Type = type;
        NumberType = numberType;
        
        MinValue = (minValue != Int32.MinValue)  ? minValue : Int32.MinValue;
        MaxValue = (maxValue != Double.MaxValue) ? maxValue : Int32.MaxValue;
    }

    public bool CheckValue(double value) {
        return MinValue <= value && value <= MaxValue;
    }
}
