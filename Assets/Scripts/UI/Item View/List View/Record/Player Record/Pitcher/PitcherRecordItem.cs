using UnityEngine;
using TMPro;

public class PitcherRecordItem : RecordItemBase {
    [SerializeField] TextMeshProUGUI _rankText, _textName, _textG, _textInning, _textERA, _textFIP, _textWHIP,
        _textGameStarted, _textCG, _textSHO, _textWin, _textLose, _textSave, _textR, _textER, _textSO,
        _textTBF, _textHit, _textHit2, _textHit3, _textHomerun, _textBB, _textIBB, _textHBP, _textWP, _textGB;

    public override void SetData(PlayerBase player, int rank = 1) {
        if(player is Pitcher) {
            _player = player;
            Pitcher pitcher = (Pitcher)_player;

            if (pitcher != null) {
                TuplePitcherSeason season = pitcher.Season;

                if(_teamLogoImage != null && _getLogoAction != null) {
                    _teamLogoImage.sprite = _getLogoAction(pitcher.Stats.TeamID);
                }
                _rankText.text = rank.ToString();

                _textName.text = pitcher.Base.Name;
                _textG.text = season.G.ToString();
                _textInning.text = Utils.doubleToString(season.IP, round:1);
                _textERA.text = Utils.doubleToString(season.ERA, round:2);
                _textFIP.text = Utils.doubleToString(season.FIP, round:2);
                _textWHIP.text = Utils.doubleToString(season.WHIP, round:2);
                _textGameStarted.text = season.GS.ToString();
                _textCG.text = season.CG.ToString();
                _textSHO.text = season.SHO.ToString();
                _textWin.text = season.W.ToString();
                _textLose.text = season.L.ToString();
                _textSave.text = season.SV.ToString();
                _textR.text = season.R.ToString();
                _textER.text = season.ER.ToString();
                _textSO.text = season.SO.ToString();
                _textTBF.text = season.BF.ToString();
                _textHit.text = season.H.ToString();
                _textHit2.text = season.H2.ToString();
                _textHit3.text = season.H3.ToString();
                _textHomerun.text = season.HR.ToString();
                _textBB.text = season.BB.ToString();
                _textIBB.text = season.IBB.ToString();
                _textHBP.text = season.HBP.ToString();
                _textWP.text = season.WP.ToString();
                _textGB.text = season.GBRate.ToString();

                if(_button != null) {
                    // buttonObj.onClick.AddListener(() => OpenSingleDialog());
                }
                SetActive(true);
            }
            else {
                SetActive(false);
            }
        }
        else {
            SetActive(false);
        }
    }
}
