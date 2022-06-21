using UnityEngine;
using TMPro;

public class BatterRecordItem : RecordItemBase {
    [SerializeField]
    TextMeshProUGUI _textNum, _textName, _textPosition, _textG, _textPA, _textAB, _textR, _textRBI,
        _textHit, _textHit2, _textHit3, _textHomerun, _textAVG, _textOBP, _textSLG, _textOPS, _textwOBA,
        _textBB, _textHBP, _textIBB, _textSO, _textGIDP, _textSH, _textSF, _textSB, _textCS, _textE;

    protected override void AwakeAfter() {
        _isBatter = true;
    }

    public override void SetData(PlayerBase player, int rank = 1) {
        if(player is Batter) {
            _player = player;
            Batter batter = (Batter)_player;
            if(batter != null) {
                if(_teamLogoImage != null && _getLogoAction != null) {
                    _teamLogoImage.sprite = _getLogoAction(batter.Stats.TeamID);
                }
                _textNum.text = rank.ToString();
                _textName.text = batter.Base.Name;

                _textG.text = batter.Season.G.ToString();
                _textPA.text = batter.Season.PA.ToString();
                _textAB.text = batter.Season.AB.ToString();
                _textR.text = batter.Season.R.ToString();
                _textRBI.text = batter.Season.RBI.ToString();

                _textHit.text = batter.Season.H.ToString();
                _textHit2.text = batter.Season.H2.ToString();
                _textHit3.text = batter.Season.H3.ToString();
                _textHomerun.text = batter.Season.HR.ToString();
                _textAVG.text = Utils.doubleToString(batter.Season.AVG, round:3);
                _textOBP.text = Utils.doubleToString(batter.Season.OBP, round:3);
                _textSLG.text = Utils.doubleToString(batter.Season.SLG, round:3);
                _textOPS.text = Utils.doubleToString(batter.Season.OPS, round:3);
                _textwOBA.text = batter.Season.wOBA.ToString();

                _textBB.text = batter.Season.BB.ToString();
                _textHBP.text = batter.Season.HBP.ToString();
                _textIBB.text = batter.Season.IBB.ToString();
                _textSO.text = batter.Season.SO.ToString();
                _textGIDP.text = batter.Season.GIDP.ToString();
                _textSH.text = batter.Season.SH.ToString();
                _textSF.text = batter.Season.SF.ToString();
                _textSB.text = batter.Season.SB.ToString();
                _textCS.text = batter.Season.CS.ToString();
                _textE.text = batter.Season.E.ToString();
            
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
