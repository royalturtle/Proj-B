using UnityEngine;
using TMPro;

public class TeamSingleRecordBase : MonoBehaviour {
    public virtual void SetData(TeamSeason team) {}

    protected void SetText(TextMeshProUGUI ui, string text, int maxLength = -1) {
        if(ui) { 
            // ui.text = CheckMaxLength(text:text, maxLength:maxLength);
            ui.text = text;
        }
    }

    protected void SetText(TextMeshProUGUI ui, int value, int maxLength = -1) {
        if(ui) { 
            // ui.text = CheckMaxLength(text:value.ToString(), maxLength:maxLength);
            ui.text = value.ToString();
        }
    }

    protected void SetText(TextMeshProUGUI ui, float value, int maxLength = -1) {
        if(ui) { 
            // ui.text = CheckMaxLength(text:((int)value).ToString(), maxLength:maxLength);
            ui.text = ((int)value).ToString();
        }
    }

    string CheckMaxLength(string text, int maxLength) {
        string result = text;
        if(maxLength > 0) {
            while(result.Length < maxLength) {
                result = " " + result;
            }
        }
        return result;
    }
}
