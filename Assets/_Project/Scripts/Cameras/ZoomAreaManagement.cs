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
    }
    private void ChangeZoom(float targetSize)
    {
        DOVirtual.Float(_vcam.m_Lens.OrthographicSize, targetSize, 1f, s => _vcam.m_Lens.OrthographicSize = s);
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
