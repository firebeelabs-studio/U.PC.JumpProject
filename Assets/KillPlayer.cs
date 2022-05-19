using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlayer : MonoBehaviour
{
    private Respawn _spawnManager;
    private void Awake()
    {
        _spawnManager = FindObjectOfType<Respawn>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        StartCoroutine(_spawnManager.RespawnPlayer(col.transform));
    }
}
