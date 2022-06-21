public class MatchSceneArguments : SceneArgumentsBase {
    public int TurnCount { get; private set; }
    public bool IsQuickMatch { get; private set; }

    public MatchSceneArguments(int turnCount = 1, bool isQuickMatch = false) {
        TurnCount = turnCount;
        IsQuickMatch = isQuickMatch;
    }
}
