using FishNet.Connection;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using FishNet.Transporting;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNameTracker : NetworkBehaviour
{
    //Called on any name change
    public static event Action<NetworkConnection, string> OnNameChange;

    //collection of each playername on connection
    [SyncObject]
    private readonly SyncDictionary<NetworkConnection, string> _playerNames = new SyncDictionary<NetworkConnection, string>();

    //Singleton
    private static PlayerNameTracker _instance;

    private void Awake()
    {
        _instance = this;
        _playerNames.OnChange += _playerNames_OnChange;
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        base.NetworkManager.ServerManager.OnRemoteConnectionState += ServerManager_OnRemoteConnectionState;
    }

    public override void OnStopServer()
    {
        base.OnStopServer();
        base.NetworkManager.ServerManager.OnRemoteConnectionState -= ServerManager_OnRemoteConnectionState;
    }

    //called when client connection state is changing
    private void ServerManager_OnRemoteConnectionState(NetworkConnection arg1, RemoteConnectionStateArgs arg2)
    {
        //if client is logging of delete his nick and connection from collection
        if (arg2.ConnectionState != RemoteConnectionState.Started)
        {
            _playerNames.Remove(arg1);
        }
    }

    private void _playerNames_OnChange(SyncDictionaryOperation op, NetworkConnection key, string value, bool asServer)
    {
        if (op == SyncDictionaryOperation.Add || op == SyncDictionaryOperation.Set)
        {
            OnNameChange?.Invoke(key, value);
        }
    }

    public static string GetPlayerName(NetworkConnection conn)
    {
        if (_instance._playerNames.TryGetValue(conn, out string result))
        {
            return result;
        }
        else
        {
            return string.Empty;
        }
    }

    //lets clients set name
    [Client]
    public static void SetName(string name)
    {
        _instance.ServerSetName(name);
    }

    //sets name on server
    [ServerRpc(RequireOwnership = false)]
    private void ServerSetName(string name, NetworkConnection sender = null)
    {
        _playerNames[sender] = name;
    }
}
