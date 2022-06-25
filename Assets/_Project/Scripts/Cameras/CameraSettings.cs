using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSettings : MonoBehaviour
{
    public static CameraSettings Instance { get; private set; }
    public float CameraSize;
    [Space(10)]
    public bool ShouldZoom;
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
