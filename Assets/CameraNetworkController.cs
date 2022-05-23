using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraNetworkController : NetworkBehaviour
{
    [SerializeField] private GameObject _cameras;
    public override void OnStartClient()
    {
        base.OnStartClient();
        if (base.IsOwner)
        {
            _cameras.SetActive(true);
        }
    }
}
