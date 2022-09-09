using UnityEngine;

public class OneWaySlide : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private AudioClip _slideSound;

    private SpriteRenderer _sprite;
    private AudioPlayer _audioPlayer;

    private void Awake()
    {
        _sprite = GetComponent<SpriteRenderer>();
        _audioPlayer = GetComponent<AudioPlayer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _sprite.sortingLayerName = "Deafult";
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _sprite.sortingLayerName = "Top";
            _particleSystem.gameObject.transform.position = new Vector2(collision.gameObject.transform.position.x, _particleSystem.gameObject.transform.position.y);
            _particleSystem.Play();
            _audioPlayer.PlayOneShotSound(_slideSound);
        }
    }
}
