using Cinemachine;
using FishNet.Object;
using UnityEngine;
using TarodevController;
using System.Collections;

public class CameraController : NetworkBehaviour
{
    [SerializeField] private float _maxZoom = 5;
    [SerializeField] private float _smooth = 2;
    private float _defaultZoom;
    private CinemachineVirtualCamera _cam;
    private PlayerController _playerRb;
    public static bool canZoom = false;

    private void Awake()
    {
        _defaultZoom = CameraSettings.Instance.CameraSize;
        _cam = FindObjectOfType<CinemachineVirtualCamera>();
        _playerRb = GetComponentInParent<PlayerController>();
    }
    public override void OnStartClient()
    {
        base.OnStartClient();
        if (base.IsOwner)
        {
            GetComponent<CinemachineVirtualCamera>().m_Lens.OrthographicSize = CameraSettings.Instance.CameraSize;
        }
    }
    public void ZoomOut()
    {
        if (_playerRb.Grounded && canZoom)
        {
            canZoom = false;
        }
        if (canZoom)
        {
            _cam.m_Lens.OrthographicSize = Mathf.Lerp(_cam.m_Lens.OrthographicSize, _defaultZoom +_maxZoom, Time.deltaTime * _smooth);
        }
        else
        {
            _cam.m_Lens.OrthographicSize = Mathf.Lerp(_cam.m_Lens.OrthographicSize, _defaultZoom, Time.deltaTime * _smooth);
        }
    }
    private void Update() => ZoomOut();
}
