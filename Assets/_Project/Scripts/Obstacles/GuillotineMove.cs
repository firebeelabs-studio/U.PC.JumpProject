using UnityEngine;

public class GuillotineMove : MonoBehaviour
{
    [SerializeField] Transform _blade;
    [SerializeField] SpriteRenderer _lines;
    [SerializeField] BoxCollider2D _collider;
    [SerializeField] private float _fallSpeed, _returnSpeed, _delay;

    private float _speed, _timer, _angle, _distance;
    private Vector2 _endPos, _startPos, _startSize;

    private void Start()
    {
        // set the lowest position of blade and the size of lines
        _endPos = _blade.transform.localPosition;
        _startSize = _lines.size;

        // goes back to the start position
        _startPos = new Vector2(_blade.transform.localPosition.x, 0.3f);
        _lines.size = new Vector2(_startSize.x, 0);

        //calculate the distance between 2 positions
        _distance = _startPos.y - _endPos.y;

        //set basic stats
        _angle = Mathf.PI;
        _speed = _fallSpeed;

        // a little delay on start prevent 1st fall without 'else if' in update below \/
        _timer = 0.5f;
    }

    private void Update()
    {
        print(_speed);
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
        else if (_angle > 1.5f)
        {
            _collider.enabled = false;
            _speed = _returnSpeed;
        }
    }

    private void MoveGuillotine()
    {
        _blade.transform.localPosition = new Vector2(_startPos.x, _startPos.y + Mathf.Sin(_angle) * _distance);
        _lines.size = new Vector2(_lines.size.x, -Mathf.Sin(_angle) * _startSize.y);
        if (_angle >= 2 * Mathf.PI)
        {
            _angle = Mathf.PI;
            _timer = _delay;
        }
    }
}
