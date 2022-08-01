using UnityEngine;

public class PetAnimator : MonoBehaviour
{
    [SerializeField] private PointFollower _pF;

    private float _maxTilt, _tiltSpeed;
    private float _lastPosX, _checkDirection;
    private int _inputX;

    void Start()
    {
        _maxTilt = _pF.MaxTilt;
        _tiltSpeed = _pF.TiltSpeed;
        _lastPosX = transform.position.x;
    }

    void Update()
    {
        _checkDirection = transform.position.x - _lastPosX;
        _lastPosX = transform.position.x;

        // flip the sprite
        transform.localScale = new Vector3(_checkDirection > 0 ? 1 : -1, 1, 1);
        _inputX = _checkDirection > 0 ? 1 : -1;

        var targetRotVector = new Vector3(0, 0, -Mathf.Lerp(-_maxTilt, _maxTilt, Mathf.InverseLerp(-1, 1, _inputX)));
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(targetRotVector), _tiltSpeed * Time.deltaTime);
    }



}
