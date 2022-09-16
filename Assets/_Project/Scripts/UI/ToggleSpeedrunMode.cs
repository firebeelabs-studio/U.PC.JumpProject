using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleSpeedrunMode : MonoBehaviour
{
    public static event Action<bool> SpeedrunModeToggle;
    public static ToggleSpeedrunMode Instance;
    private Toggle _toggle;
    private void Awake()
    {
        Instance = this;
        _toggle = GetComponent<Toggle>();
    }
    private void Start()
    {
        if (PlayerPrefs.HasKey("SpeedrunMode"))
        {
            _toggle.isOn = PlayerPrefs.GetInt("SpeedrunMode") == 1;
        }
        else
        {
            PlayerPrefs.SetInt("SpeedrunMode", 0);
            _toggle.isOn = false;
        }

        _toggle.onValueChanged.AddListener((isOn) => { Toggle(isOn); });
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