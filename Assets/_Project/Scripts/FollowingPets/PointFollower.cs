using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointFollower : MonoBehaviour
{
    [SerializeField] private Transform _visual;
    [SerializeField] private Transform _player;
    [SerializeField] private Transform _followingCircle;
    [SerializeField] private RandomPoint _rP;

    [Header("PET MOVEMENT")]
    [SerializeField] private AnimationCurve _movementCurve;
    [SerializeField] private float _radius, _speed, _timerLimit, _maxTilt, _tiltSpeed;

    [Header("CIRCLE MOVEMENT")]
    [SerializeField] private AnimationCurve _CircleCurve;
    [SerializeField] private float _circleSpeed;

    private Vector3 _endPos, _distance;
    private Vector2 _newPos;
    private float _current, _timer;

    public float Range => _radius;
    public float CircleSpeed => _circleSpeed;
    public float MaxTilt => _maxTilt;
    public float TiltSpeed => _tiltSpeed;
    public AnimationCurve CircleCurve => _CircleCurve;

    private void Start()
    {
        _visual.transform.position = _followingCircle.transform.position;
        _newPos = transform.position;
        _endPos = transform.position;
        _timer = 0;
    }

    private void Update() //creates new point when pet reaches the old one
    {
        // this prevent change children's position (pet) according to parent's position (player)
        _distance = _player.position - _endPos;
        _endPos = _player.position;
        _visual.transform.position -= _distance;

        // follow _newPos constantly
        _current = Mathf.MoveTowards(0, 1, Time.deltaTime);
        _visual.transform.position = Vector3.Lerp(_visual.transform.position, _newPos, _speed * _movementCurve.Evaluate(_current));


        if (Vector2.Distance(_visual.transform.position, _newPos) < 0.5f)
        {
            //adds a litle delay before setting newPos
            _timer += Time.deltaTime;
            if (_timer < _timerLimit) return;
            _timer = 0;

            // this loop makes the distance between 2 positions greater than set value;
            Vector2 distance;
            do
            {
                distance = _rP.CreateNewPosition(_radius);
            }
            while (Vector2.Distance(_visual.transform.position, distance) < 1);
            _newPos = distance;

        }
        
        //follow the middle of circle when is out of the range
        if (Vector2.Distance(_visual.transform.position, _rP.transform.position) > _radius)
        {
            _newPos = _rP.transform.position;
        }
    }
}
