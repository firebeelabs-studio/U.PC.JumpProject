using Cinemachine;
using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindCameraPath : MonoBehaviour
{
    public void Awake()
    {
        GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineTrackedDolly>().m_Path = FindObjectOfType<CinemachineSmoothPath>();
        
    }
}
