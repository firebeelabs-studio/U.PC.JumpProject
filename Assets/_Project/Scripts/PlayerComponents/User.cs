using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.UI;
using FishNet.Connection;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using TarodevController;
using UnityEngine;

public sealed class User : NetworkBehaviour
{
    // public static User Instance { get; private set; }
    // public NetworkConnection UserConnection { get; private set; }
    // [field: SerializeField]
    // [field: SyncVar]
    // public string Nick
    // {
    //     get;
    //     [ServerRpc] private set;
    // }
    //
    // [field: SyncVar]
    // public bool IsReady
    // {
    //     get;
    //     
    //     [ServerRpc(RequireOwnership = false)]
    //     set;
    // }
    //
    // [SyncVar] public PlayerController ControlledPlayer;
    //
    // public override void OnStartServer()
    // {
    //     base.OnStartServer();
    //     UserConnection = NetworkObject.LocalConnection;
    //     //GameManager.Instance.Users.Add(this);
    // }
    //
    // public override void OnStopServer()
    // {
    //     base.OnStopServer();
    //     
    //     //GameManager.Instance.Users.Remove(this);
    // }
    //
    // public override void OnStartClient()
    // {
    //     base.OnStartClient();
    //     if (!IsOwner) return;
    //     Instance = this;
    //     if (IsSpawned)
    //     {
    //         //ViewManager.Instance.Initialize();
    //     }
    // }
    // public void ChangeName(string nick)
    // {
    //     Nick = nick;
    // }
    //
    // [ServerRpc(RequireOwnership = false)]
    // public void ServerSetIsReady(bool value)
    // {
    //     IsReady = value;
    // }
}
