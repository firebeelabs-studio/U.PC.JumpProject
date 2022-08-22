using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformRotate : MonoBehaviour
{
    [SerializeField] private float _timeToRotate = 3f;
    [SerializeField] private float _rotateSpeed = 2f;
    private float _rotateTimer;
    private bool _rotated = false;
    [SerializeField] float eulerAngZ;

    void Update()
    {
        eulerAngZ = transform.localEulerAngles.z;
        _rotateTimer += Time.deltaTime;

        //Rotate180Deg();

        if (_rotateTimer >= _timeToRotate && _rotated == false)
        {
            RotateOnZAxis(180);
            //_rotated = true;

            if (transform.eulerAngles.z.Equals(180)) //&& _rotated == true)
            {
                //_rotated = false;
                _rotateTimer = 0;
                _rotated = true;
            }
        }
        if (_rotateTimer >= _timeToRotate && _rotated == true)
        {
            RotateOnZAxis(0);

            if (transform.eulerAngles.z <= 0) //&& _rotated == true)
            {
                //_rotated = false;
                _rotateTimer = 0;
                _rotated = false;
            }

        }
    }

    // private void RotatePlatform()
    // {
    //     transform.Rotate(0, 0, Mathf.Lerp(0, 90, _rotateSpeed * Time.deltaTime));
    // }

    private void RotateOnZAxis(int degrees)
    {
        //transform.rotation = Vector3.RotateTowards(transform.rotation, Quaternion.Euler(0,_rotationY,0), 5 * Time.deltaTime);
        //transform.RotateTowards(transform.rotation, Quaternion.Euler(0,_rotationY,0), 5 * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, degrees), Time.deltaTime * _rotateSpeed);
    }
}
