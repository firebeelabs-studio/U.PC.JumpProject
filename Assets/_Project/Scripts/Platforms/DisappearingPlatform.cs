using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearingPlatform : MonoBehaviour
{
    [SerializeField] private DisablePlatform _disableManager;
    [SerializeField] private AudioClip _stepSound;

    private AudioPlayer _audioPlayer;

    private void Awake()
    {
        GameManager.Platforms.Add(gameObject);
        _audioPlayer = GetComponent<AudioPlayer>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _disableManager.Disable();
            //_audioPlayer.PlayOneShotSound(_stepSound);
        }
    }
}
