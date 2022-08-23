using UnityEngine;

public class TrackCreator : MonoBehaviour
{
    [SerializeField] private GameObject _joint, _track;
    [SerializeField] private Transform _saw;
    [SerializeField] private float _speed = 1;
    [SerializeField] private bool _loop;
    [SerializeField] private Vector2[] _points;

    private Vector2 _startPos;
    private int _index;
    private bool _ascending;

    private void Start()
    {
        _startPos = _saw.transform.position;
        _saw.transform.localPosition = _points[0];

        for (int i = 0; i < _points.Length; i++)
        {
            Instantiate(_joint, (Vector2)transform.position + _points[i], Quaternion.identity, this.transform);
        }

        for (int i = 0; i < _points.Length - 1; i++)
        {
            SetTrack(_points[i + 1], _points[i]);
        }

        if (_loop)
        {
            SetTrack(_points[^1], _points[0]);
        }
    }

    private void SetTrack(Vector2 previousWaypoint, Vector2 nextWaypoint)
    {
        // calculates rotation of every instance of track
        Quaternion singleTrackRotation = CalculateRotationBetween2Points(previousWaypoint, nextWaypoint);

        // midPos calculates the middle between 2 waypoints;
        Vector2 midPos = new Vector2((previousWaypoint.x + nextWaypoint.x) / 2, (previousWaypoint.y + nextWaypoint.y) / 2);

        // creates instantiate of the single track and put it in _midPos
        SpriteRenderer sprite = Instantiate(_track, (Vector2)transform.position + midPos, singleTrackRotation, this.transform).GetComponent<SpriteRenderer>();

        // calculates the length of the track and set it
        float length = Vector2.Distance(previousWaypoint, nextWaypoint);
        sprite.size = new Vector2(length, 1);
    }

    private Quaternion CalculateRotationBetween2Points(Vector2 firstPoint, Vector2 secondPoint)
    {
        float angle;
        // calculates the vector between 2 points
        Vector2 direction = firstPoint - secondPoint;

        // fixes the angle value properly - values in inspector are different than Vector2.Angle returns
        if (firstPoint.y - secondPoint.y > 0)
        {
            angle = Vector2.Angle(Vector2.right, direction);
        }
        else
        {
            angle = -Vector2.Angle(Vector2.right, direction);
        }
        // turns angle into quaternion
        Quaternion trackRotationValue = Quaternion.Euler(0, 0, angle);

        return trackRotationValue;
    }

    private void Update()
    {
        var target = _points[_index] + _startPos;
        _saw.transform.position = Vector2.MoveTowards(_saw.transform.position, target, _speed * Time.deltaTime);

        if ((Vector2)_saw.transform.position == target)
        {
            _index = _ascending ? _index + 1 : _index - 1;
            if (_index >= _points.Length)
            {
                if (_loop)
                {
                    _index = 0;
                }
                else
                {
                    _ascending = false;
                    _index--;
                }
            }
            else if (_index < 0)
            {
                _ascending = true;
                _index = 1;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying) return;
        var curPos = (Vector2)_saw.transform.position;
        var previous = curPos + _points[0];
        for (var i = 0; i < _points.Length; i++)
        {
            var p = _points[i] + curPos;
            Gizmos.DrawWireSphere(p, 0.2f);
            Gizmos.DrawLine(previous, p);

            previous = p;

            if (_loop && i == _points.Length - 1) Gizmos.DrawLine(p, curPos + _points[0]);
        }
    }
}
