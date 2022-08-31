using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlatformRotate : MonoBehaviour
{
    [SerializeField] 
    private float _timeToRotate = 3f;
    private float _rotateTimer;
    private bool _rotated;

    private void Update()
    {
        _rotateTimer += Time.deltaTime;

        if (_rotateTimer >= _timeToRotate && !_rotated)
        {
            transform.DOPunchRotation(new Vector3(0, 0, 10), 1f, 10, 1f).OnComplete(() => { transform.DORotate(new Vector3(0, 0, 180), 2f); ; });
            _rotateTimer = 0;
            _rotated = true;
        }
        if (_rotateTimer >= _timeToRotate && _rotated)
        {
            transform.DOPunchRotation(new Vector3(0, 0, 10), 1f, 10, 1f).OnComplete(() => { transform.DORotate(new Vector3(0, 0, 0), 2f); ; });
            _rotateTimer = 0;
            _rotated = false;
        }
    }
}
