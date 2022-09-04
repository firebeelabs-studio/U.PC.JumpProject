using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSettings : MonoBehaviour
{
    public static CameraSettings Instance { get; private set; }
    [SerializeField] private float _cameraSize;
    public float CameraSize => _cameraSize;
    [Space(15)]
    [Header("Parallax")]
    [SerializeField] private GameObject _backgroundPrefab;
    public GameObject BackgroundPrefab => _backgroundPrefab;
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
    public void InstantiateNewBackground(Transform referenceObj)
    {
        GameObject newBg = Instantiate(_backgroundPrefab);
        newBg.GetComponent<ParallaxBackground>().ParallaxReferenceTransform = referenceObj;
        newBg.SetActive(true);
    }
}
