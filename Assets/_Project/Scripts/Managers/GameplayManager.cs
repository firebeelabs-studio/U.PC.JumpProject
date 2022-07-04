using System;
using System.Collections;
using System.Collections.Generic;
using FishNet;
using FishNet.Connection;
using FishNet.Managing.Scened;
using FishNet.Object;
using TarodevController;
using UnityEngine;

public class GameplayManager : NetworkBehaviour
{
    [SerializeField] private Vector2 _spawnPoint;
    [SerializeField] private NetworkObject _playerPrefab;
    [SerializeField] private Timer _timer;

    private RoomDetails _roomDetails;
    private MatchmakingNetwork _matchmakingNetwork;
    private List<NetworkObject> _spawnedPlayerObjects = new();
    private bool _isStarted;

    #region Initialize

    private void OnDestroy()
    {
        if (_matchmakingNetwork is not null)
        {
            _matchmakingNetwork.OnClientStarted -= MatchmakingNetwork_OnClientStarted;
            _matchmakingNetwork.OnClientLeftRoom -= MatchmakingNetwork_OnClientLeftRoom;
        }
    }

    public void Initialize(RoomDetails roomDetails, MatchmakingNetwork matchmakingNetwork)
    {
        _roomDetails = roomDetails;
        _matchmakingNetwork = matchmakingNetwork;
        _matchmakingNetwork.OnClientStarted += MatchmakingNetwork_OnClientStarted;
        _matchmakingNetwork.OnClientLeftRoom += MatchmakingNetwork_OnClientLeftRoom;
    }

    private void MatchmakingNetwork_OnClientLeftRoom(RoomDetails arg1, NetworkObject arg2)
    {
        for (int i = 0; i < _spawnedPlayerObjects.Count; i++)
        {
            NetworkObject entry = _spawnedPlayerObjects[i];
            if (entry is null)
            {
                _spawnedPlayerObjects.RemoveAt(i);
                i--;
                continue;
            }

            if (_spawnedPlayerObjects[i].Owner == arg2.Owner)
            {
                InstanceFinder.ServerManager.Despawn(entry.gameObject);
                _spawnedPlayerObjects.RemoveAt(i);
                i--;
            }
        }
    }
    private void MatchmakingNetwork_OnClientStarted(RoomDetails roomDetails, NetworkObject client)
    {
        //we are in wrong room, go back
        if (roomDetails != _roomDetails) return;

        if (client is null || client.Owner is null) return;
        
        SpawnPlayer(client.Owner);

    }

    private void SpawnPlayer(NetworkConnection conn)
    {
        //Create object and move it to proper scene
        NetworkObject netIdent = Instantiate<NetworkObject>(_playerPrefab, _spawnPoint, Quaternion.identity);
        SceneLookupData sld = SceneLookupData.CreateData(gameObject.scene.handle);
        UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(netIdent.gameObject, sld.GetScene(out _));
        
        _spawnedPlayerObjects.Add(netIdent);
        Spawn(netIdent.gameObject, conn);

        netIdent.transform.position = _spawnPoint;
        RpcTeleport(netIdent, _spawnPoint);
    }

    #endregion

    [ObserversRpc]
    private void RpcTeleport(NetworkObject ident, Vector2 position)
    {
        ident.transform.position = position;
    }

    private void Update()
    {
        if (_roomDetails.MemberIds.Count == _roomDetails.StartedMembers.Count && !_isStarted)
        {
            //dirty, but works, have to clean this temp
            _isStarted = true;
           StartCoroutine(WaitBeforeStart());
        }
    }

    IEnumerator WaitBeforeStart()
    {
        //now it's only waiting few seconds before start, but we have to show some feedback to players
        yield return new WaitForSeconds(3);
        foreach (var player in _spawnedPlayerObjects)
        {
            LetPlayerMove(player.Owner, player);
        }
        _timer.RunStart();
    }

    [TargetRpc]
    private void LetPlayerMove(NetworkConnection conn, NetworkObject obj)
    {
        obj.GetComponent<PlayerController>().CanMove = true;
    }
}
