using UnityEngine;

public class OldPetAnimator : MonoBehaviour
{
    [SerializeField] private PointFollower _pF;

    private float _maxTilt, _tiltSpeed;
    private float _lastPosX, _checkDirection;
    private int _inputX;
    private Vector2 _scale;


    void Start()
    {
        _scale = transform.localScale;
        _maxTilt = _pF.MaxTilt;
        _tiltSpeed = _pF.TiltSpeed;
        _lastPosX = transform.position.x;
    }

    void Update()
    {
        _checkDirection = transform.position.x - _lastPosX;
        _lastPosX = transform.position.x;

        // flip the sprite
        transform.localScale = new Vector3(_pF.IsGoingRight == true ? _scale.x : -_scale.x, _scale.y, 1);

        _inputX = _checkDirection > 0 ? 1 : -1;

        var targetRotationVector = new Vector3(0, 0, -Mathf.Lerp(-_maxTilt, _maxTilt, Mathf.InverseLerp(-1, 1, _inputX)));
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(targetRotationVector), _tiltSpeed * Time.deltaTime);
    }



}
