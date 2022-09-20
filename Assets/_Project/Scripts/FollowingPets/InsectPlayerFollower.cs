using UnityEngine;

public class InsectPlayerFollower : InsectIdleMovement
{
    [Header("PLAYER FOLLOWER")]
    [SerializeField] private Transform _player;
    [SerializeField] private AnimationCurve _curve;
    [SerializeField] private float _circleSpeed, _followCircleSpeedUp;

    private Vector2 _previousPos, _distance;
    private float _current, _basicSpeed, _checkPlayerPos;
    private bool _isGoingRight;

    public bool IsGoingRight => _isGoingRight;

    override protected void Start()
    {
        base.Start();
        _previousPos = _player.position;
        _circleCenter = (Vector2)_player.transform.position + new Vector2(-1, 1);
        _basicSpeed = _speed;
    }

    override protected void Update()
    {
        base.Update();
        _distance = (Vector2)_player.position - _previousPos;
        _previousPos = _player.position;
        transform.position -= (Vector3)_distance;

        if (Vector2.Distance(_circleCenter, _player.position) > _radius)
        {
            _current = Mathf.MoveTowards(0, 1, Time.deltaTime);
            _circleCenter = Vector3.Lerp(_circleCenter, _player.position + new Vector3(-1, 1, 0), _circleSpeed * _curve.Evaluate(_current));
        }

        //follow the middle of circle when is out of the range
        if (Vector2.Distance(transform.position, _circleCenter) > _radius)
        {
            _newPos = _circleCenter;

            // speed up
            _speed = _followCircleSpeedUp;

            //checks player position to flip sprite in PetAnimator script
            _checkPlayerPos = _player.transform.position.x - transform.position.x;
            _isGoingRight = (_checkPlayerPos > 0) ? true : false;
        }
        else if (_newPos == _circleCenter)
        {
            _speed = _basicSpeed;
        }
    }
}
