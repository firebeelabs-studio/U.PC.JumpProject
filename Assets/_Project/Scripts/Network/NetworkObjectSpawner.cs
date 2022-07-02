using System;
using System.Collections;
using System.Collections.Generic;
using FishNet.Managing;
using FishNet.Managing.Server;
using FishNet.Object;
using FishNet.Transporting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkObjectSpawner : MonoBehaviour
{
    [SerializeField] private NetworkObject _lobbyNetworkPrefab;
    [SerializeField] private string _sceneName;
    private NetworkManager _networkManager;

    private void Awake()
    {
        _networkManager = FindObjectOfType<NetworkManager>();
        _networkManager.ServerManager.OnServerConnectionState += ServerManager_OnServerConnectionState;
        NetworkObject nob = Instantiate(_lobbyNetworkPrefab);
        Scene scene = SceneManager.GetSceneByName(_sceneName);
        SceneManager.MoveGameObjectToScene(nob.gameObject, scene);
        _networkManager.ServerManager.Spawn(nob.gameObject);
    }

    private void ServerManager_OnServerConnectionState(ServerConnectionStateArgs obj)
    {
        if (obj.ConnectionState != LocalConnectionState.Started) return;

        NetworkObject nob = Instantiate(_lobbyNetworkPrefab);
        Scene scene = SceneManager.GetSceneByName(_sceneName);
        SceneManager.MoveGameObjectToScene(nob.gameObject, scene);
        _networkManager.ServerManager.Spawn(nob.gameObject);
        
    }
}
