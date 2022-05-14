using System;
using UnityEngine;

public class Goal : MonoBehaviour {
    [SerializeField] private GameObject _visual;
    public static event Action OnGoalTriggered;
    private bool _running;
    
    private void Awake() => _visual.SetActive(false);

    private void OnEnable() => FinishLine.Crossed += FinishLineOnOnRunEvent;
    private void OnDisable() => FinishLine.Crossed -= FinishLineOnOnRunEvent;
    
    private void FinishLineOnOnRunEvent(bool running) {
        _visual.SetActive(running);
        _running = running;
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if(!_running) return;
        OnGoalTriggered?.Invoke();
        _visual.SetActive(false);
    }
}