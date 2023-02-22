using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ReplayRecorder : MonoBehaviour
{
    [SerializeField] private Sprite _sprite;
    private Queue<ReplayData> _recordingQueue = new();
    private bool _isRecording = false;

    private void OnEnable()
    {
        StartRun.RunStart += StartRecording;
        SwampieTypeReader.SwampieInstantiated += On_SwampieInstantiated;
        FinishSinglePlayer.RunFinish += StopRecording;
    }

    private void OnDisable()
    {
        StartRun.RunStart -= StartRecording;
        SwampieTypeReader.SwampieInstantiated -= On_SwampieInstantiated;
        FinishSinglePlayer.RunFinish -= StopRecording;
    }
    private void LateUpdate()
    {
        if (!_isRecording) return;
        
        ReplayData data = new(transform.position, transform.localScale);
        RecordReplayFrame(data);
    }
    
    private void RecordReplayFrame(ReplayData data)
    {
        _recordingQueue.Enqueue(data);
    }

    private void StartRecording()
    {
        _isRecording = true;
    }

    private void StopRecording()
    {
        _isRecording = false;
        ReplayPlayer.RecordedQueue = new Queue<ReplayData>(_recordingQueue);
    }

    private void On_SwampieInstantiated(List<OutfitData> outfitData)
    {
        string bodyId = outfitData.FirstOrDefault(data => data.skinType == SwampieSkin.SkinType.Body)?.Id;
        string hatId = outfitData.FirstOrDefault(data => data.skinType == SwampieSkin.SkinType.Hat)?.Id;
        string eyesId = outfitData.FirstOrDefault(data => data.skinType == SwampieSkin.SkinType.Eyes)?.Id;
        string mouthId = outfitData.FirstOrDefault(data => data.skinType == SwampieSkin.SkinType.Mouth)?.Id;
        string jacketId = outfitData.FirstOrDefault(data => data.skinType == SwampieSkin.SkinType.Jacket)?.Id;
        ReplayData data = new(transform.position, transform.localScale, bodyId, hatId, eyesId, mouthId, jacketId);
        RecordReplayFrame(data);
    }
}
