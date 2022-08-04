using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SparkFollower : MonoBehaviour
{
    [SerializeField] private GameObject[] _waypoints;
    private int _currentWaypointIndex = 0;

    [SerializeField] private float _speed = 2f;
    [SerializeField] private float _rotateSpeed = 0.5f;
    [SerializeField] ParticleSystem _sparkParticles;
    private int _rotateDirection;
    private int _sparkDirection;

    private void Update()
    {
        if (Vector2.Distance(_waypoints[_currentWaypointIndex].transform.position, transform.position) < .1f)
        {
            _currentWaypointIndex++;
            _rotateDirection = -1;
            _sparkDirection = -1;

            if (_currentWaypointIndex >= _waypoints.Length)
            {
                _currentWaypointIndex = 0;
                _rotateDirection = 1;
                _sparkDirection = 1;
            }
        }
        transform.position = Vector2.MoveTowards(transform.position, _waypoints[_currentWaypointIndex].transform.position, Time.deltaTime * _speed);
        transform.localScale = new Vector3 (_sparkDirection, 0, 0);
    }
}
