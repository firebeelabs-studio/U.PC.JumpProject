using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraNetworkController : MonoBehaviour
{
    [SerializeField] private GameObject _cameras;
    public void Awake()
    {
        _cameras.SetActive(true);
    }
}
