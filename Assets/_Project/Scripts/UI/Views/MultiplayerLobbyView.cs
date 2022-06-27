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
    [SerializeField] private Button _startButton;

    public override void Initialize()
    {
        _readyButton.onClick.AddListener(() => User.Instance.IsReady = !User.Instance.IsReady);
        _startButton.onClick.AddListener(() => GameManager.Instance.StartGame());
        
        base.Initialize();
    }

    private void Update()
    {
        if (!IsInitialized) return;

        string playerListText = "Players list:";
        print(GameManager.Instance.Users.Count);

        for (int i = 0; i < GameManager.Instance.Users.Count; i++)
        {
            User user = GameManager.Instance.Users[i];
            playerListText += $"\r\nPlayer {user.Nick}";
            playerListText += user.IsReady ? $" Is Ready: <color=green> {user.IsReady} </color>" : $" Is Ready: <color=red> {user.IsReady} </color>";
        }

        _playerList.text = playerListText;
        _startButton.interactable = GameManager.Instance.CanStart;
    }
}
