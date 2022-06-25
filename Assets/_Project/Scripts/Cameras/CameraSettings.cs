using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSettings : MonoBehaviour
{
    public static CameraSettings Instance { get; private set; }
    public float CameraSize;
    [Header("Zooming")]
    [Space(10)]
    public bool ShouldZoom;
    public float MaxZoom = 15f;
    public float ZoomSpeed = 45f;
    public float CamZOffset = 20f;
    [Space(10)]
    public bool ShouldParallax;
    public enum ParallaxAxis
    {
        Vertical,
        Horizontal
    }
    public ParallaxAxis Axis;
    private void Awake()
    {
        Instance = this;
    }
}
