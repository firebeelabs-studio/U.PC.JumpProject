using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class StarAnim : MonoBehaviour
{
    [SerializeField] private float _duration = 1f;
    [SerializeField] private int _vibratio = 1;
    [SerializeField] private float _force = 1f;

    [SerializeField] private AudioClip _starSound;
    [SerializeField] private float _pitch = 0.8f;
    private AudioPlayer _audioPlayer;

    private void Awake()
    {
        _audioPlayer = GetComponent<AudioPlayer>();
    }

    public void RunPunchAnimation()
    {
        _audioPlayer.PlayOneShotSound(_starSound, 1, _pitch);
        transform.localScale = Vector3.one;
        transform.DOPunchScale (new Vector3 (_force, _force, _force), _duration, _vibratio);
    }
}
