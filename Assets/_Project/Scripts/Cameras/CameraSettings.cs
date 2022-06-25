using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSettings : MonoBehaviour
{
    public static CameraSettings Instance { get; private set; }
    public float CameraSize;
    public bool ShouldZoom;
    private void Awake()
    {
        Instance = this;
    }
}
