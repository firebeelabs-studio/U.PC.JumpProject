using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawHorizontalMovement : MonoBehaviour
{
    [SerializeField] private GameObject[] _waypoints;
    [SerializeField] private float _speed = 2f;
    [Tooltip("Left = 0, right = 1")]
    [SerializeField] private int _currentWaypointIndex;
    [SerializeField] Transform parentTransform;
    [SerializeField] private Transform _particleTransform;
    private int _sparkDirection;

    private void Start()
    {
        if (_currentWaypointIndex == 0)
        {
            _sparkDirection = 1;
        }
        else if (_currentWaypointIndex > 0)
        {
            _sparkDirection = -1;
        }
        transform.localScale = new Vector3 (_sparkDirection, 1, 1);
        _particleTransform.localScale = new Vector3(_sparkDirection, 1, 1);  
        parentTransform.position = Vector2.MoveTowards(parentTransform.position, _waypoints[_currentWaypointIndex].transform.position, Time.deltaTime * _speed);

    }

    private void Update()
    {
        if (Vector2.Distance(_waypoints[_currentWaypointIndex].transform.position, parentTransform.position) < .1f)
        {
            _currentWaypointIndex++;
            _sparkDirection = -1;

            if (_currentWaypointIndex >= _waypoints.Length)
            {
                _currentWaypointIndex = 0;
                _sparkDirection = 1;
            }
            transform.localScale = new Vector3 (_sparkDirection, 1, 1);
            _particleTransform.localScale = new Vector3(_sparkDirection, 1, 1);
        }
        parentTransform.position = Vector2.MoveTowards(parentTransform.position, _waypoints[_currentWaypointIndex].transform.position, Time.deltaTime * _speed);
        
    }
}
