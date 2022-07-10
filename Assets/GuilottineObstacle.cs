using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuilottineObstacle : MonoBehaviour
{
    [SerializeField] private Obstacle _obstacleType;
    [SerializeField] private float _speed;
    [SerializeField] private float _delay;
    [SerializeField] private float _startDelay;
    [SerializeField] private float _distance;
    [SerializeField] private GameObject _blade;
    [SerializeField] private GameObject _press;
    private enum Obstacle
    {
        Blade,
        Press
    }
    private Vector2 _startPos;
    private float _angle, _timer;

    private void Start()
    {
        if (_obstacleType == Obstacle.Blade)
        {
            _blade.SetActive(true);
        }
        else
        {
            _press.SetActive(true);
        }
        _startPos = transform.position;
        _timer = _startDelay;
    }
    private void Update()
    {
        if (_timer > 0)
        {
            _timer -= Time.deltaTime;
        }
        else
        {
            _angle += Time.deltaTime * _speed;
            if (_blade.activeInHierarchy)
            {
                MoveObstacle(_blade);
            }
            else
            {
                MoveObstacle(_press);
            }
        }
    }
    private void MoveObstacle(GameObject obstacle)
    {
        obstacle.transform.position = new Vector2(_startPos.x, _startPos.y + Mathf.Sin(_angle) * _distance);
        if (_angle >= 2 * Mathf.PI)
        {
            _angle = 0;
            _timer = _delay;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(0, -_distance));
    }
}
