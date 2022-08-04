using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawHorizontalMovement : MonoBehaviour
{
    [SerializeField] private GameObject[] _waypoints;
    [SerializeField] private float _speed = 2f;
    private int _currentWaypointIndex = 0;
    [SerializeField] Transform parentTransform;

    private void Update()
    {
        if (Vector2.Distance(_waypoints[_currentWaypointIndex].transform.position, parentTransform.position) < .1f)
        {
            _currentWaypointIndex++;

            if (_currentWaypointIndex >= _waypoints.Length)
            {
                _currentWaypointIndex = 0;
            }
        }
        parentTransform.position = Vector2.MoveTowards(parentTransform.position, _waypoints[_currentWaypointIndex].transform.position, Time.deltaTime * _speed);
    }
}
