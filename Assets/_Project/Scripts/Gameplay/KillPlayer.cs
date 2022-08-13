using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlayer : MonoBehaviour
{
    private float _angle = Mathf.PI;
    private Respawn _spawnManager;
    private void Awake()
    {
        _spawnManager = FindObjectOfType<Respawn>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            StartCoroutine(_spawnManager.RespawnPlayer(col.transform));
            GameManager.ResetPlayerPowers();
        }
    }
}
