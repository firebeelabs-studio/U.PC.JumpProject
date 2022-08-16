using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioPlayer : MonoBehaviour
{
    [SerializeField] private float _maxHearableDistance = 30;
    [SerializeField] private bool _shouldRespectDistance = false;
    private AudioSource _source;
    void Awake()
    {
        _source = GetComponent<AudioSource>();
    }

    private void Start()
    {
        SetupSoundProperties();
    }

    public void PlayOneShotSound(AudioClip clip, float volume = 1, float pitch = 1) 
    {
        _source.pitch = pitch;
        _source.PlayOneShot(clip, volume);
    }

    private void SetupSoundProperties()
    {
        if (_shouldRespectDistance)
        {
            _source.rolloffMode = AudioRolloffMode.Linear;
            _source.maxDistance = _maxHearableDistance;
            _source.spatialBlend = 1;
        }
        else
        {
            _source.spatialBlend = 0;
        }
    }
}
