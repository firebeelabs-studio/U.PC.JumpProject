using UnityEngine;

public class TrackDrawer : MonoBehaviour
{
    [SerializeField] private IWaypointPath _waypointPath;
    [SerializeField] private GameObject _joint, _track;
    [SerializeField] private Transform _objectToFollow;

    private void Awake()
    {
        _waypointPath = GetComponentInChildren<IWaypointPath>();
    }

    private void Start()
    {
        for (int i = 0; i < _waypointPath.Points.Length; i++)
        {
            Instantiate(_joint, (Vector2)transform.position + _waypointPath.Points[i], Quaternion.identity, transform);
        }

        for (int i = 0; i < _waypointPath.Points.Length - 1; i++)
        {
            SetTrack(_waypointPath.Points[i + 1], _waypointPath.Points[i]);
        }

        if (_waypointPath.IsTrackLooped)
        {
            SetTrack(_waypointPath.Points[^1], _waypointPath.Points[0]);
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
        sprite.size = new Vector2(length, 5.12f);
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
}
