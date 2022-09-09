using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlayer : MonoBehaviour
{
    private float _penaltyTime = 0.75f;
    private Respawn _spawnManager;
    private bool _canKill = true;
    private void Awake()
    {
        _spawnManager = FindObjectOfType<Respawn>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            if (col.TryGetComponent(out IPawnController player))
            {
                player.KillPlayer();
            }
            if (_canKill)
            {
                StartCoroutine(_spawnManager.RespawnPlayer(col.transform, _penaltyTime));
                _canKill = false;
                StartCoroutine(TimeToNextKill());
            }
            GameManager.ResetPlayerPowers();
        }
    }
    private IEnumerator TimeToNextKill()
    {
        yield return new WaitForSeconds(_penaltyTime);
        _canKill = true;
    }
}
