using UnityEngine;
using TMPro;

public class StatViewBase : MonoBehaviour {
    public virtual void SetData(PlayerBase player) {}

    protected void SetText(TextMeshProUGUI ui, string text) {
        if(ui) { ui.text = text; }
    }

    protected void SetText(TextMeshProUGUI ui, int value) {
        if(ui) { ui.text = value.ToString(); }
    }

    protected void SetText(TextMeshProUGUI ui, float value) {
        if(ui) { ui.text = ((int)value).ToString(); }
    }

    protected void SetText(TextMeshProUGUI ui, double value, bool isColor = false) {
        if(ui) { 
            ui.text = ((int)value).ToString(); 
            if(isColor) {
                ui.color = ColorConstants.Instance.GetColorOfStat(value);
            }
        }
    }
}
