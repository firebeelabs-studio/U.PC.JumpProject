using UnityEngine;

public class Press : MonoBehaviour, IPlayerEffector
{
    [Header("Behaviour")]
    [SerializeField] private bool _shouldMoveDown = true;
    [SerializeField] private float _speed;
    [SerializeField] private float _delay;
    [SerializeField] private float _startDelay, _smashDelay;
    [SerializeField] private float _distance;

    private Rigidbody2D _rb;
    private Vector2 _startPos, _change, _lastPos, newPos;
    private bool _didSmashOnce, _playReturnSoundOnce;
    private float _angle, _timer;
    
    [Space(10)]
    [Header("Particles")]
    [SerializeField] private ParticleSystem _pressParticles;

    [Space(10)]
    [Header("Sounds")]
    [SerializeField] private AudioClip _hitPlayerSound;
    [SerializeField] private AudioClip _hitGroundSound;
    [SerializeField] private AudioClip _returnSound;
    private AudioPlayer _audioPlayer;

    [Space(10)]
    [Header("Slow effect")]
    [SerializeField] private float _slowDuration;
    [SerializeField] private float _slowPower;
    public float SlowDuration => _slowDuration;
    public float SlowPower => _slowPower;

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
            _didSmashOnce = false;
            _playReturnSoundOnce = false;
        }
        else if (_angle >= 1.5f * Mathf.PI && !_didSmashOnce)
        {
            _timer = _smashDelay;
            _didSmashOnce = true;
        }
        else if (_angle >= 1.5f * Mathf.PI)
        {
            if (_playReturnSoundOnce) return;
            
            _audioPlayer.PlayOneShotSound(_returnSound);
            _playReturnSoundOnce = true;

        }

        _change = _lastPos - newPos;
        _lastPos = newPos;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground") && _angle >= Mathf.PI && !_didSmashOnce)
        {
            _pressParticles.Play();
            _audioPlayer.PlayOneShotSound(_hitGroundSound);
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            _audioPlayer.PlayOneShotSound(_hitPlayerSound);
            _didSmashOnce = true;
        }
    }

    private void OnDrawGizmos()
    {
        // represents the max distance in scene
        if(transform.localRotation.eulerAngles.z == 0)
        {
            Gizmos.DrawLine((Vector2)transform.position + new Vector2(-3, -2.56f - _distance), (Vector2)transform.position + new Vector2(3, -2.56f - _distance));
        }
        else if (transform.localRotation.eulerAngles.z == 180)
        {
            Gizmos.DrawLine((Vector2)transform.position + new Vector2(-3, +2.56f + _distance), (Vector2)transform.position + new Vector2(3, +2.56f + _distance));
        }
        
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
