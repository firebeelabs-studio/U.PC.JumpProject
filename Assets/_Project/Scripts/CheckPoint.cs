using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private Respawn _spawnManager;
    private Transform _respawnPos;
    private void Awake()
    {
        _spawnManager = FindObjectOfType<Respawn>();
    }
    
    private void Start()
    {
        _respawnPos = transform.GetChild(0).transform;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _spawnManager.ChangeSpawnPos(collision.GetComponent<NetworkObject>().Owner, _respawnPos);
    }
}
