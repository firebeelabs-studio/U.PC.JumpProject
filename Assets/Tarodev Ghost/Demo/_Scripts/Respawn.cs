using System;
using System.Collections;
using UnityEngine;

public class Respawn : MonoBehaviour {
    [SerializeField] private float _penaltyTime = 2;
    [SerializeField] private Transform _respawnPos;
    private Transform _startPos;
    private float _timeStartedPenalty;
    private CheckPoint _lastCheckPoint;


    private void Start()
    {
        FinishSinglePlayer.RunFinish += EndRun;
        StartRun.RunStart += RunStart;
        _startPos = _respawnPos;
    }

    private void OnDisable()
    {
        FinishSinglePlayer.RunFinish -= EndRun;
        StartRun.RunStart -= RunStart;
    }

    public void ChangeSpawnPos(Transform newPos, CheckPoint checkPoint)
    {
        if (_lastCheckPoint is not null)
        {
            _lastCheckPoint.ResetCheckPoint();
        }
        _respawnPos = newPos;
        _lastCheckPoint = checkPoint;
    } 
        

    private void EndRun() => _respawnPos = _startPos;
    private void RunStart() => _respawnPos = _startPos;

    public IEnumerator RespawnPlayer(Transform player, float penaltyTime = 0) {
        _timeStartedPenalty = Time.time;
        Vector3 diedPos = player.position;
        do
        {
            player.position = diedPos;
        } while (_timeStartedPenalty + penaltyTime > Time.time);
        player.position = _respawnPos.position;
        yield return null;
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(_respawnPos.position, 0.5f);
    }
}
