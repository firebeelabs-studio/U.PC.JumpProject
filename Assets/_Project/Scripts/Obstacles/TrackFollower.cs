using UnityEngine;

public class TrackFollower : MonoBehaviour
{
    [SerializeField] private TrackDrawer _track;

    private Vector2 _startPos;
    private int _index;
    private bool _ascending;

    void Start()
    {
        transform.localPosition = _track.Points[0];
    }

    private void Update()
    {
        Vector2 target = _track.Points[_index] + _startPos;
        transform.localPosition = Vector2.MoveTowards(transform.localPosition, target, _track.Speed * Time.deltaTime);

        if ((Vector2)transform.localPosition == target)
        {
            _index = _ascending ? _index + 1 : _index - 1;
            if (_index >= _track.Points.Length)
            {
                if (_track.IsTrackLooped)
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
}
