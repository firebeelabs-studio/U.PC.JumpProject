using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using TMPro;
using UnityEngine;

public class Timer : NetworkBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _timerText;
    private float _starTime;
    private bool _isRunStarted;
    private bool _initialized;
    [SyncVar(OnChange = nameof(On_TimeChange))]
    private float _timeInSeconds;


    // private void OnEnable()
    // {
    //     FinishLevel.EndRun += EndRun;
    //     StartRun.RunStart += RunStart;
    // }
    //
    // private void OnDisable()
    // {
    //     FinishLevel.EndRun -= EndRun;
    //     StartRun.RunStart -= RunStart;
    // }
    public override void OnStartNetwork()
    {
        base.OnStartNetwork();
        _initialized = true;
        //temp
        _timerText = GameObject.Find("TimerText").GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (!_isRunStarted || !_initialized) return;

        if (IsServer)
        {
            _timeInSeconds = Time.time - _starTime;
        }
    }

    private void On_TimeChange(float prevValue, float newValue, bool asServer)
    {
        if (asServer) return;

        _timerText.text = _timeInSeconds.ToString();
    }

    public void RunStart()
    {
        _starTime = Time.time;
        _isRunStarted = true;
    }

    private void EndRun()
    {
        _isRunStarted = false;
    }
}
