using Cinemachine;
using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindVirtualCamera : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _cam;
    public  void Awake()
    {

            _cam.m_Follow = gameObject.transform;
            _cam.m_Priority = 10;

    }
}
