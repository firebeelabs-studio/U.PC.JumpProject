using UnityEngine;

public class CircularRandomPoint : MonoBehaviour
{
    [SerializeField] private AnimationCurve _animationCurve;
    [SerializeField] private float _range, _speed, _minDistance, _newPosDelay;

    private Vector2 _circleCenter;
    private Vector2 _startPos, _newPos, _randomPos;
    private float _newPosX, _newPosY, _current, _timer, _progress;

    void Start()
    {
        _circleCenter = transform.position;
        _startPos = transform.position;
        _newPos = transform.position;
    }

    void Update()
    {

        _progress = Mathf.InverseLerp(_startPos.x, _newPos.x, transform.position.x);
        transform.position = Vector3.MoveTowards(transform.position, _newPos, _speed * _animationCurve.Evaluate(_progress));

        if ((Vector2)transform.position == _newPos)
        {
            //adds a litle delay before setting newPos
            _timer += Time.deltaTime;

            if (_timer < _newPosDelay) return;

            _timer = 0;
            _startPos = _newPos;

            //calculates new position and increseases it by minDistance
            _randomPos = Random.insideUnitSphere * _range;
            _newPosX = _randomPos.x > 0 ? _newPosX = _randomPos.x + _minDistance : _newPosX = -_randomPos.x -_minDistance;
            _newPosY = _randomPos.y > 0 ? _newPosY = _randomPos.y + _minDistance : _newPosY = -_randomPos.y - _minDistance;

            _newPos = _circleCenter + new Vector2(_newPosX, _newPosY);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_circleCenter, _range + _minDistance);
    }
#endif
}
