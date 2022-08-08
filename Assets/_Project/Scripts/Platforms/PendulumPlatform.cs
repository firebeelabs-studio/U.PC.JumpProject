using System.Collections;
using System.Collections.Generic;
using TarodevController;
using UnityEngine;

public class PendulumPlatform : MonoBehaviour
{
    [SerializeField] private LineRenderer _lineRenderer;
    private Vector2 _nextPos;
    private Vector2 _lastPos;
    private Vector2 _change;
    [SerializeField] private Transform _lrStartPos, _lrEndPos;
    private void Start()
    {
        _lineRenderer.positionCount = 2;
    }
    private void Update()
    {
        _lineRenderer.SetPosition(0,_lrStartPos.position);
        _lineRenderer.SetPosition(1,_lrEndPos.position);
    }
    private void FixedUpdate()
    {
        _nextPos = transform.position;
        _change = _lastPos - _nextPos;
        _lastPos = _nextPos;
    }
    public Vector2 EvaluateEffector()
    {
        return -_change;
    }
}