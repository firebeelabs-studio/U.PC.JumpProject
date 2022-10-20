using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScoreReader : MonoBehaviour
{
    [SerializeField] private static List<PlayerLevelData> _levelData;

    private void Awake()
    {
        //Init _levelData cuz we don't have db rn
        _levelData.Add(new PlayerLevelData
        {
            TimeInSeconds = 120f,
            LevelName = "Jungle 1",
            UserName = "test 1"
        });
        _levelData.Add(new PlayerLevelData
        {
            TimeInSeconds = 100f,
            LevelName = "Jungle 1",
            UserName = "test 2"
        });
        _levelData.Add(new PlayerLevelData
        {
            TimeInSeconds = 110f,
            LevelName = "Jungle 1",
            UserName = "test 3"
        })
            ;_levelData.Add(new PlayerLevelData
        {
            TimeInSeconds = 90f,
            LevelName = "Jungle 1",
            UserName = "test 4"
        });
        //db call
    }

    public static List<PlayerLevelData> GetDataForLevel(string levelName)
    {
        return _levelData.Where(x => x.LevelName == levelName).OrderBy(p => p.TimeInSeconds).ToList();
    }

    public static void AddPlayerScore(PlayerLevelData newScore)
    {
        if (_levelData.Where(x => x.UserName == newScore.UserName && x.LevelName == newScore.LevelName).Count() != 0)
        {
            PlayerLevelData lastscore = _levelData.Where(x => x.UserName == newScore.UserName && x.LevelName == newScore.LevelName).Single();
            if (lastscore.TimeInSeconds > newScore.TimeInSeconds)
            {
                int index =_levelData.IndexOf(lastscore);
                _levelData[index] = newScore;
            }
        }
        else
        {
            _levelData.Add(newScore);
        }
    }
}
