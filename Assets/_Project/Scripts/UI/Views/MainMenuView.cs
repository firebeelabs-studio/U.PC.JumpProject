using System;
using System.Collections;
using System.Collections.Generic;
using FirstGearGames.LobbyAndWorld.Lobbies;
using FishNet;
using FishNet.Managing.Scened;
using FishNet.Object;
using TarodevController;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class MainMenuView : View
{
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _optionsButton;
    [SerializeField] private Button _levelCreatorButton;
    [SerializeField] private Button _profileButton;
    [SerializeField] private SceneLoader _sceneLoader;
    

    public override void Initialize()
    {
        _startButton.onClick.AddListener(() =>
        {
            _sceneLoader.LoadScene();
        });
        base.Initialize();
    }
}
