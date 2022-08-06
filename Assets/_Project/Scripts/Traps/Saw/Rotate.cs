using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField] private float _rotateSpeed = 0.5f;
    [SerializeField] private GameObject[] _waypoints;
    private int _currentWaypointIndex = 0;
    
    void Update()
    {
    
        if (Vector2.Distance(_waypoints[_currentWaypointIndex].transform.position, transform.position) < .1f)
        {
            _currentWaypointIndex++;

            if (_currentWaypointIndex >= _waypoints.Length)
            {
                _currentWaypointIndex = 0;
            }
        }
        transform.Rotate(0, 0, 360 * _rotateSpeed * Time.deltaTime);
    }
}
