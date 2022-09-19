using UnityEngine;

public class FollowCircularRandomPoint : MonoBehaviour
{
    [SerializeField] private AnimationCurve _animationCurve;
    [SerializeField] private float _radius, _speed, _minNewPosDistance, _newPosTimeDelay;

    private Vector2 _circleCenter, _startPos, _newPos, _randomPos;
    private float _timer, _progress;

    void Start()
    {
        _circleCenter = transform.position;
        _startPos = transform.position;
        _newPos = transform.position;
        if (_minNewPosDistance > _radius)
        {
            _minNewPosDistance = _radius;
        }
    }

    void Update()
    {
        _progress = Mathf.InverseLerp(_startPos.x, _newPos.x, transform.position.x);
        transform.position = Vector3.MoveTowards(transform.position, _newPos, _speed * _animationCurve.Evaluate(_progress) * Time.deltaTime);

        if ((Vector2)transform.position == _newPos)
        {
            
            //adds a litle delay before setting newPos
            _timer += Time.deltaTime;

            if (_timer < _newPosTimeDelay) return;

            _timer = 0;
            _startPos = _newPos;

            //calculates new position inside the circle
            _randomPos = _circleCenter + (Vector2)Random.insideUnitSphere * _radius;

            if (Vector2.Distance(transform.position, _randomPos) < _minNewPosDistance)
            {
                _randomPos = ReturnPointOnCircle(transform.position, _randomPos, _minNewPosDistance);
                _newPos = _circleCenter + _randomPos;
            }
            else
            {
                _newPos = _randomPos;
            }
        }
    }

    // this method is responsible for making new position distance higher than minimum value
    private Vector2 ReturnPointOnCircle(Vector2 circleCenter, Vector2 posInsideCircle, float radius)
    {
        float factor = radius / Vector2.Distance(circleCenter, posInsideCircle);
        float distanceX = factor * (posInsideCircle.x - circleCenter.x);
        float distanceY = factor * (posInsideCircle.y - circleCenter.y);
        Vector2 posOnCircle = new Vector2(distanceX, distanceY);
        return posOnCircle;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
        {
            Gizmos.DrawWireSphere(transform.position, _radius);
        }
        else
        {
            Gizmos.DrawWireSphere(_circleCenter, _radius);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _minNewPosDistance);
        }
    }
#endif
}
