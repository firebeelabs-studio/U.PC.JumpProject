using System.IO;
using TarodevGhost;
using UnityEditor;
using UnityEngine;

public class GhostRunner : MonoBehaviour {
    [SerializeField] private Transform _recordTarget;
    [SerializeField] private GameObject _ghostPrefab;
    [SerializeField, Range(1, 10)] private int _captureEveryNFrames = 2;

    private ReplaySystem _system;

    private void Awake() => _system = new ReplaySystem(this);
    
    private void OnEnable() => FinishLine.Crossed += OnFinishLineCrossed;
    private void OnDisable() => FinishLine.Crossed -= OnFinishLineCrossed;
    static void WriteString(string text)

    {

        string path = "Assets/_Project/test.txt";

        //Write some text to the test.txt file

        StreamWriter writer = new StreamWriter(path, true);

        writer.WriteLine(text);

        writer.Close();

        //Re-import the file to update the reference in the editor

        AssetDatabase.ImportAsset(path);


    }
    private void OnFinishLineCrossed(bool runStarting) {
        if (runStarting) {
            _system.StartRun(_recordTarget, _captureEveryNFrames);
            _system.PlayRecording(RecordingType.Best, Instantiate(_ghostPrefab));
        }
        else {
            _system.FinishRun();
            _system.StopReplay();
            WriteString(_system.SerializeRun());
        }
    }
}

