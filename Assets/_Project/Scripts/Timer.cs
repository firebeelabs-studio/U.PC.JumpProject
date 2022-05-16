using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _timerText;
    private float _starTime;
    private bool _isRunStarted;

    private void OnEnable()
    {
        FinishLevel.EndRun += EndRun;
        StartRun.RunStart += RunStart;
    }

    private void OnDisable()
    {
        FinishLevel.EndRun -= EndRun;
        StartRun.RunStart -= RunStart;
    }

    private void Update()
    {
        if (!_isRunStarted) return;
        float t = Time.time - _starTime;
        string minutes = ((int)t / 60).ToString();
        string seconds = ((t % 60).ToString("f2", CultureInfo.InvariantCulture));

        _timerText.text = minutes + ":" + seconds;
    }

    private void RunStart()
    {
        _starTime = Time.time;
        _isRunStarted = true;
    }

    private void EndRun()
    {
        _isRunStarted = false;
    }
}
