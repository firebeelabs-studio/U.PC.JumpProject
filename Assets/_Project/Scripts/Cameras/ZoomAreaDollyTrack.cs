using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        if (_zoomWaypoints.Count < 2) //disables script if there's no at least 2 waypoints to zoom
        {
            enabled = false;
        }
        else
        {
            if (_zoomWaypoints.Count > _dollyTrack.m_Waypoints.Length-1) //if there's more waypoints in list than waypoints for real (dolly track) it removes excess
            {
                _zoomWaypoints.RemoveRange((int)_dollyTrack.m_Waypoints.Length-1, (int)(_zoomWaypoints.Count - _dollyTrack.m_Waypoints.Length));
            }
            foreach (WaypointsToZoom waypoint in _zoomWaypoints)
            {
                if (waypoint.zoomValue < 0) //if entered the negative value it changes it to positive
                {
                    waypoint.zoomValue *= -1;
                }
                if (waypoint.zoomType == WaypointsToZoom.ZoomType.ResetToDefault)
                {
                    waypoint.zoomValue = 0;
                }
            }
            _zoomWaypoints = _zoomWaypoints.OrderBy(x => x.waypointNumber).ToList(); //sorts list ascending (0 -> 1 -> 2...)
            if (_zoomWaypoints[^1].waypointNumber == _dollyTrack.m_Waypoints.Length-1)
            {
                _zoomWaypoints.Remove(_zoomWaypoints[^1]);
            }
            WaypointsToZoom lastEmptyWaypoint = new(_zoomWaypoints[^1].waypointNumber + 1, 0,WaypointsToZoom.ZoomType.ResetToDefault);
            _zoomWaypoints.Add(lastEmptyWaypoint); //adds empty waypoint at the end of list
            _previousWaypoint = _zoomWaypoints[0].waypointNumber;
            _nextWaypoint = _zoomWaypoints[1].waypointNumber;
        }
    }
    private void Update()
    {
        float currentPosition = _cameraBody.m_PathPosition;
        CheckPosition(currentPosition);
    }
    private void Zoom(int waypointNumber)
    {
        int index = waypointNumber - _zoomWaypoints[0].waypointNumber; //always gets first index of list
        if (_zoomWaypoints[index].zoomType == WaypointsToZoom.ZoomType.ResetToDefault)
        {
            ZoomReset();
        }
        else
        {
            if (_zoomWaypoints[index].zoomType == WaypointsToZoom.ZoomType.ZoomIn)
            {
                _targetSize = _defaultZoom - _zoomWaypoints[index].zoomValue;
            }
            else
            {
                _targetSize = _defaultZoom + _zoomWaypoints[index].zoomValue;
            }
            if (_vcam.m_Lens.OrthographicSize == _targetSize) return;
            _vcam.m_Lens.OrthographicSize = Mathf.MoveTowards(_vcam.m_Lens.OrthographicSize, _targetSize, _zoomSpeed * Time.deltaTime);
        }
    }
    private void ZoomReset()
    {
        if (_vcam.m_Lens.OrthographicSize != _defaultZoom)
        {
            _vcam.m_Lens.OrthographicSize = Mathf.MoveTowards(_vcam.m_Lens.OrthographicSize, _defaultZoom, _zoomSpeed * Time.deltaTime);
        }
    }
    private void CheckPosition(float currentPos)
    {
        if ((currentPos < _zoomWaypoints[0].waypointNumber || currentPos >= _zoomWaypoints[^1].waypointNumber)) //checks if position of camera is before/after the first/last waypoint
        {
            if (_vcam.m_Lens.OrthographicSize == _defaultZoom) return; //checks if zoom is set to default
            else
            {
                ZoomReset();
            }
        }
        else
        {
            if (currentPos >= _previousWaypoint && currentPos < _nextWaypoint) //checks if current position if between two waypoints
            {
                Zoom(_previousWaypoint);
            }
            else if (_previousWaypoint != _zoomWaypoints[0].waypointNumber && currentPos < _previousWaypoint) //checks if player passed previous ( <--O-- ) waypoint and decrease it's numbers by 1
            {
                ChangeWaypoints(-1);
            }
            else if (_nextWaypoint != _zoomWaypoints[^1].waypointNumber && currentPos > _nextWaypoint) //checks if player passed next ( --O--> ) waypoint and increase it's numbers by 1
            {
                ChangeWaypoints(1);
            }
        }
    }
    private void ChangeWaypoints(int changeValue)
    {
        _previousWaypoint += changeValue;
        _nextWaypoint += changeValue;
    }
}
