using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SetSummaryPlaces : MonoBehaviour
{
    [SerializeField] private TMP_Text _1stPlaceNick;
    [SerializeField] private TMP_Text _1stPlaceTime;
    [SerializeField] private TMP_Text _2ndPlaceNick;
    [SerializeField] private TMP_Text _2ndPlaceTime;
    [SerializeField] private TMP_Text _3rdPlaceNick;
    [SerializeField] private TMP_Text _3rdPlaceTime;

    [SerializeField] private TMP_Text _playerTime;

    private List<PlayerLevelData> _levelData;

    private void UpdatePlaces(float timeInSeconds)
    {
        ScoreReader.AddPlayerScore(new PlayerLevelData
        {
            LevelName = "Jungle 1",
            TimeInSeconds = timeInSeconds,
            UserName = "Player test"
        });
        _levelData = ScoreReader.GetDataForLevel("Jungle 1");

        _1stPlaceNick.text = _levelData[0].UserName;
        _1stPlaceTime.text = _levelData[0].TimeInSeconds.ToString();
        _2ndPlaceNick.text = _levelData[1].UserName;
        _2ndPlaceTime.text = _levelData[1].TimeInSeconds.ToString();
        _3rdPlaceNick.text = _levelData[2].UserName;
        _3rdPlaceTime.text = _levelData[2].TimeInSeconds.ToString();

        _playerTime.text = timeInSeconds.ToString();

    }
}
