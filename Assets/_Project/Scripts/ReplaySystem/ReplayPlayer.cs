using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplayPlayer : MonoBehaviour
{
    [SerializeField] private GameObject _replayGhostPrefab;
    public static Queue<ReplayData> RecordedQueue;
    private bool _isDoingReplay = false;
    private Replay _replay;

    private void Update()
    {
        if (!_isDoingReplay) return;

        bool hasMoreFrames = _replay.PlayNextFrame();

        if (!hasMoreFrames)
        {
            StopReplay();
        }
    }

    [ContextMenu("START REPLAY")]
    private void StartReplay()
    {
        _replay = new Replay(RecordedQueue);
        _replay.InstantiateReplayGhost(_replayGhostPrefab);
        _isDoingReplay = true;
    }

    [ContextMenu("RESTART REPLAY")]
    private void RestartReplay()
    {
        _isDoingReplay = true;
        _replay.RestartFromBeginning();
    }

    [ContextMenu("STOP REPLAY")]
    private void StopReplay()
    {
        _isDoingReplay = false;
        _replay.DestroyGhostIfExists();
        _replay = null;
    }
}
