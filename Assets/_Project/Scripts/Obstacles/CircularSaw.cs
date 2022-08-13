using UnityEngine;

public class CircularSaw : MonoBehaviour
{
    [SerializeField] TypeOfMotion _motion;
    [SerializeField] private float _rotateSpeed;
    [SerializeField] private bool _isRotatingClockwise;
    private float _timer;
    private int _phase, _direction;
    private enum TypeOfMotion
    {
        Pendulum,
        Circular
    }

    private void Start()
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
        _timer += Time.fixedDeltaTime;
        if (_timer > 1f)
        {
            _phase++;
            _phase %= 4;
            _timer = 0;
        }
        if (_phase == 0)
        {
            transform.Rotate(0, 0, _rotateSpeed * (1 - _timer));
        }
        else if (_phase == 1)
        {
            transform.Rotate(0, 0, -_rotateSpeed * _timer);
        }
        else if (_phase == 2)
        {
            transform.Rotate(0, 0, -_rotateSpeed * (1 - _timer));
        }
        else if (_phase == 3)
        {
            transform.Rotate(0, 0, _rotateSpeed * _timer);
        }
    }

    private void CircularMotion()
    {
        transform.Rotate(0, 0, _direction * _rotateSpeed);
    }
}
