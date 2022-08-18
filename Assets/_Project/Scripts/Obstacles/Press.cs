using UnityEngine;
using DG.Tweening;
using System.Linq;
using System.Collections;

public class Press : MonoBehaviour, IPlayerEffector
{
    [Header("Behaviour")]
    [SerializeField] private bool _shouldMoveDown = true;
    [SerializeField] private float _speed;
    [SerializeField] private float _delay;
    [SerializeField] private float _startDelay;
    [SerializeField] private float _distance;
    [Space(10)]
    [Header("Particles")]
    [SerializeField] private ParticleSystem _pressParticles;
    private AudioPlayer _audioPlayer;
    private Vector2 _startPos;
    private float _angle, _timer;
    [Space(10)]
    [Header("Sounds")]
    [SerializeField] private AudioClip _smashSound;
    [Space(10)]
    [Header("Slow effect")]
    [SerializeField] private float _slowDuration;
    public float SlowDuration => _slowDuration;
    [SerializeField] private float _slowPower;
    public float SlowPower => _slowPower;
    private Vector2 _change, _lastPos, newPos;
    private Rigidbody2D _rb;


    private void Awake()
    {
        _audioPlayer = GetComponent<AudioPlayer>();
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        _startPos = transform.position;
        _timer = _startDelay;
    }
    private void Update()
    {
        if (_timer > 0)
        {
            _timer -= Time.deltaTime;
        }
        else
        {
            _angle += Time.deltaTime * _speed;
            MoveObstacle();
        }

    }
    private void FixedUpdate()
    {
        _rb.MovePosition(newPos);
    }
    private void MoveObstacle()
    {
        if (!_shouldMoveDown)
        {
            newPos =  new Vector2(_startPos.x, _startPos.y + Mathf.Sin(-_angle) * _distance);
        }
        else
        {
            newPos = new Vector2(_startPos.x, _startPos.y + Mathf.Sin(_angle) * _distance);
        }
        
        if (_angle >= 2 * Mathf.PI)
        {
            _angle = 0;
            _timer = _delay;
        }

        _change = _lastPos - newPos;
        _lastPos = newPos;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground") && _angle >= Mathf.PI)
        {
            _pressParticles.Play();
            _audioPlayer.PlayOneShotSound(_smashSound);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(0, -_distance));
    }

    public Vector2 EvaluateEffector()
    {
        if (_angle > Mathf.PI)
        {
            return -_change;
        }
        else
        {
            return Vector2.zero;
        }
    }
}
