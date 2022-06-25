using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSettings : MonoBehaviour
{
    public static CameraSettings Instance { get; private set; }
    public float CameraSize { get; private set; }
    [Header("Zooming")]
    [Space(10)]
    public readonly bool ShouldZoom;
    public readonly float MaxZoom = 15f;
    public readonly float ZoomSpeed = 45f;
    public readonly float CamZOffset = 20f;
    [Space(10)]
    public readonly bool ShouldParallax;
    public enum ParallaxAxis
    {
        Vertical,
        Horizontal
    }
    public readonly ParallaxAxis Axis;
    private void Awake()
    {
        Instance = this;
    }
}
