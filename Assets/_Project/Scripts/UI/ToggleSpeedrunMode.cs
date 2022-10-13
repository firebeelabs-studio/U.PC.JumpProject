using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleSpeedrunMode : MonoBehaviour
{
    public static event Action<bool> SpeedrunModeToggle;
    public static ToggleSpeedrunMode Instance;
    [SerializeField] private AnimatedToggle _animToggle;
    private void Awake()
    {
        Instance = this;
        _animToggle.ToggleValueChanged += Toggle;
    }
    private void Start()
    {
        if (PlayerPrefs.HasKey("SpeedrunMode"))
        {
            _animToggle.SetStartToggle(PlayerPrefs.GetInt("SpeedrunMode") == 1);
        }
        else
        {
            PlayerPrefs.SetInt("SpeedrunMode", 0);
            _animToggle.SetStartToggle(false);
        }
    }
    public void Toggle(bool isOn)
    {
        SpeedrunModeToggle?.Invoke(isOn);
        if (isOn)
        {
            //enable speedrun mode
            PlayerPrefs.SetInt("SpeedrunMode", 1);

        }
        else
        {
            //disable speedrun
            PlayerPrefs.SetInt("SpeedrunMode", 0);
        }
    }
}