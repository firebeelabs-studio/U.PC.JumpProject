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
        Debug.Log("x");
        Debug.Log(base.IsOwner);
        if (base.IsOwner)
        {
            Debug.Log("y");
            FindObjectOfType<CinemachineVirtualCamera>().m_Follow = gameObject.transform;
        }
    }
}
