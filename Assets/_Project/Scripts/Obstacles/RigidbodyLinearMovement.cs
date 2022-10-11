using UnityEngine;

public class RigidbodyLinearMovement : MonoBehaviour, IPlayerEffector
{
    [SerializeField] private float _speed, _targetRange, _startDelay, _startPosDelay, _endPosDelay, _GizmosOffset;
    [SerializeField] private bool _isMovingHorizontal;
    [SerializeField] AnimationCurve _forwardMovementCurve, _backwardMovementCurve;
    private Rigidbody2D _rb;
    private Vector2 _lastPos, _startPos, _currentPos, _moveVector, _change;
    private float _move, _moveSpeed, _progress, _previousPos, _currentTarget, _timer;
    private bool _isReturning;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        _startPos = _rb.position;
        _timer = _startDelay ;
    }

    void FixedUpdate()
    {
        _move = Mathf.MoveTowards(_move, _currentTarget, _moveSpeed);

        if (!_isMovingHorizontal)
        {
            _moveVector = new Vector2(0, _move);
        }
        else
        {
            _moveVector = new Vector2(_move, 0);
        }

        _currentPos = _startPos + _moveVector;
        _rb.MovePosition(_currentPos);

        // adding timer
        if (_move == _targetRange)
        {
            _timer -= Time.fixedDeltaTime;

            if (_timer > 0) return;

            _currentTarget = 0;
            _previousPos = _targetRange;
            _isReturning = true;
            _timer = _startPosDelay;
            

        }
        else if (_move == 0)
        {
            _timer -= Time.fixedDeltaTime;

            if (_timer > 0) return;

            _currentTarget = _targetRange;
            _previousPos = 0;
            _isReturning = false;
            _timer = _endPosDelay;
        }

        // calculates speed dependent on direction of movement
        _progress = Mathf.InverseLerp(_previousPos, _currentTarget, _move);
        if (!_isReturning)
        {
            _moveSpeed = _forwardMovementCurve.Evaluate(_progress) * _speed * Time.fixedDeltaTime;
        }
        else
        {
            _moveSpeed = _backwardMovementCurve.Evaluate(_progress) * _speed * Time.fixedDeltaTime;
        }

        // calculates effector
        if (_move == 0 || _move == _targetRange)
        {
            print("done");
            //_change = _lastPos - _currentPos;
            _change = Vector2.zero;
        }
        else
        {
            _change = _lastPos - _currentPos;
        }
        _lastPos = _currentPos;
    }
    
    public Vector2 EvaluateEffector()
    {
        return -_change; // * _speed;
    }

    private void OnDrawGizmos()
    {
        // represents the max distance in scene
        if (!_isMovingHorizontal)
        {
            Gizmos.DrawLine((Vector2)transform.position + new Vector2(-5, _GizmosOffset), (Vector2)transform.position + new Vector2(5, _GizmosOffset));
            Gizmos.DrawLine((Vector2)transform.position + new Vector2(-5, _targetRange + _GizmosOffset), (Vector2)transform.position + new Vector2(5, _targetRange + _GizmosOffset));
        }
        else
        {
            Gizmos.DrawLine((Vector2)transform.position + new Vector2(_GizmosOffset ,-5), (Vector2)transform.position + new Vector2(_GizmosOffset, 5));
            Gizmos.DrawLine((Vector2)transform.position + new Vector2(_targetRange + _GizmosOffset, -5), (Vector2)transform.position + new Vector2(_targetRange + _GizmosOffset, 5));
        }
    }
}
