using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoulderRotation : MonoBehaviour
{
    [SerializeField] private float _rotateSpeed = 0.5f;
    [SerializeField] private Transform _startingPosition;
    [SerializeField] private Transform _currentPosition;
    private bool _isGoingRight;
    private float _positionCheck;

    void Update()
    {
        _positionCheck = _startingPosition.transform.position.x - _currentPosition.transform.position.x;
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
