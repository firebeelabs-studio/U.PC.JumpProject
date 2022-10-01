using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Cameras;
using Cinemachine;
using UnityEngine;
using DG.Tweening;

public class ZoomAreaManagement : MonoBehaviour
{
    public static ZoomAreaManagement Instance { get; private set; }
    private CinemachineSmoothPath _dollyTrack;
    public CinemachineSmoothPath DollyTrack => _dollyTrack;
    private CinemachineTrackedDolly _cameraBody;
    private CinemachineVirtualCamera _vcam;
    private float _defaultZoom;
    private float _currentZoom;
    private float _currentWaypointPos;
    [SerializeField] private List<ZoomPoint> _zoomPoints = new();
    private int _currentIndex = 0;
    private bool _checked;
    private List<ZoomPoint> point;
    private void Awake()
    {
        Instance = this;
        _vcam = GetComponent<CinemachineVirtualCamera>();
        _cameraBody = _vcam.GetCinemachineComponent<CinemachineTrackedDolly>();
        _dollyTrack = FindObjectOfType<CinemachineSmoothPath>();
    }

    private void Start()
    {
        _defaultZoom = CameraSettings.Instance.CameraSize;
        _currentZoom = _defaultZoom;
        _vcam.m_Lens.OrthographicSize = _defaultZoom;
        point = _zoomPoints.OrderBy(x => x.WaypointNumber).ToList();
    }
    private void ChangeZoom(float targetSize)
    {
        DOVirtual.Float(_vcam.m_Lens.OrthographicSize, targetSize, 1f, s => _vcam.m_Lens.OrthographicSize = s);
    }

    private void Update()
    {
        _currentWaypointPos = _cameraBody.m_PathPosition;
        
        if (_currentWaypointPos >= point[_currentIndex].WaypointNumber + 1)
        {
            if (_currentIndex == point.Count - 1) return;
            
            _currentIndex++;
            _currentZoom = _vcam.m_Lens.OrthographicSize;
        }
        else if (_currentWaypointPos >= point[_currentIndex].WaypointNumber)
        {
            var completionPercent = _currentWaypointPos - (int) _currentWaypointPos;
            var currentZoom = (point[_currentIndex].ZoomValue - _currentZoom)  * completionPercent;
            _vcam.m_Lens.OrthographicSize = _currentZoom + currentZoom;
            _checked = false;
        }
    }
    
    //useless atm
    private void GetClosestWaypoint()
    {
        ZoomPoint closestPoint = new ZoomPoint
        {
            WaypointNumber = 100
        };
        for (int i = 0; i < _zoomPoints.Count - 1; i++)
        {
            var distance =  _zoomPoints[i].WaypointNumber -_currentWaypointPos;
            if (distance < 0) continue;

            if (distance <= closestPoint.WaypointNumber - _currentWaypointPos)
            {
                closestPoint = _zoomPoints[i];
                _currentIndex = i;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("ZoomArea"))
        {
            if (collision.TryGetComponent(out ZoomArea zoomArea))
            {
                ChangeZoom(zoomArea.TargetSize);
            }
        }
        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("ZoomArea"))
        {
            ChangeZoom(_defaultZoom);
        }
    }
}
