using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using UnityEngine;

public class KillPlayer : NetworkBehaviour
{
    private Respawn _spawnManager;
    private void Awake()
    {
        _spawnManager = FindObjectOfType<Respawn>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            StartCoroutine(_spawnManager.RespawnPlayer(col.transform, col.GetComponent<NetworkObject>().Owner));
            GameManager.ResetPlayerPowers();
        }
    }
}
