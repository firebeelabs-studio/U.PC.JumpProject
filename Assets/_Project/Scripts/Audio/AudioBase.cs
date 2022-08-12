using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioBase : MonoBehaviour
{
    private AudioSource _source;
    void Awake()
    {
        _source = GetComponent<AudioSource>();
    }

    private void PlayOneShotSound(AudioClip clip, float volume = 1, float pitch = 1) {
        _source.pitch = pitch;
        _source.PlayOneShot(clip, volume);
    }
}
