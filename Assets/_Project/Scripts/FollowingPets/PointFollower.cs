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



        _current = Mathf.MoveTowards(0, 1, _speed * Time.deltaTime);
        transform.position = Vector3.Lerp(transform.position, _newPos, 2 * _curve.Evaluate(_current));
        //transform.position = Vector3.Lerp(transform.position, _newPos, 5 * Time.deltaTime);


        if (Vector2.Distance(transform.position, _newPos) < 0.5f)
        {
            _timer += Time.deltaTime;
            if (_timer > _timerLimit)
            {
                _newPos = _rP.CreateNewPosition(_radius);
                _timer = 0;
            }

        }
    }


//#if UNITY_EDITOR

//    private void OnDrawGizmos()
//    {
//        Gizmos.DrawWireSphere(transform.position, _radius);
//    }
//#endif
}
