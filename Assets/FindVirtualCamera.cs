using Cinemachine;
using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindVirtualCamera : NetworkBehaviour
{
    public override void OnStartClient()
    {
        base.OnStartClient();
        if (base.IsOwner)
        {
            FindObjectOfType<CinemachineVirtualCamera>().m_Follow = gameObject.transform;
        }
    }
}
