using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularMovingPlatforms : MonoBehaviour
{
    [SerializeField] private float _radius;
    [SerializeField]private float _speed;
    [SerializeField] private Transform[] _platforms;
    private float _angleStep;

    private void Start()
    {
        _angleStep = Mathf.PI * 2 / _platforms.Length; //dividing full circle angles (radians) by number of platforms
        for (int i = 0; i < _platforms.Length; i++)
        {
            float angle = i * _angleStep; //setting the starting angle depending on index
            Vector2 newPos = new Vector2(transform.position.x + Mathf.Cos(angle) * _radius, transform.position.y + Mathf.Sin(angle) * _radius); //creating new vector for each platform's position
            _platforms[i].position = newPos;
            if (_platforms[i].TryGetComponent<PlatformCircularEffector>(out PlatformCircularEffector effector))
            {
                effector.Angle = angle;
                effector.Radius = _radius;
                effector.Speed = _speed;
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _radius);
    }
}
