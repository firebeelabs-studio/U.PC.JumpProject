using UnityEngine;

public class GuillotineMove : MonoBehaviour
{
    [SerializeField] Transform _blade;
    [SerializeField] Transform _lines;
    [SerializeField] SpriteRenderer _linesSprite;
    [SerializeField] BoxCollider2D _collider;
    [SerializeField] private float _fallSpeed, _returnSpeed, _delay, _startDelay;

    private float _speed, _distance, _timer, _angle = Mathf.PI;
    private Vector2 _endPos, _startPos, _startSize;

    private bool _runStarted;

    private void OnEnable()
    {
        StartRun.RunStart += On_RunStart;
    }

    private void On_RunStart()
    {
        _runStarted = true;
    }

    private void OnDisable()
    {
        StartRun.RunStart -= On_RunStart;
    }
    
    private void Start()
    {
        // set the lowest position of blade and the size of lines
        _endPos = _blade.transform.localPosition;
        _startSize = _linesSprite.size;

        // goes back to the start position
        _startPos = new Vector2(_blade.transform.localPosition.x, _lines.transform.localPosition.y - 0.38f);
        _linesSprite.size = new Vector2(_startSize.x, 0);

        //calculate the distance between 2 positions
        _distance = _startPos.y - _endPos.y;

        //set start speed
        _speed = _fallSpeed;
        _timer = _startDelay;

    }

    private void Update()
    {
        if (!_runStarted) return;

        if (_timer > 0)
        {
            _timer -= Time.deltaTime;
        }
        else
        {
            _angle += Time.deltaTime * _speed;
            MoveGuillotine();
        }

        if (_angle < 1.5f * Mathf.PI && _timer < 0)
        {
            _collider.enabled = true;
            _speed = _fallSpeed;
        }
        else if (_angle > 1.5f * Mathf.PI)
        {
            _collider.enabled = false;
            _speed = _returnSpeed;
        }
    }

    private void MoveGuillotine()
    {
        _blade.transform.localPosition = new Vector2(_startPos.x, _startPos.y + Mathf.Sin(_angle) * _distance);
        _linesSprite.size = new Vector2(_linesSprite.size.x, -Mathf.Sin(_angle) * _startSize.y);
        if (_angle >= 2 * Mathf.PI)
        {
            _angle = Mathf.PI;
            _timer = _delay;
        }
    }
}
