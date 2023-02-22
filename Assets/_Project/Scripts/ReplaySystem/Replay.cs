using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Replay
{
    public ReplayGhost ReplayGhost { get; private set; }
    private Queue<ReplayData> _originalQueue;
    private Queue<ReplayData> _replayQueue;

    public Replay(Queue<ReplayData> recordingQueue)
    {
        _originalQueue = new Queue<ReplayData>(recordingQueue);
        _replayQueue = new Queue<ReplayData>(recordingQueue);
    }

    public void RestartFromBeginning()
    {
        _replayQueue = new Queue<ReplayData>(_originalQueue);
    }

    public bool PlayNextFrame()
    {
        bool hasMoreFrames = false;
        if (ReplayGhost != null && _replayQueue.Count != 0)
        {
            ReplayData data = _replayQueue.Dequeue();
            ReplayGhost.SetDataForFrame(data);
            hasMoreFrames = true;
        }

        return hasMoreFrames;
    }

    public void InstantiateReplayGhost(GameObject ghostPrefab)
    {
        if (_replayQueue.Count != 0)
        {
            ReplayData startingData = _replayQueue.Peek();
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
