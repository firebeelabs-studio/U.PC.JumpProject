using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlatformRotate : MonoBehaviour
{
    [SerializeField] private float _startDelay, _rotationTime = 0.5f, _topPosTime = 3f, _backPosTime = 2f, _shakeAmplitude = 5f, _shakeDuration = 1f; 
    [SerializeField] private int _shakeFrequency = 10;
    [SerializeField] private bool _isRotatingClockvise;
    private float _rotateTimer;
    private int _rotationAngle;
    private bool _isRotated;

    private bool _runStarted;

    private void OnEnable()
    {
        StartRun.RunStart += On_RunStart;
    }

    private void On_RunStart()
    {
        _runStarted = true;
    }

    private void OnDisable()
    {
        StartRun.RunStart -= On_RunStart;
    }
    
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
        if (!_runStarted) return;

        _rotateTimer += Time.deltaTime;

        if (_rotateTimer >= _topPosTime && !_isRotated)
        {
            transform.DOPunchRotation(new Vector3(0, 0, _shakeAmplitude), _shakeDuration, _shakeFrequency, 1f).OnComplete(() => { transform.DORotate(new Vector3(0, 0, _rotationAngle), _rotationTime); ; });
            _rotateTimer = 0;
            _isRotated = true;
        }
        if (_rotateTimer >= _backPosTime && _isRotated)
        {
            transform.DOPunchRotation(new Vector3(0, 0, _shakeAmplitude), _shakeDuration, _shakeFrequency, 1f).OnComplete(() => { transform.DORotate(new Vector3(0, 0, 0), _rotationTime); ; });
            _rotateTimer = 0;
            _isRotated = false;
        }
    }
}
