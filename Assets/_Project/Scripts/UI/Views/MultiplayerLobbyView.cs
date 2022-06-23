using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class MultiplayerLobbyView : View
{
    [SerializeField] private TMP_Text _playerList;
    [SerializeField] private Button _readyButton;

    public override void Initialize()
    {
        _readyButton.onClick.AddListener(() =>
        {
            User.Instance.IsReady = !User.Instance.IsReady;
        });
        base.Initialize();
    }

    private void Update()
    {
        if (!IsInitialized) return;

        string playerListText = "can't see players";

        for (int i = 0; i < GameManager.Instance.Users.Count; i++)
        {
            User user = GameManager.Instance.Users[i];
            playerListText += $"\r\nPlayer {user.Nick} (Is Ready: {user.IsReady})";
        }

        _playerList.text = playerListText;
    }
}
