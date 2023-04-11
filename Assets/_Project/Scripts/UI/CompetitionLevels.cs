using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompetitionLevels : MonoBehaviour
{
    [SerializeField] private string _level1;
    [SerializeField] private string _level2;
    [SerializeField] private string _level3;
    [SerializeField] private string _level4;
    [SerializeField] private string _level5;
    [SerializeField] private Button[] _buttons = new Button[5];
    private string[] _levelNames;
    private void Start()
    {
        _levelNames = new[]
        {
            _level1,
            _level2,
            _level3,
            _level4,
            _level5,
        };
        for (int i = 0; i < _levelNames.Length; i++)
        {
            var i1 = i;
            _buttons[i].onClick.AddListener(()=> LoadingScreenCanvas.Instance.LoadScene(_levelNames[i1]));
        }
    }
}
