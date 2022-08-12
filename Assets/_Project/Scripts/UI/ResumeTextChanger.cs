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
        _timeText.text = $"Your time: {_endLevelTimers.TimeInSeconds}";
        _previousTimeText.text = _endLevelTimers.Times.Count > 1 ? $"Previous time: {_endLevelTimers.Times[^2]}" : "Your first try was Swamptastic!";
        if (_endLevelTimers.TimeInSeconds <= _thresholds[0])
        {
            _stars[2].SetActive(true);
        }
        else if (_endLevelTimers.TimeInSeconds <= _thresholds[1])
        {
            _stars[1].SetActive(true);
        }
        else
        {
            _stars[0].SetActive(true);
        }
    }
}
