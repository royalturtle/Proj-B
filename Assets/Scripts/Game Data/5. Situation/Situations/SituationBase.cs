
public class SituationBase
{
    public bool IsBeforeMatch { get; private set; }
    public int Turn {get; private set; }
    public SituationTypes SituationType { get; private set; }

    public SituationBase(int turn, SituationTypes situationType, bool isBeforeMatch=true)
    {
        Turn = turn;
        SituationType = situationType;
        IsBeforeMatch = isBeforeMatch;
    }
}
