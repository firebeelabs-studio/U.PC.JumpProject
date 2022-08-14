using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResumeTextChanger : MonoBehaviour
{
    [SerializeField] private TMP_Text _timeText;
    [SerializeField] private TMP_Text _previousTimeText;
    [SerializeField] private TMP_Text _timeNeededForNextStarText;
    [SerializeField] private TimerSinglePlayer _endLevelTimers;
    [SerializeField] private GameObject[] _stars;
    [SerializeField] private List<float> _thresholds = new();
    private void OnEnable()
    {
        _thresholds.Sort();
        _timeText.text = $"Your time: {(int)_endLevelTimers.TimeInSeconds}";
        _previousTimeText.text = _endLevelTimers.Times.Count > 1 ? $"Previous time: {(int)_endLevelTimers.Times[^2]}s" : "Your first try was Swamptastic!";
        SetupStars();
    }

    private void SetupStars()
    {
        //not sure atm which solution is easier to read, to make it work we have to change stars order in array
        // for (int i = 0; i < _thresholds.Count; i++)
        // {
        //     _stars[i].SetActive(_endLevelTimers.TimeInSeconds <= _thresholds[i]);
        // }
        
        _stars[2].SetActive(_endLevelTimers.TimeInSeconds <= _thresholds[0]);
        _stars[1].SetActive(_endLevelTimers.TimeInSeconds <= _thresholds[1]);
        _stars[0].SetActive(_endLevelTimers.TimeInSeconds <= _thresholds[2]);
    }
}
