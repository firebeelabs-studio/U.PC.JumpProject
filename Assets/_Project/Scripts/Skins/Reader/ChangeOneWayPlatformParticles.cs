using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeOneWayPlatformParticles : MonoBehaviour
{
    [SerializeField] private Color _blueColor;
    [SerializeField] private Color _yellowColor;
    [SerializeField] private Color _turquoiseColor;
    [SerializeField] private Color _purpleColor;
    [SerializeField] private Color _greenColor;
    private ParticleSystem _particleSystem;
    private static readonly int BaseColor = Shader.PropertyToID("_BaseColor");

    private void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
    }

    private void Start()
    {
        switch (SwampieTypeReader.SwampieType)
        {
            case SwampieSkin.SwampieType.Blue:
                _particleSystem.GetComponent<Renderer>().material.SetColor(BaseColor, _blueColor);
                break;
            case SwampieSkin.SwampieType.Green:
                _particleSystem.GetComponent<Renderer>().material.SetColor(BaseColor, _greenColor);
                break;
            case SwampieSkin.SwampieType.Yellow:
                _particleSystem.GetComponent<Renderer>().material.SetColor(BaseColor, _yellowColor);;
                break;
            case SwampieSkin.SwampieType.Turquoise:
                _particleSystem.GetComponent<Renderer>().material.SetColor(BaseColor, _turquoiseColor);;
                break;
            case SwampieSkin.SwampieType.Purple:
                _particleSystem.GetComponent<Renderer>().material.SetColor(BaseColor, _purpleColor);;
                break;
        }
    }
}
