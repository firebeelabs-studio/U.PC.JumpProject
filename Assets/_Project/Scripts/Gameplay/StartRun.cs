using System;
using UnityEngine;

public class StartRun : MonoBehaviour
{
    public static event Action RunStart;

    // sounds
    [SerializeField] private AudioClip _startSound;
    private AudioPlayer _audioPlayer;

    private void Awake()
    {
        _audioPlayer = GetComponent<AudioPlayer>();   
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            RunStart?.Invoke();     //start timer
            _audioPlayer.PlayOneShotSound(_startSound);
        }
    }
}
