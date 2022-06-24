using Cinemachine;
using FishNet.Object;
using UnityEngine;
using TarodevController;
using System.Collections;

public class CameraController : NetworkBehaviour
{
    [SerializeField] private float _maxZoom;
    [SerializeField] private float _zoomDuration;
    private float _elapsedTime;
    private float _defaultZoom;
    private CinemachineVirtualCamera _cam;
    private PlayerController _playerController;
    
    private void Awake()
    {
        _defaultZoom = CameraSettings.Instance.CameraSize;
        _cam = FindObjectOfType<CinemachineVirtualCamera>();
        _playerController = GetComponentInParent<PlayerController>();
    }
    public override void OnStartClient()
    {
        base.OnStartClient();
        if (base.IsOwner)
        {
            GetComponent<CinemachineVirtualCamera>().m_Lens.OrthographicSize = CameraSettings.Instance.CameraSize;
        }
    }
    private void LateUpdate()
    {

    }

    private void ZoomOutWhileFalling()
    {

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
