using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSettings : MonoBehaviour
{
    public static CameraSettings Instance { get; private set; }
    public float CameraSize;
    private void Awake()
    {
        Instance = this;
    }
}
