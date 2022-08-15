using UnityEngine;

public class CircularSaw : MonoBehaviour
{
    [SerializeField] TypeOfMotion _motion;
    
    [Header("PENDULUM")]
    [SerializeField] private AnimationCurve _curve;
    [SerializeField] private float _maxRotateSpeed;
    [SerializeField] private float _angleRange;
    private Quaternion _maxSwingQuaternion, _targetQuaternion, _startQuaternion = Quaternion.Euler(0, 0, 0);
    private float _startPos, _targetAngle, _progress, _currentAngle;

    [Header("CIRCULAR")]
    [SerializeField] private float _rotateSpeed;
    [SerializeField] private bool _isRotatingClockwise;
    private int _direction;

    private enum TypeOfMotion
    {
        Pendulum,
        Circular
    }

    private void Start()
    {
        if (_motion == TypeOfMotion.Pendulum)
        {
            _targetAngle = _angleRange / 2;
        }
        else
        {
            if (_isRotatingClockwise)
            {
                _direction = -1;
            }
            else
            {
                _direction = 1;
            }
        }
    }

    private void FixedUpdate()
    {
        if (_motion == TypeOfMotion.Pendulum)
        {
            PendulumMotion();
        }
        else
        {
            CircularMotion();
        }
    }

    private void PendulumMotion()
    {
        // changes the direction when hits target and transforms it into quaternions
        // set _targetQuaterion prevents wrong rotation, when the set angle is > 180
        if (transform.rotation == _maxSwingQuaternion)
        {
            _startPos = _targetAngle;
            _targetAngle = -1 * _targetAngle;
            _targetQuaternion = _startQuaternion;
        }
        else if (transform.rotation == _startQuaternion)
        {
            _targetQuaternion = _maxSwingQuaternion;
        }

        _maxSwingQuaternion = Quaternion.Euler(0, 0, _targetAngle);

        // it calculates the eulerAngles into degree (eulerAngles are clamped in 0-360)
        if (transform.rotation.eulerAngles.z < 180)
        {
            _currentAngle = transform.rotation.eulerAngles.z;
        }
        else
        {
            _currentAngle = transform.rotation.eulerAngles.z - 360;
        }

        // progress returns a current % of path, that saw has passed
        // then it is transfered into curve.Evaluate and returns a acceleration
        _progress = Mathf.InverseLerp(_startPos, _targetAngle, _currentAngle);

        // rotates object to the target position
        transform.rotation = Quaternion.RotateTowards(transform.rotation, _targetQuaternion, _maxRotateSpeed * _curve.Evaluate(_progress));
    }

    private void CircularMotion()
    {
        transform.Rotate(0, 0, _direction * _rotateSpeed);
    }
}
