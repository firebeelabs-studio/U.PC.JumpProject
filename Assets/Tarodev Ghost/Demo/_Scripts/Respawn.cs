using System;
using System.Collections;
using UnityEngine;

public class Respawn : MonoBehaviour {
    [SerializeField] private float _penaltyTime = 2;
    [SerializeField] private Transform _respawnPos;
    private Transform _startPos;
    private float _timeStartedPenalty;


    private void Start()
    {
        FinishSinglePlayer.RunFinish += EndRun;
        StartRun.RunStart += RunStart;
        _startPos = _respawnPos;
    }

    public void ChangeSpawnPos(Transform newPos) => _respawnPos = newPos;

    private void EndRun() => _respawnPos = _startPos;
    private void RunStart() => _respawnPos = _startPos;

    public IEnumerator RespawnPlayer(Transform player) {
        _timeStartedPenalty = Time.time;
        do {
            player.position = _respawnPos.position;
            GameManager.SpawnAllCollectibles();
            yield return null;
        } while (_timeStartedPenalty + _penaltyTime > Time.time);
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(_respawnPos.position, 0.5f);
    }
}
