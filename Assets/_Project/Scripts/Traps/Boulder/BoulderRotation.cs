using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoulderRotation : MonoBehaviour
{
    [SerializeField] private float _rotateSpeed = 0.5f;
    private Vector2 _startingPosition;
    private Transform _currentPosition;
    private bool _isGoingRight;
    private float _positionCheck;

    private void Start()
    {
        _startingPosition = transform.position;
        _currentPosition = transform;
    }

    void Update()
    {
        _positionCheck = _startingPosition.x - _currentPosition.transform.position.x;
        _isGoingRight = (_positionCheck > 0) ? true : false;

        if (_isGoingRight)
        {
            transform.Rotate(0, 0, 360 * _rotateSpeed);
        }
        else
        {
            transform.Rotate(0, 0, 360 * - _rotateSpeed);
        }
    }
}
