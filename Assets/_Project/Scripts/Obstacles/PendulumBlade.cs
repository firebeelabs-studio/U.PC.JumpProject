using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PendulumBlade : MonoBehaviour
{
    [SerializeField] private float _swingSpeedAndDistance;
    private float _timer;
    private int _phase;

    private void FixedUpdate()
    {
        _timer += Time.fixedDeltaTime;
        if (_timer > 1f)
        {
            _phase++;
            _phase %= 4;
            _timer = 0;
        }
        if (_phase == 0)
        {
            transform.Rotate(0, 0, _swingSpeedAndDistance * (1 - _timer));
        }
        else if (_phase == 1)
        {
            transform.Rotate(0, 0, -_swingSpeedAndDistance * _timer);
        }
        else if (_phase == 2)
        {
            transform.Rotate(0, 0, -_swingSpeedAndDistance * (1 - _timer));
        }
        else if (_phase == 3)
        {
            transform.Rotate(0, 0, _swingSpeedAndDistance * _timer);
        }
    }
}
