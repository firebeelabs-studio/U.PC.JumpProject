using UnityEngine;

public class TransformPathFollower : MonoBehaviour, IWaypointPath
{
    [SerializeField] private float _speed = 1;
    [SerializeField] private bool _isTrackLooped;
    [SerializeField] private Vector2[] _points;

    private Vector2 _startPos;
    private int _index;
    private bool _ascending;

    public Vector2[] Points
    {
        get
        {
            return _points;
        }
    }

    public bool IsTrackLooped => _isTrackLooped;

    void Start()
    {
        _startPos = transform.position;
    }

    private void Update()
    {
        Vector2 target = _points[_index] + _startPos;
        transform.position = Vector2.MoveTowards(transform.position, target, _speed * Time.deltaTime);

        if ((Vector2)transform.position == target)
        {
            _index = _ascending ? _index + 1 : _index - 1;
            if (_index >= _points.Length)
            {
                if (_isTrackLooped)
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
        Vector2 curPos = transform.position;
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
