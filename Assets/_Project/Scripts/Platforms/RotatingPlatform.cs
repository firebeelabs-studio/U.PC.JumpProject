using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingPlatform : MonoBehaviour
{
    [SerializeField] private float _timeToRotate = 10f;
    [SerializeField] private float _timeToRotateBack;
    [SerializeField] private float _timeToShake = 5f;
    [SerializeField] private GameObject _platform;
    private float _time;
    private float _shakeTime;
    private float _time2;

    private void Start() 
    {
        _timeToRotateBack = _timeToRotateBack + _timeToRotate;
    }

    void Update()
    {
        _time += Time.deltaTime;
        _time2 += Time.deltaTime;
        _shakeTime += Time.deltaTime;

        //if (_shakeTime >= _timeToShake)
        //{
        //    ShakePlatform(_platform.gameObject);
        //    _shakeTime = 0;
        //}

        if (_time >= _timeToRotate)
        {
            transform.Rotate(0, 0, 180);
            _time = 0;
        }
        
        if (_time2 >= _timeToRotateBack)
        {
            transform.Rotate(0, 0, 180);
            _time2 = 0;
        }
        //InvokeRepeating("RotatePlatform", _time, _time2);
    }

    //private void ShakePlatform(GameObject platform)
    //{
    //    platform.GetComponent<PlatformShake>().Shake();
    //}
}
