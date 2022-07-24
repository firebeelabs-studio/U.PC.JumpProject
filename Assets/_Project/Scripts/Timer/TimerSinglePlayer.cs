using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimerSinglePlayer : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _timerText;
    private float _starTime;
    private bool _isRunStarted;
    private float _timeInSeconds;

    private List<float> times = new();
    //Timer display
    private float _minutes, _seconds;

    private void Awake()
    {
        _timerText = GameObject.Find("TimerText").GetComponent<TextMeshProUGUI>();
        StartRun.RunStart += On_RunStart;
        LevelCompletedSingle.RunFinish += On_RunFinish;
    }

    private void Update()
    {
        if (!_isRunStarted) return;
        _timeInSeconds = Time.time - _starTime;
        _minutes = (int)(_timeInSeconds / 60f);
        _seconds = (int)(_timeInSeconds % 60f);
        _timerText.SetText(_minutes.ToString("00") + ":" + _seconds.ToString("00"));
    }

    private void On_RunStart()
    {
        _starTime = Time.time;
        _isRunStarted = true;
    }

    private void On_RunFinish()
    {
        _isRunStarted = false;
        if (_timeInSeconds < 50)
        {
            if (SceneManager.GetActiveScene().name != "MatitekLevel2")
            {
                SceneManager.LoadScene("MatitekLevel2");
            }
        }
        times.Add(_timeInSeconds);
    }
}
