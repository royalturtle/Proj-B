using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InGameTopBar : MonoBehaviour {
    [SerializeField] TextMeshProUGUI _managerText, _clubText, _moneyText, _dateText;
    [SerializeField] Image _clubImage;
    [SerializeField] DateView _date;

    public void Ready(GameDataMediator gameData) {
        if(gameData != null) {
            Team myTeam = gameData.Teams.MyTeam;
            SetText(_managerText, gameData.ManagerName);
            SetText(_clubText, myTeam.Name);
            SetText(_moneyText, gameData.Money.ToString());
            if(_clubImage) {
                _clubImage.sprite = ResourcesUtils.GetTeamIconImage(myTeam.LogoName);
            }
            UpdateDate(gameData);
        }
    }

    public void UpdateMoney(GameDataMediator gameData) {
        if(gameData != null) {
            SetText(_moneyText, gameData.Money.ToString());
        }
    }

    public void UpdateDate(GameDataMediator gameData) {
        if(gameData != null && _date != null) {
            _date.SetData(date: new DateObj(year: gameData.CurrentYear, turn: gameData.CurrentTurn));
        }
    }

    void SetText(TextMeshProUGUI ui, string text) {
        if(ui) {ui.text = text;}
    }
}
