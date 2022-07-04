using System.Collections;
using System.Collections.Generic;
using System.IO;
using TarodevGhost;
//using UnityEditor;
using UnityEngine;

public class GhostManager : MonoBehaviour
{
    [SerializeField] private Transform _recordTarget;
    [SerializeField] private GameObject _ghostPrefab;
    [SerializeField, Range(1, 10)] private int _captureEveryNFrames = 2;

    private ReplaySystem _system;

    private void Awake() => _system = new ReplaySystem(this);
    private void Start()
    {
        //Deserialize saved scores   
    }
    private void OnEnable()
    {
        // FinishLevel.EndRun += EndRun;
        StartRun.RunStart += RunStart;
    }

    private void OnDisable()
    {
        // FinishLevel.EndRun -= EndRun;
        StartRun.RunStart -= RunStart;
    }

    private void RunStart()
    {
        _system.StartRun(_recordTarget, _captureEveryNFrames);
        _system.PlayRecording(RecordingType.Best, Instantiate(_ghostPrefab));
    }

    private void EndRun()
    {
        _system.FinishRun();
        _system.StopReplay();
        //WriteString(_system.SerializeRun());
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
