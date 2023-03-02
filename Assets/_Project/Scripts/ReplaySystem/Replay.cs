using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Replay
{
    public ReplayGhost ReplayGhost { get; private set; }
    private Queue<ReplayStep> _originalQueue;
    private Queue<ReplayStep> _replayQueue;

    public Replay(Queue<ReplayStep> recordingQueue)
    {
        _originalQueue = new Queue<ReplayStep>(recordingQueue);
        _replayQueue = new Queue<ReplayStep>(recordingQueue);
    }

    public void RestartFromBeginning()
    {
        _replayQueue = new Queue<ReplayStep>(_originalQueue);
    }

    public bool PlayNextFrame()
    {
        bool hasMoreFrames = false;
        if (ReplayGhost != null && _replayQueue.Count != 0)
        {
            ReplayStep data = _replayQueue.Dequeue();
            //ReplayGhost.SetDataForFrame(data);
            hasMoreFrames = true;
        }

        return hasMoreFrames;
    }

    public void InstantiateReplayGhost(GameObject ghostPrefab)
    {
        if (_replayQueue.Count != 0)
        {
            ReplayStep startingData = _replayQueue.Peek();
            ReplayGhost = Object.Instantiate(ghostPrefab, startingData.Position, Quaternion.identity)
                .GetComponent<ReplayGhost>();
        }
    }

    public void DestroyGhostIfExists()
    {
        if (ReplayGhost != null)
        {
            Object.Destroy(ReplayGhost.gameObject);
        }
    }
}
