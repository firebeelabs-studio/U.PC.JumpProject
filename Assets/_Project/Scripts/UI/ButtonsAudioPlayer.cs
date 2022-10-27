using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonsAudioPlayer : MonoBehaviour
{
    private AudioSource _audioSource;
    private float _timer;
    private bool _initialVolume = false;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.volume = 0;
    }

    private void Update()
    {
        if (!_initialVolume)
        {
            _timer += Time.deltaTime;

            if (_timer >= 0.1f)
            {
                _audioSource.volume = 1;
                _initialVolume = true;
            }
        }
        
    }
}
