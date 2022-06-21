using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using TarodevController;
using UnityEngine;

public sealed class User : NetworkBehaviour
{
    public static User Instance { get; private set; }
    [SyncVar] public string Username;
    [SyncVar] public bool IsReady;
    [SyncVar] public PlayerController ControlledPlayer;

    public override void OnStartClient()
    {
        base.OnStartClient();
        if (!IsOwner) return;
        Instance = this;
    }
}
