using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomAreaDollyTrack : MonoBehaviour
{
    private CinemachineSmoothPath _dollyTrack;
    private List<WaypointsToZoom> _waypointsToZoom = new();
    
    private CinemachineTrackedDolly _cameraBody;
    private CinemachineVirtualCamera _cmcamera;
    private float _defaultZoom;
    private float _zoomSpeed;

    private void Awake()
    {
        _cmcamera = GetComponent<CinemachineVirtualCamera>();
        _cameraBody = _cmcamera.GetCinemachineComponent<CinemachineTrackedDolly>();
        _dollyTrack = FindObjectOfType<CinemachineSmoothPath>();
        _waypointsToZoom = CameraSettings.Instance.zoomWaypoints;
        _defaultZoom = CameraSettings.Instance.CameraSize;
        _zoomSpeed = CameraSettings.Instance.ZoomSpeed;
    }
    private void Start()
    {
        if (_waypointsToZoom.Count == 0)
        {
            enabled = false;
        }
        if (_waypointsToZoom.Count > _dollyTrack.m_Waypoints.Length)
        {
            _waypointsToZoom.RemoveRange((int)_dollyTrack.m_Waypoints.Length, (int)(_waypointsToZoom.Count - _dollyTrack.m_Waypoints.Length));
        }
        foreach (WaypointsToZoom waypoint in _waypointsToZoom)
        {
            if (waypoint.waypointNumber < 0)
            {
                waypoint.waypointNumber = 0;
                waypoint.zoomValue = 0;
            }
            if (waypoint.zoomValue < 0)
            {
                waypoint.zoomValue *= -1;
            }
        }
    }
    private void Update()
    {
        var currentPosition = (int)_cameraBody.m_PathPosition;
        if ((currentPosition < _waypointsToZoom[0].waypointNumber || currentPosition > _waypointsToZoom[^1].waypointNumber) && _cmcamera.m_Lens.OrthographicSize != _defaultZoom) return;
        for (int i = 0; i <= _waypointsToZoom.Count - 1; i++)
        {
            if (i + 1 > _waypointsToZoom.Count - 1) return;
            if (currentPosition >= _waypointsToZoom[i].waypointNumber && currentPosition < _waypointsToZoom[i+1].waypointNumber)
            {
                
                if (_waypointsToZoom[i].zoomType == WaypointsToZoom.ZoomType.ZoomIn)
                {
                    var targetSize = _defaultZoom - _waypointsToZoom[i].zoomValue;
                    if (_cmcamera.m_Lens.OrthographicSize > targetSize)
                    {
                        _cmcamera.m_Lens.OrthographicSize = Mathf.MoveTowards(_cmcamera.m_Lens.OrthographicSize, targetSize, _zoomSpeed * Time.deltaTime);
                    }
                }
                else
                {
                    var targetSize = _defaultZoom + _waypointsToZoom[i].zoomValue;
                    if (_cmcamera.m_Lens.OrthographicSize < targetSize)
                    {
                        _cmcamera.m_Lens.OrthographicSize = Mathf.MoveTowards(_cmcamera.m_Lens.OrthographicSize, targetSize, _zoomSpeed * Time.deltaTime);
                    }
                }
            }
            else
            {
                _cmcamera.m_Lens.OrthographicSize = Mathf.MoveTowards(_cmcamera.m_Lens.OrthographicSize, _defaultZoom, _zoomSpeed * Time.deltaTime);
            }
        }
    }
}
