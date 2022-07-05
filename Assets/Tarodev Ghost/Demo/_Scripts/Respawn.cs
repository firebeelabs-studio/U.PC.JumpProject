using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FishNet.Connection;
using FishNet.Object;
using UnityEngine;

public class Respawn : NetworkBehaviour 
{
    //resp per connection
    public Dictionary<NetworkConnection, Transform> _respPoints = new();
    //wait before resp
    [SerializeField] private float _penaltyTime = 2;
    //start pos rename that
    [SerializeField] private Transform _respawnPos;
    private float _timeStartedPenalty;
    
    private void Start()
    {
        // FinishLevel.EndRun += EndRun;
    }

    public void FirstInitialize(List<NetworkObject> players)
    {
        //For each player set default respawn
        foreach (NetworkObject player in players)
        {
            _respPoints.Add(player.Owner, _respawnPos);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void ChangeSpawnPos(NetworkConnection conn, Transform newPos)
    {
        print("SpawnChanged");
        _respPoints[conn] = newPos;
    }
    //private void EndRun() => _respawnPos = _startPos;
    
    public IEnumerator RespawnPlayer(Transform player, NetworkConnection conn) {
        _timeStartedPenalty = Time.time;
        do {
            print("RespMePls");
            SpawnPlayerServer(player, conn);
            GameManager.SpawnAllCollectibles();
            yield return null;
        } while (_timeStartedPenalty + _penaltyTime > Time.time);
    }

    [ServerRpc(RequireOwnership = false)]
    private void SpawnPlayerServer(Transform player, NetworkConnection conn)
    {
        player.position = _respPoints[conn].position;
        print("Respawned");
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(_respawnPos.position, 0.5f);
    }
}
