using System;
using System.Collections;
using System.Collections.Generic;
using FishNet.Connection;
using FishNet.Managing.Logging;
using FishNet.Object;
using UnityEngine;

public class FinishLevel : NetworkBehaviour
{
    [SerializeField] private GameplayManager _gm;
    private bool _collided;
    //TODO: Make this collision server only
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (IsServer) return;
        if (col.CompareTag("Player") && !_collided)
        {
            SendMessage(col.GetComponent<NetworkObject>().Owner);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void SendMessage(NetworkConnection conn)
    {
        _gm.PlayerEndRun(conn);
    }
}
