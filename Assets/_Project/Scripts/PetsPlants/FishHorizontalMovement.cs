using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishHorizontalMovement : MonoBehaviour
{
    [SerializeField] private GameObject[] _waypoints;
    [SerializeField] private float _speed = 2f;
    private int _currentWaypointIndex;
    private int _direction;

    private void Start()
    {
        if (_currentWaypointIndex == 0)
        {
            _direction = 1;
        }
        else if (_currentWaypointIndex > 0)
        {
            _direction = -1;
        }
    }

    private void Update()
    {
        if (Vector2.Distance(_waypoints[_currentWaypointIndex].transform.position, transform.position) < .1f)
        {
            _currentWaypointIndex++;
            _direction = -1;
            if (_currentWaypointIndex >= _waypoints.Length)
            {
                _currentWaypointIndex = 0;
                _direction = 1;
            }
        }
        transform.position = Vector2.MoveTowards(transform.position, _waypoints[_currentWaypointIndex].transform.position, Time.deltaTime * _speed);
        transform.localScale = new Vector3(_direction, 1, 1);
    }
}
