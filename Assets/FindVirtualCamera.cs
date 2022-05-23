using Cinemachine;
using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindVirtualCamera : NetworkBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _cam;
    public override void OnStartClient()
    {
        base.OnStartClient();
        if (base.IsOwner)
        {
            _cam.m_Follow = gameObject.transform;
            _cam.m_Priority = 10;

            //can't do this like that cuz clients can't find eachothers cameras, but it's not a problem cuz cameras are rn in player prefab
            //FindObjectOfType<CinemachineVirtualCamera>().m_Follow = gameObject.transform;
        }
        else
        {
            //Note: without priority changes on client side clients have wrong cameras on them
            _cam.m_Priority = 0;
        }
    }
}
