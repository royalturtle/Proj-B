using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerIndividualView<T> : FullPanel where T : PlayerBase {
    protected TeamDataMediator TeamData;
    protected T _player;

    [SerializeField] protected Image _clubImage;
    [SerializeField] protected PlayerReferenceView<T> _teamView, _recordView;
    [SerializeField] protected IndividualBasicStatusView _basicView;

    protected int _playerIndex = -1;
    protected bool _isRecord;

    public void Ready(TeamDataMediator teamData) {
        TeamData = teamData;
    }

    public virtual void SetData(int index = -1) {}
    public void SetData(T player, bool isRecord) {
        _isRecord = isRecord;
        _player = player;
        SetData();
    }

    public void SetTeamData(int teamId) {
        if(TeamData != null) {
            if(_clubImage) {
                _clubImage.sprite = TeamData.GetLogo(teamId);
            }
        }
    }

    public void SetSiblingData(bool isNext) {
        PlayerBase player = null;
        PlayerReferenceView<T> reference = GetReference();
        int newIndex = _playerIndex;
        if(reference != null && _playerIndex != GameConstants.NULL_INT) {
            (newIndex, player) = reference.GetNextPlayer(index:_playerIndex, isNext:isNext);
            if(player != null && player is T) {
                _player = (T)player;
                SetData(index:newIndex);
            }
        }
    }

    protected PlayerReferenceView<T> GetReference() {
        return _isRecord ? _recordView : _teamView;
    }

    protected int GetIndexOfPlayerId(int id) {
        int result = GameConstants.NULL_INT;
        PlayerReferenceView<T> reference = GetReference();
        if(reference != null) {
            result = reference.FindPlayer(id);
        }
        return result;
    }
}
