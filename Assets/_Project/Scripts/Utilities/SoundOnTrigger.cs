using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundOnTrigger : MonoBehaviour
{
    [SerializeField] private AudioClip _audioClip;
    private AudioPlayer _audioPlayer;

    private void Awake()
    {
        _audioPlayer = GetComponent<AudioPlayer>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        print("hit");
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            print("hit");
            //_audioPlayer.PlayOneShotSound(_audioClip);
        }
    }
}
