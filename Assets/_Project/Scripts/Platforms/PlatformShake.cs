using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformShake : MonoBehaviour
{
    private float _passedAngle = 15.0f;
    private float _minusAngle = - 15.0f;
    [SerializeField] private float _speed = 2f;
    [SerializeField] private float _timeBetweenShakes = 3f;
    public float Timer;
    private Quaternion _angleQuaternion;
    private Quaternion _passedAngleQuaternion;
    private Quaternion _minusAngleQuaternion;
    private Quaternion _transformQuaternion;
    private bool _shakedLeft = false;
    private bool _shakedRight = false;
    private bool _zeroAngle = true;

    private void Start()
    {
        _passedAngleQuaternion = Quaternion.Euler(0, 0, _passedAngle);
        _minusAngleQuaternion = Quaternion.Euler(0, 0, _minusAngle);
        _transformQuaternion = Quaternion.Euler(transform.rotation.x, transform.rotation.y, transform.rotation.z);
    }

    private void Update() 
    {
        Timer += Time.deltaTime;
        // if (_timer >= _timeBetweenShakes)
        // {
        //     if (_zeroAngle)
        //     {
        //         Shake(_passedAngle);

        //         if (transform.rotation.z == _passedAngleQuaternion.z)
        //         {
        //             _zeroAngle = false;
        //             _shakedLeft = true;
        //         }
        //     }
        //     else if (_shakedLeft)
        //     {
        //         Shake(_minusAngle);

        //         if (transform.rotation.z == (_minusAngleQuaternion.z))
        //         {
        //             _shakedLeft = false;
        //             _shakedRight = true;
        //         }
        //     }
        //     else if (_shakedRight)
        //     {
        //         Shake(0);

        //         if (transform.rotation == Quaternion.Euler(0, 0, 0))
        //         {
        //             _shakedRight = false;
        //             _zeroAngle = true;
        //             _timer = 0;
        //         }
        //     }
        // }
    }

    public void OneShake()
    {
        if (Timer >= _timeBetweenShakes)
        {
            if (_zeroAngle)
            {
                Shake(_passedAngle);

                if (transform.rotation.z == _passedAngleQuaternion.z)
                {
                    _zeroAngle = false;
                    _shakedLeft = true;
                }
            }
            else if (_shakedLeft)
            {
                Shake(_minusAngle);

                if (transform.rotation.z == (_minusAngleQuaternion.z))
                {
                    _shakedLeft = false;
                    _shakedRight = true;
                }
            }
            else if (_shakedRight)
            {
                Shake(0);

                if (transform.rotation == Quaternion.Euler(0, 0, 0))
                {
                    _shakedRight = false;
                    _zeroAngle = true;
                    Timer = 0;
                }
            }
        }
    }

    private void Shake(float degeree)
    {
        _angleQuaternion = Quaternion.Euler(0, 0, degeree);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, _angleQuaternion, _speed * Time.deltaTime);
    }
}
