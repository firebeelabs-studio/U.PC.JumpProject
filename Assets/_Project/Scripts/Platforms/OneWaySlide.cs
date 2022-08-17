using UnityEngine;

public class OneWaySlide : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private AudioClip _slideSound;

    private AudioPlayer _audioPlayer;

    private void Awake()
    {
        _audioPlayer = GetComponent<AudioPlayer>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _particleSystem.gameObject.transform.position = new Vector2(collision.gameObject.transform.position.x, _particleSystem.gameObject.transform.position.y);
            _particleSystem.Play();
            _audioPlayer.PlayOneShotSound(_slideSound);
        }
    }
}
