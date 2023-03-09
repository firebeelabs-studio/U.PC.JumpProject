using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Cinemachine;
using Newtonsoft.Json;
using TarodevController;
using UnityEditor;
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
    private PlayerAnimator _playerAnimator;
    private Transform _newCameraFollow;
    private SpriteRenderer _targetSpriteRenderer;

    private ReplaySystem _system;
    private ReplayData _replayData;

    private void Awake()
    {
        _system = new ReplaySystem(this);
    }

    private void Start()
    {
        //Deserialize saved scores
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
            _system.FinishRun(false);
            enabled = false;
        }
    }

    private void RunStart()
    {
        _system.StartRun(_recordTarget, _playerAnimator, _captureEveryNFrames);
    }

    private void EndRun(float newScore)
    {
        bool newRecord =_system.FinishRun();
        if (newRecord)
        {
            string jsonReplay = JsonConvert.SerializeObject(_system.NewReplay, Formatting.Indented);
            //TODO: SEND REPLAY ON SERVER
            //WriteString(jsonReplay);
        }
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
        _replayData = new ReplayData( SceneManager.GetActiveScene().name, _hash, bodyId, hatId, eyesId, mouthId, jacketId);
        _playerAnimator = _recordTarget.GetComponentInChildren<PlayerAnimator>();
        _targetSpriteRenderer = _playerAnimator.GetComponent<SpriteRenderer>();
        _system.SerializeSkinsData(_replayData);
    }
    
    private void WriteString(string text)
    {
        if (string.IsNullOrEmpty(text)) return;
        
        string path = "Assets/_Project/Replays/test" + System.Guid.NewGuid() +".txt";

        //Write some text to the test.txt file

        StreamWriter writer = new StreamWriter(path, true);

        writer.WriteLine(text);

        writer.Close();

        //Re-import the file to update the reference in the editor

        //AssetDatabase.ImportAsset(path);
    }
}
