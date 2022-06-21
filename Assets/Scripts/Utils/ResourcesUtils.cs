using UnityEngine;
using UnityEngine.UI;

public static class ResourcesUtils {
    public static Sprite GetTeamIconImage(string name) {
        return Resources.Load<Sprite>("Images/TeamLogo/" + name);
    }
}
