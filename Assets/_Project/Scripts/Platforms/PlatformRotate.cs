using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlatformRotate : MonoBehaviour
{
    [SerializeField] private float _startDelay, _rotationTime = 0.5f, _timeToRotate = 3f, _shakeAmplitude = 5f, _shakeDuration = 1f; 
    [SerializeField] private int _shakeFrequency = 10;
    [SerializeField] private bool _isRotatingClockvise;
    private float _rotateTimer;
    private int _rotationAngle;
    private bool _isRotated;

    private void Start()
    {
        if (_isRotatingClockvise)
        {
            _rotationAngle = -179;
        }
        else
        {
            _rotationAngle = 179;
        }

        _rotateTimer = -_startDelay;
    }

    private void Update()
    {
        _rotateTimer += Time.deltaTime;

        if (_rotateTimer >= _timeToRotate && !_isRotated)
        {
            transform.DOPunchRotation(new Vector3(0, 0, _shakeAmplitude), _shakeDuration, _shakeFrequency, 1f).OnComplete(() => { transform.DORotate(new Vector3(0, 0, _rotationAngle), _rotationTime); ; });
            _rotateTimer = 0;
            _isRotated = true;
        }
        if (_rotateTimer >= _timeToRotate && _isRotated)
        {
            transform.DOPunchRotation(new Vector3(0, 0, _shakeAmplitude), _shakeDuration, _shakeFrequency, 1f).OnComplete(() => { transform.DORotate(new Vector3(0, 0, 0), _rotationTime); ; });
            _rotateTimer = 0;
            _isRotated = false;
        }
    }
}
