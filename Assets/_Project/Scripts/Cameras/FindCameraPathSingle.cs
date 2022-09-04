using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindCameraPathSingle : MonoBehaviour
{
    private void Start()
    {
        GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineTrackedDolly>().m_Path = FindObjectOfType<CinemachineSmoothPath>();
    }
}
