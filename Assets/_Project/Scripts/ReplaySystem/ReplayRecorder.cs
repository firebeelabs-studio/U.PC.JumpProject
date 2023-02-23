using System.Collections.Generic;
using System.Linq;
using FishNet.Managing.Scened;
using UnityEngine;
using UnityEngine.SceneManagement;
using SceneManager = UnityEngine.SceneManagement.SceneManager;

public class ReplayRecorder : MonoBehaviour
{
    [SerializeField] private string _levelName;
    [SerializeField] private string _hash;
    private Queue<ReplayStep> _recordingQueue = new();
    private bool _isRecording = false;

    private void Start()
    {
        _levelName = SceneManager.GetActiveScene().name;
    }

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
        
        ReplayStep data = new(transform.position, transform.localScale);
        RecordReplayFrame(data);
    }
    
    private void RecordReplayFrame(ReplayStep data)
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
        ReplayPlayer.RecordedQueue = new Queue<ReplayStep>(_recordingQueue);
    }

    private void On_SwampieInstantiated(List<OutfitData> outfitData)
    {
        string bodyId = outfitData.FirstOrDefault(data => data.skinType == SwampieSkin.SkinType.Body)?.Id;
        string hatId = outfitData.FirstOrDefault(data => data.skinType == SwampieSkin.SkinType.Hat)?.Id;
        string eyesId = outfitData.FirstOrDefault(data => data.skinType == SwampieSkin.SkinType.Eyes)?.Id;
        string mouthId = outfitData.FirstOrDefault(data => data.skinType == SwampieSkin.SkinType.Mouth)?.Id;
        string jacketId = outfitData.FirstOrDefault(data => data.skinType == SwampieSkin.SkinType.Jacket)?.Id;
        ReplayData data = new( _levelName,_hash, bodyId, hatId, eyesId, mouthId, jacketId);
        ReplayPlayer.ReplayData = data;
    }
}
