using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSettings : MonoBehaviour
{
    public static CameraSettings Instance { get; private set; }
    public float CameraSize { get; private set; }
    [Header("Zooming")]
    [Space(10)]
    public bool ShouldZoom;
    public float MaxZoom { get; private set; } = 15f;
    public float ZoomSpeed { get; private set; } = 45f;
    public float CamZOffset { get; private set; } = 20f;
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
