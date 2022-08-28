using UnityEngine;

public class TrackDrawer : MonoBehaviour
{
    [SerializeField] private GameObject _joint, _track;
    [SerializeField] private Transform _saw;
    [SerializeField] private float _speed = 1;
    [SerializeField] private bool _isTrackLooped;
    [SerializeField] private Vector2[] _points;

    public Vector2[] Points
    {
        get
        {
            return _points;
        }
    }
    public float Speed => _speed;
    public bool IsTrackLooped => _isTrackLooped;

    private void Start()
    {
        for (int i = 0; i < _points.Length; i++)
        {
            Instantiate(_joint, (Vector2)transform.position + _points[i], Quaternion.identity, transform);
        }

        for (int i = 0; i < _points.Length - 1; i++)
        {
            SetTrack(_points[i + 1], _points[i]);
        }

        if (_isTrackLooped)
        {
            SetTrack(_points[^1], _points[0]);
        }
    }

    private void SetTrack(Vector2 previousWaypoint, Vector2 nextWaypoint)
    {
        // calculates rotation for single instance of track
        Quaternion trackRotation = CalculateRotationBetween2Points(previousWaypoint, nextWaypoint);

        // midPos calculates the middle between 2 waypoints;
        Vector2 midPos = new Vector2((previousWaypoint.x + nextWaypoint.x) / 2, (previousWaypoint.y + nextWaypoint.y) / 2);

        // creates instantiate of the single track and put it in _midPos
        SpriteRenderer sprite = Instantiate(_track, (Vector2)transform.position + midPos, trackRotation, transform).GetComponent<SpriteRenderer>();

        // calculates the length of the track and set it
        float length = Vector2.Distance(previousWaypoint, nextWaypoint);
        sprite.size = new Vector2(length, 1);
    }

    private Quaternion CalculateRotationBetween2Points(Vector2 firstWaypoint, Vector2 secondWaypoint)
    {
        float angle;
        // calculates the vector between 2 points
        Vector2 direction = firstWaypoint - secondWaypoint;

        // fixes the angle value properly - values in inspector are different than Vector2.Angle returns
        if (firstWaypoint.y - secondWaypoint.y > 0)
        {
            angle = Vector2.Angle(Vector2.right, direction);
        }
        else
        {
            angle = -Vector2.Angle(Vector2.right, direction);
        }

        return Quaternion.Euler(0, 0, angle);
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying) return;
        Vector2 curPos = _saw.transform.position;
        Vector2 previous = curPos + _points[0];
        for (int i = 0; i < _points.Length; i++)
        {
            Vector2 next = _points[i] + curPos;
            Gizmos.DrawWireSphere(next, 0.2f);
            Gizmos.DrawLine(previous, next);

            previous = next;

            if (_isTrackLooped && i == _points.Length - 1) Gizmos.DrawLine(next, curPos + _points[0]);
        }
    }
}
