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

    private void OnEnable()
    {
        _timeText.text = $"Your time: {_endLevelTimers.TimeInSeconds}";
        _previousTimeText.text = _endLevelTimers.Times.Count > 1 ? $"Previous time: {_endLevelTimers.Times[^2]}" : "Your first try was Swamptastic!";
    }
}
