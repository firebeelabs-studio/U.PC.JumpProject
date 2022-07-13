using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollower : MonoBehaviour
{
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private JumpSimulation _jumpSimulation;
    [SerializeField] private Rigidbody2D _rb;

    private Vector2 _currentPosition;
    private int _currentPoint;
    private int _pointLimit = 1000;

    private void Awake()
    {
        // the value of _currentPoint is -1, because in FixedUpdate _lineRenderer.positionCount starts from 1
        transform.rotation = Quaternion.Euler(0, 0, 0);
        _currentPoint = -1;
    }

    void FixedUpdate()
    {
        if (!_jumpSimulation.isSimulationActive) return;
        if (_currentPoint > _pointLimit) return;

        // total position count must be bigger than current point every frame (+1 doesnt work, also i set +2)
        // drawes a line based on fake player's rigidbody's position
        _lineRenderer.positionCount = _currentPoint + 2;
        _currentPoint++;
        _currentPosition = _rb.transform.position;
        _lineRenderer.SetPosition(_currentPoint, _currentPosition);
    }
    public void ResetDrawing()
    {
        _currentPoint = -1;
        _lineRenderer.positionCount = 0;
    }
}
