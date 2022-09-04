using System;
using UnityEngine;

public class FinishSinglePlayer : MonoBehaviour
{
    public static event Action RunFinish;
    public bool IsFinished;

    // sounds
    [SerializeField] private AudioClip _finishSound;
    private AudioPlayer _audioPlayer;

    private void Awake()
    {
        _audioPlayer = GetComponent<AudioPlayer>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            if (IsFinished) return;
            IsFinished = true;
            RunFinish?.Invoke();
            _audioPlayer.PlayOneShotSound(_finishSound);
        }
    }
}
