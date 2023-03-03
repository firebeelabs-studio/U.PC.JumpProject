using System;
using UnityEngine;

public class PlatformCircularEffector : MonoBehaviour, IPlayerEffector
{
    public float Angle;
    public float Radius;
    public float Speed;
    [SerializeField] private Transform _axisObj;
    private Rigidbody2D _rb;
    private Vector2 _change, _lastPos, _nextPos;
    private float _nextPosX, _nextPosY;
    private bool _runStarted;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        StartRun.RunStart += On_RunStart;
    }

    private void OnDisable()
    {
        StartRun.RunStart -= On_RunStart;
    }

    private void FixedUpdate()
    {
        if (!_runStarted) return;

        Angle += Time.fixedDeltaTime * Speed; //increasing angle value
        
        _nextPosX = _axisObj.position.x + Mathf.Cos(Angle) * Radius; //calculating new x position around the axis (parent object)
        _nextPosY = _axisObj.position.y + Mathf.Sin(Angle) * Radius; //calculating new y position around the axis (parent object)
        _nextPos = new(_nextPosX, _nextPosY); //applying the new position
        _rb.MovePosition(_nextPos);
        if (Angle >= 360f)
        {
            Angle = 0;
        }
        _change = _nextPos - _lastPos; //calculating the difference between last position and next position
        _lastPos = _nextPos;
    }

    public Vector2 EvaluateEffector()
    {
        return _change;
    }

    private void On_RunStart()
    {
        _runStarted = true;
    }
}
