using System;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReplayManager : MonoBehaviour
{
    [SerializeField] private Transform _recordTarget;
    [SerializeField] private GameObject _ghostPrefab;
    [SerializeField, Range(1, 10)] private int _captureEveryNFrames = 2;
    [SerializeField] private string _hash;
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;
    [SerializeField] private float _lowestThreshold;
    [SerializeField] private TimerSinglePlayer _timer;
    private Transform _newCameraFollow;

    private ReplaySystem _system;
    private string _levelName;
    private ReplayData _replayData;

    private void Awake()
    {
        _system = new ReplaySystem(this);
    }

    private void Start()
    {
        //Deserialize saved scores   
        _levelName = SceneManager.GetActiveScene().name;
    }
    private void OnEnable()
    {
        FinishSinglePlayer.RunFinish += EndRun;
        StartRun.RunStart += RunStart;
        SwampieTypeReader.SwampieInstantiated += On_SwampieInstantiated;
    }

    private void OnDisable()
    {
        FinishSinglePlayer.RunFinish -= EndRun;
        StartRun.RunStart -= RunStart;
        SwampieTypeReader.SwampieInstantiated -= On_SwampieInstantiated;
    }

    private void Update()
    {
        if (_timer.TimeInSeconds >= _lowestThreshold)
        {
            enabled = false;
        }
    }

    private void RunStart()
    {
        _system.StartRun(_recordTarget, _captureEveryNFrames);
    }

    private void EndRun()
    {
        _system.FinishRun();
    }

    [ContextMenu("START REPLAY")]
    public void PlayReplay()
    {
        GameObject ghostObj = Instantiate(_ghostPrefab);
        if (_replayData != null)
        {
            ghostObj.GetComponent<ReplayGhost>().InitializeVisuals(_replayData);
        }
        _system.PlayRecording(RecordingType.Best, ghostObj);
        _virtualCamera.m_Follow = ghostObj.transform;
        _virtualCamera.m_LookAt = ghostObj.transform;
    }

    [ContextMenu("STOP REPLAY")]
    public void StopReplay()
    {
        _system.StopReplay();
        _virtualCamera.m_Follow = _recordTarget.transform;
        _virtualCamera.m_LookAt = _recordTarget.transform;
    }
    
    private void On_SwampieInstantiated(List<OutfitData> outfitData)
    {
        if (outfitData == null) return;
        
        string bodyId = outfitData.FirstOrDefault(data => data.skinType == SwampieSkin.SkinType.Body)?.Id;
        string hatId = outfitData.FirstOrDefault(data => data.skinType == SwampieSkin.SkinType.Hat)?.Id;
        string eyesId = outfitData.FirstOrDefault(data => data.skinType == SwampieSkin.SkinType.Eyes)?.Id;
        string mouthId = outfitData.FirstOrDefault(data => data.skinType == SwampieSkin.SkinType.Mouth)?.Id;
        string jacketId = outfitData.FirstOrDefault(data => data.skinType == SwampieSkin.SkinType.Jacket)?.Id;
        _replayData = new ReplayData( _levelName, _hash, bodyId, hatId, eyesId, mouthId, jacketId);
    }

    //take this out to static writer / reader
    //static void WriteString(string text)
    //{
    //    if (string.IsNullOrEmpty(text)) return;

    //    Debug.Log("Saving...");
    //    string path = "Assets/_Project/test" + System.Guid.NewGuid() +".txt";

    //    //Write some text to the test.txt file

    //    StreamWriter writer = new StreamWriter(path, true);

    //    writer.WriteLine(text);

    //    writer.Close();

    //    //Re-import the file to update the reference in the editor

    //    AssetDatabase.ImportAsset(path);


    //}
}
