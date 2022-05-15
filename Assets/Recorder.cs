using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recorder : MonoBehaviour
{
    [SerializeField]
    private GameObject _ghostPrefab;
    private Replay _system;
    private void Awake() => _system = new Replay(this);

    private void Start()
    {
        _system.StartRun(gameObject.transform);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            _system.FinishRun();
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            _system.PlayRecording(RecordingType.Best, Instantiate(_ghostPrefab)); // The ghost should be a very basic prefab without colliders or rigidbodies. See the demo scene for an example.
        }
    }
}
