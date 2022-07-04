using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomAreaDollyTrack : MonoBehaviour
{
    private CinemachineSmoothPath _dollyTrack;
    private List<WaypointsToZoom> _zoomWaypoints = new();
    
    private CinemachineTrackedDolly _cameraBody;
    private CinemachineVirtualCamera _vcam;
    private float _defaultZoom;
    private float _zoomSpeed;
    private int _previousWaypoint, _nextWaypoint;
    private bool _shouldZoom = true;
    private float _targetSize;

    private void Awake()
    {
        _vcam = GetComponent<CinemachineVirtualCamera>();
        _cameraBody = _vcam.GetCinemachineComponent<CinemachineTrackedDolly>();
        _dollyTrack = FindObjectOfType<CinemachineSmoothPath>();
        _zoomWaypoints = CameraSettings.Instance.zoomWaypoints;
        _defaultZoom = CameraSettings.Instance.CameraSize;
        _zoomSpeed = CameraSettings.Instance.ZoomSpeed;
    }
    private void Start()
    {
        if (_zoomWaypoints.Count == 0)
        {
            enabled = false;
        }
        if (_zoomWaypoints.Count > _dollyTrack.m_Waypoints.Length)
        {
            _zoomWaypoints.RemoveRange((int)_dollyTrack.m_Waypoints.Length, (int)(_zoomWaypoints.Count - _dollyTrack.m_Waypoints.Length));
        }
        foreach (WaypointsToZoom waypoint in _zoomWaypoints)
        {
            if (waypoint.waypointNumber < 0 || waypoint.waypointNumber == 0)
            {
                _zoomWaypoints.Remove(waypoint);
            }
            if (waypoint.zoomValue < 0)
            {
                waypoint.zoomValue *= -1;
            }
        }
        _previousWaypoint = _zoomWaypoints[0].waypointNumber;
        _nextWaypoint = _zoomWaypoints[1].waypointNumber;
    }
    private void Update()
    {
        float currentPosition = _cameraBody.m_PathPosition;
        CheckPosition(currentPosition);
        print($"previous: {_previousWaypoint}");
        print($"next: {_nextWaypoint}");
    }
    private void Zoom(int waypointNumber)
    {
        
        if (_zoomWaypoints[waypointNumber].zoomType == WaypointsToZoom.ZoomType.ZoomIn)
        {
            _targetSize = _defaultZoom + _zoomWaypoints[waypointNumber].zoomValue;
        }
        else
        {
            _targetSize = _defaultZoom - _zoomWaypoints[waypointNumber].zoomValue;
        }
        if (_vcam.m_Lens.OrthographicSize == _targetSize) return;
        _vcam.m_Lens.OrthographicSize = Mathf.MoveTowards(_vcam.m_Lens.OrthographicSize, _targetSize, _zoomSpeed * Time.deltaTime);
    }
    private void ZoomReset()
    {
        _vcam.m_Lens.OrthographicSize = Mathf.MoveTowards(_vcam.m_Lens.OrthographicSize, _defaultZoom, _zoomSpeed * Time.deltaTime);
    }
    private void CheckPosition(float currentPos)
    {
        if ((currentPos < _zoomWaypoints[0].waypointNumber || currentPos > _zoomWaypoints[^1].waypointNumber)) //check if position of camera is before/after the first/last waypoint
        {
            if (_vcam.m_Lens.OrthographicSize == _defaultZoom) //check if zoom is set to default
            {
                return;
            }
            else
            {
                ZoomReset();
            }
        }
        else
        {
            if (currentPos >= _previousWaypoint && currentPos < _nextWaypoint) // do the zoom after passing waypoint
            {
                Zoom(_previousWaypoint);
                print(_previousWaypoint);
            }
            if (currentPos > _nextWaypoint) //check if passed next waypoint
            {
                if (_nextWaypoint < _zoomWaypoints[^1].waypointNumber)
                {
                    ChangeWaypoints(true);
                }
            }
            else if (currentPos < _previousWaypoint) //check if passed previous waypoint
            {
                if (_previousWaypoint > _zoomWaypoints[0].waypointNumber)
                {
                    ChangeWaypoints(false);
                }
            }
        }
        
    }
    private void ChangeWaypoints(bool doIncrement)
    {
        if (doIncrement)
        {
            _previousWaypoint += 1;
            _nextWaypoint += 1;
        }
        else
        {
            _previousWaypoint -= 1;
            _nextWaypoint -= 1;
        }
    }
}
