using Cinemachine;
using FishNet.Object;
using UnityEngine;
using TarodevController;
using System.Collections;

public class CameraController : NetworkBehaviour
{
    private float _maxZoom;
    private float _zoomSpeed;
    private float _camZOffset;
    private bool _shouldZoom;
    private float _defaultZoom;
    private CinemachineVirtualCamera _cam;
    private CinemachineTrackedDolly _camBody;
    private PlayerController _playerController;

    private void Awake()
    {
        _defaultZoom = CameraSettings.Instance.CameraSize;
        _shouldZoom = CameraSettings.Instance.ShouldZoom;
        _cam = GetComponent<CinemachineVirtualCamera>();
        _playerController = GetComponentInParent<PlayerController>();
        _camBody = _cam.GetCinemachineComponent<CinemachineTrackedDolly>();
    }
    private void Start()
    {
        if (_shouldZoom)
        {
            _maxZoom = CameraSettings.Instance.MaxZoom;
            _zoomSpeed = CameraSettings.Instance.ZoomSpeed;
            _camZOffset = CameraSettings.Instance.CamZOffset;
        }
    }
    public override void OnStartClient()
    {
        base.OnStartClient();
        if (base.IsOwner)
        {
            _cam.m_Lens.OrthographicSize = CameraSettings.Instance.CameraSize;
        }
    }
    private void LateUpdate() => ZoomOutWhileFalling(_playerController.VelocityY);

    private void ZoomOutWhileFalling(float velocity)
    {
        if (!_shouldZoom) return;
        var currentVelocity = velocity;
        if (currentVelocity < 0)
        {
            if (currentVelocity < -50f) //checks if player has enough speed (probably need to change it; max vel is currently about -61.0...)
            {
                _cam.m_Lens.OrthographicSize = Mathf.MoveTowards(_cam.m_Lens.OrthographicSize, (_defaultZoom + _maxZoom), _zoomSpeed * Time.deltaTime); //smoothly changes the size of lens (need to check it on build if its smooth enough)
                _camBody.m_PathOffset.z = Mathf.MoveTowards(_camBody.m_PathOffset.z, _camZOffset, _zoomSpeed * Time.deltaTime); //moves the camera by the offset to let the player see incoming obstacles
            }

        }
        else
        {
            _cam.m_Lens.OrthographicSize = Mathf.MoveTowards(_cam.m_Lens.OrthographicSize, _defaultZoom, (_zoomSpeed+(_zoomSpeed*1.5f)) * Time.deltaTime); //smoothly zooms back to the normal view
            _camBody.m_PathOffset.z = Mathf.MoveTowards(_camBody.m_PathOffset.z, 0, _zoomSpeed * Time.deltaTime); //moves camera back to original position
        }
        
    }















    //out of date
    #region BouncerCameraZoomOut
    //public static bool shouldZoom = false;
    //public void ZoomOut() 
    //{
    //    _elapsedTime += Time.deltaTime;
    //    float fixedDruation = _zoomDuration / _elapsedTime;
    //    if (shouldZoom && _playerController.Input.JumpHeld)
    //    {
    //        _cam.m_Lens.OrthographicSize = Mathf.Lerp(_cam.m_Lens.OrthographicSize, _defaultZoom +_maxZoom, fixedDruation);  //zooming out
    //    }
    //    else
    //    {
    //        _cam.m_Lens.OrthographicSize = Mathf.Lerp(_cam.m_Lens.OrthographicSize, _defaultZoom, fixedDruation);    //zooming in
    //        shouldZoom = false;
    //    }
    //}
    #endregion
}
