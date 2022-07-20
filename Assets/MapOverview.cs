using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapOverview : MonoBehaviour
{
    public static MapOverview Instance { get; private set; }
    public bool OverviewEnded => _overviewEnded;
    [SerializeField] private float _speedForOverviewCamera;
    private Transform _visualObjToFollowAndLookAt;
    private bool _overviewEnded, _overviewStarted;
    private CinemachineSmoothPath _dollyTrack;
    private CinemachineVirtualCamera _vcam;
    private CinemachineTrackedDolly _cameraBody;
    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        OverviewInProgress();
    }

    public void StartMapOverview(CinemachineSmoothPath dollyTrack, CinemachineVirtualCamera vcam, Transform visualObjTransform)
    {
        _overviewStarted = true;
        _vcam = vcam;
        _cameraBody = vcam.GetCinemachineComponent<CinemachineTrackedDolly>();
        _dollyTrack = dollyTrack;
        _cameraBody.m_PathPosition = 0;
        _visualObjToFollowAndLookAt = visualObjTransform;
    }

    private void OverviewInProgress()
    {
        if (_overviewStarted)
        {
            if (_cameraBody.m_PathPosition < _dollyTrack.PathLength)
            {
                _cameraBody.m_PathPosition += Time.deltaTime * _speedForOverviewCamera;
            }
            else if (_cameraBody.m_PathPosition >= _dollyTrack.PathLength && !_overviewEnded)
            {
                EndMapOverview();
            }
        }
    }

    private void EndMapOverview()
    {
        _vcam.m_Follow = _visualObjToFollowAndLookAt;
        _vcam.m_LookAt = _visualObjToFollowAndLookAt;
        _overviewStarted = false;
        _overviewEnded = true;
    }
}
