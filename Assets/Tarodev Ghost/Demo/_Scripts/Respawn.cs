using System;
using System.Collections;
using UnityEngine;

public class Respawn : MonoBehaviour {
    [SerializeField] private Vector2 _respawnPoint;
    [SerializeField] private float _penaltyTime = 2;
    private float _timeStartedPenalty;


    private void OnTriggerEnter2D(Collider2D col) {
        StartCoroutine(RespawnPlayer(col.transform));
    }

    IEnumerator RespawnPlayer(Transform player) {
        _timeStartedPenalty = Time.time;
        do {
            player.position = _respawnPoint;
            yield return null;
        } while (_timeStartedPenalty + _penaltyTime > Time.time);
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(_respawnPoint,0.5f);
    }
}
