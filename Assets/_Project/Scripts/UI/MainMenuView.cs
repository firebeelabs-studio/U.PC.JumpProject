using System;
using System.Collections;
using System.Collections.Generic;
using FishNet;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuView : View
{
    [SerializeField] private Button _connectButton;

    public override void Initialize()
    {
        _connectButton.onClick.AddListener(() => InstanceFinder.ClientManager.StartConnection());
        base.Initialize();
    }
}
