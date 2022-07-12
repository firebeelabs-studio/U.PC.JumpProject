using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using FishNet.Connection;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using TMPro;
using UnityEngine;

//TODO: make this script server only
public class Timer : NetworkBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _timerText;
    private float _starTime;
    private bool _isRunStarted;
    private bool _initialized;
    [SyncVar(OnChange = nameof(On_TimeChange))]
    private float _timeInSeconds;

    private Dictionary<NetworkConnection, float> _finishes = new();
    
    public override void OnStartNetwork()
    {
        base.OnStartNetwork();
        _initialized = true;
        //temp
//        _timerText = GameObject.Find("TimerText").GetComponent<TextMeshProUGUI>();
    }

    [Server]
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

    public void EndRun(NetworkConnection conn, int playerCount)
    {
        //prevents running this code few times
        if (!_isRunStarted) return;
        
        if (!_finishes.ContainsKey(conn))
        {
            _finishes.Add(conn, _timeInSeconds);
        }

        if (_finishes.Count >= playerCount)
        {
            _isRunStarted = false;
            int temp = 1;
            foreach (var finisher in _finishes.Values)
            {
                print($"Place {temp} time: {finisher}");
                temp++;
            }
        }
    }
}
