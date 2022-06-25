using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class CameraSettings : MonoBehaviour
{
    public static CameraSettings Instance { get; private set; }
    [SerializeField] private float _cameraSize;
    [Space(15)]
    [Header("Zooming")]
    [SerializeField] private bool _shouldZoom;
    [SerializeField] private float _maxZoom = 15f;
    [SerializeField] private float _zoomSpeed = 45f;
    [SerializeField] private float _camZOffset = 20f;
    public bool ShouldZoom => _shouldZoom;
    public float MaxZoom => _maxZoom;
    public float ZoomSpeed => _zoomSpeed;
    public float CamZOffset => _camZOffset;
    public float CameraSize => _cameraSize;
    [Space(15)]
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
