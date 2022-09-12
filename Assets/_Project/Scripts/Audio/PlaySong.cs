using UnityEngine;

[RequireComponent(typeof(AudioSource))]

public class PlaySong : MonoBehaviour
{
    [SerializeField] private AudioClip[] _songs;
    private AudioSource _source;

    void Awake()
    {
        _source = GetComponent<AudioSource>();
    }
    private void Start()
    {
        _source.PlayOneShot(_songs[0]);
    }

}
