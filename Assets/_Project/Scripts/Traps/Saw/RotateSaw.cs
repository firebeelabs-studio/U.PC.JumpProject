using UnityEngine;

public class RotateSaw : MonoBehaviour
{
    [Range(0.2f,2)]
    [SerializeField] private float _speed;
    [SerializeField] private bool _isRotatingClockwise;

    private int _direction;

    private void Start()
    {
        _direction = (_isRotatingClockwise) ? -1 : 1;
    }

    private void Update()
    {
        transform.Rotate(0, 0, 1000 * _direction * _speed * Time.deltaTime);
    }
}
