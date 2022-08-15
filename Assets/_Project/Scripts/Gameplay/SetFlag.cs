using UnityEngine;

public class SetFlag : MonoBehaviour, ICheckpointAnim
{
    [SerializeField] Animator _anim;
    [SerializeField] AudioClip _setFlagSound;

    private AudioPlayer _audioPlayer;
    private bool _isCheckpointActivated;

    private void Awake()
    {
        _audioPlayer = GetComponent<AudioPlayer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_isCheckpointActivated) return;

        _isCheckpointActivated = true;
        _anim.Play("SetupCheckpoint");
        _audioPlayer.PlayOneShotSound(_setFlagSound);
    }
    public void ResetToDefaultState()
    {
        _isCheckpointActivated = false;
        _anim.Play("FlagDisabled");
    }
}
