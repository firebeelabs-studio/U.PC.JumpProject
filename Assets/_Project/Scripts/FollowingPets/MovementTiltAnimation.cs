using UnityEngine;

public class MovementTiltAnimation : MonoBehaviour
{
    [SerializeField] private float _maxTilt, _tiltSpeed;
    private float _lastPosX, _checkDirection;
    private int _inputX;

    protected virtual void Start()
    {
        _lastPosX = transform.position.x;
    }

    protected virtual void Update()
    {
        _checkDirection = transform.position.x - _lastPosX;
        _lastPosX = transform.position.x;
        _inputX = _checkDirection > 0 ? 1 : -1;

        var targetRotationVector = new Vector3(0, 0, -Mathf.Lerp(-_maxTilt, _maxTilt, Mathf.InverseLerp(-1, 1, _inputX)));
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(targetRotationVector), _tiltSpeed * Time.deltaTime);
    }
}
