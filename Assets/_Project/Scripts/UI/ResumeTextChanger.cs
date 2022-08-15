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
        SetupThresholdsDescending();
        _timeText.text = $"Your time: {(int)_endLevelTimers.TimeInSeconds}";
        _previousTimeText.text = _endLevelTimers.Times.Count > 1 ? $"Previous time: {(int)_endLevelTimers.Times[^2]}s" : "Your first try was Swamptastic!";
        StartCoroutine(SetupStars());
    }

    private IEnumerator SetupStars()
    {
        yield return new WaitForSeconds(.25f);
        for (int i = 0; i < _thresholds.Count; i++)
        {
            _stars[i].SetActive(_endLevelTimers.TimeInSeconds <= _thresholds[i]);
            _stars[i].GetComponent<StarAnim>().RunPunchAnimation();
            yield return new WaitForSeconds(.75f);
        }
    }

    private void SetupThresholdsDescending()
    {
        _thresholds.Sort();
        _thresholds.Reverse();
    }
}
