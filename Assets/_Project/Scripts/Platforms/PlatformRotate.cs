using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformRotate : MonoBehaviour
{
    [SerializeField] private float _timeToRotate = 3f;
    [SerializeField] private float _rotateSpeed = 2f;
    private float _rotateTimer;
    private bool _rotated = false;

    void Update()
    {
        _rotateTimer += Time.deltaTime;

        if (_rotateTimer >= _timeToRotate && _rotated == false)
        {
            RotateOnZAxis(180);

            if (transform.eulerAngles.z.Equals(180))
            {
                _rotateTimer = 0;
                _rotated = true;
            }
        }

        if (_rotateTimer >= _timeToRotate && _rotated == true)
        {
            RotateOnZAxis(0);

            if (transform.eulerAngles.z <= 0)
            {
                _rotateTimer = 0;
                _rotated = false;
            }

        }
    }

    private void RotateOnZAxis(int degrees)
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, degrees), Time.deltaTime * _rotateSpeed);
    }
}
