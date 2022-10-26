using UnityEngine;

namespace TarodevController {
    public class RigidbodyPathFollower : MonoBehaviour, IPlayerEffector, IWaypointPath
    {
        [SerializeField] private Vector2[] _points;
        [SerializeField] private float _speed = 1;
        [SerializeField] private bool _isTrackLooped;

        private Rigidbody2D _rb;
        private Vector2 _pos => _rb.position;
        private Vector2 _change, _startPos, _lastPos;
        private int _index;
        private bool _ascending;

        public Vector2[] Points
        {
            get { return _points; }
        }

        public bool IsTrackLooped => _isTrackLooped;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _startPos = _rb.position;
        }

        private void FixedUpdate()
        {
            var target = _points[_index] + _startPos;
            var newPos = Vector2.MoveTowards(_pos, target, _speed * Time.fixedDeltaTime);
            _rb.MovePosition(newPos);

            if (Vector2.Distance(_pos, target) < 0.1f)
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

            _change = _lastPos - newPos;
            _lastPos = newPos;
        }

        private void OnDrawGizmos()
        {
            if (Application.isPlaying) return;
            var curPos = (Vector2)transform.position;
            var previous = curPos + _points[0];
            for (var i = 0; i < _points.Length; i++)
            {
                var p = _points[i] + curPos;
                Gizmos.DrawWireSphere(p, 0.2f);
                Gizmos.DrawLine(previous, p);

                previous = p;

                if (_isTrackLooped && i == _points.Length - 1) Gizmos.DrawLine(p, curPos + _points[0]);
            }
        }

        public Vector2 EvaluateEffector()
        {
            return -_change; // * _speed;
        }
    }
}