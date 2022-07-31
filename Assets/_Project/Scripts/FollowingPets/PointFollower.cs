using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointFollower : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private Transform _movingRadius;
    [SerializeField] private RandomPoint _rP;
    [SerializeField] private AnimationCurve _curve;
    [SerializeField] private float _radius, _speed, _timerLimit;

    private Vector3 _endPos, _distance;
    private Vector2 _newPos;
    private float _current, _timer;

    public float Range => _radius;

    private void Start()
    {
        transform.position = _movingRadius.transform.position;
        _newPos = transform.position;
        _endPos = transform.position;
        _timer = 0;
    }

    private void Update() //creates new point when pet reaches the old one
    {
        // this prevent change children's position (pet) according to parent's position (player)
        _distance = _player.position - _endPos;
        _endPos = _player.position;
        transform.position -= _distance;

        // follow _newPos constantly
        _current = Mathf.MoveTowards(0, 1, Time.deltaTime);
        transform.position = Vector3.Lerp(transform.position, _newPos, _speed * _curve.Evaluate(_current));


        if (Vector2.Distance(transform.position, _newPos) < 0.5f)
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
            while (Vector2.Distance(transform.position, distance) < 1);
            _newPos = distance;

        }
        
        //follow the middle of circle when is out of the range
        if (Vector2.Distance(transform.position, _rP.transform.position) > _radius)
        {
            _newPos = _rP.transform.position;
        }
    }
}
