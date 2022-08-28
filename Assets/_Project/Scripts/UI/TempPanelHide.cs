using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempPanelHide : MonoBehaviour
{
    [SerializeField] private GameObject _timer;
    [SerializeField] private GameObject _resumePanel;

    private void Awake()
    {
        FinishSinglePlayer.RunFinish += OnRunFinish;
    }

    private void OnDisable()
    {
        FinishSinglePlayer.RunFinish -= OnRunFinish;
    }

    private void OnRunFinish()
    {
        _timer.SetActive(false);
        _resumePanel.SetActive(true);
    }
    
}
