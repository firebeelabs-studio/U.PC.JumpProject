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
    [SerializeField] private Button _connectButton;
    [SerializeField] private TMP_InputField _nicknameField;

    public override void Initialize()
    {
        _connectButton.onClick.AddListener(() =>
        
        {
            //User.Instance.Nick = _nicknameField.text;
            InstanceFinder.ClientManager.StartConnection();
        });
        base.Initialize();
    }
}
