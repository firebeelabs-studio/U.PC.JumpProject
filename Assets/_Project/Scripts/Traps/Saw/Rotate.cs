using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private bool _isRotatingClockwise;

    private int _direction;

    private void Start()
    {
        _direction = (_isRotatingClockwise) ? -1 : 1;
    }

    private void Update()
    {
        transform.Rotate(0, 0, _direction * _speed * Time.deltaTime);
    }
}
