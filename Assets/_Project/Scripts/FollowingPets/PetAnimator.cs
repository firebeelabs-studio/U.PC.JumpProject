using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetAnimator : MonoBehaviour
{
    [SerializeField] private float _maxTilt;
    [SerializeField] private float _tiltSpeed;

    private float _lastPosX;
    private float _checkDirection;
    private int _inputX;

    void Start()
    {
        _lastPosX = transform.position.x;
    }

    void Update()
    {
        _checkDirection = transform.position.x - _lastPosX;
        _lastPosX = transform.position.x;

        // flip the sprite
        if (_checkDirection != 0) 
        { 
            transform.localScale = new Vector3(_checkDirection > 0 ? 1 : -1, 1, 1);
            _inputX = _checkDirection > 0 ? 1 : -1;

            var targetRotVector = new Vector3(0, 0, -Mathf.Lerp(-_maxTilt, _maxTilt, Mathf.InverseLerp(-1, 1, _inputX)));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(targetRotVector), _tiltSpeed * Time.deltaTime);
        }


    }



}
