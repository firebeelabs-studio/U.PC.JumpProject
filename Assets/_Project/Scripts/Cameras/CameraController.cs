using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform _visualObjToFollowAndLookAt;
    private float _maxZoom;
    private float _zoomSpeed;
    private float _camZOffset;
    private bool _shouldZoom;
    private float _defaultZoom;
    private CinemachineVirtualCamera _vcam;
    private CinemachineTrackedDolly _camBody;
    private MapOverview _mapOverview;
    private void Start()
    {
        _vcam = GetComponent<CinemachineVirtualCamera>();
        _camBody = _vcam.GetCinemachineComponent<CinemachineTrackedDolly>();
    }

    [ContextMenu("StartMapOverview")]
    private void StartMapOverview()
    {
        CinemachineSmoothPath dollyTrack = ZoomAreaManagement.Instance.DollyTrack;
        CinemachineVirtualCamera vcam = _vcam;
        _mapOverview = MapOverview.Instance;
        if (_mapOverview != null && dollyTrack != null && vcam != null)
        {
            _mapOverview.StartMapOverview(dollyTrack, vcam, _visualObjToFollowAndLookAt);
        }
    }
}
