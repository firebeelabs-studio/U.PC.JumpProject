using System;
using System.Collections;
using System.Collections.Generic;
using FishNet;
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
    

    public override void Initialize()
    {
        _startButton.onClick.AddListener(() =>
        
        {
            //User.Instance.Nick = _nicknameField.text;
            InstanceFinder.ClientManager.StartConnection();
        });
        base.Initialize();
    }
}
