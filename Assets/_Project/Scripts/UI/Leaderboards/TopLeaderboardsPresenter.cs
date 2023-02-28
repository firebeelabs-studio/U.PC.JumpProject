using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TopLeaderboardsPresenter : MonoBehaviour
{
    [SerializeField] private LeaderboardsManagerClient _leaderboardsManagerClient;
    [SerializeField] private List<LeaderboardsPlayerRowTemplate> _scoreRows;
    [SerializeField] private LeaderboardsPlayerRowTemplate _yourScoreRow;
    private List<LeaderboardEntry> _top3AndYourScoreData;

    private void OnEnable()
    {
        //_leaderboardsManagerClient.BestScoresForCertainLevelLoaded += On_BestScoresForCertainLevelLoaded;
    }

    private void OnDisable()
    {
        //_leaderboardsManagerClient.BestScoresForCertainLevelLoaded -= On_BestScoresForCertainLevelLoaded;
    }

    private void ReloadData()
    {
        for (int i = 0; i < _scoreRows.Count; i++)
        {
            _scoreRows[i].DisplayData(_top3AndYourScoreData[i]);
        }
        _yourScoreRow.DisplayData(_top3AndYourScoreData.Last());
    }

    public void DownloadTop3AndYourScore(string levelName)
    {
        //TODO: CALL
    }

    public void On_BestScoresForCertainLevelLoaded()
    {
        //_top3AndYourScoreData = new List<LeaderboardEntry>(_leaderboardsManagerClient.BestScoresForCertainLevel);
        _top3AndYourScoreData = new List<LeaderboardEntry>()
        {       
            new()
            {
                Player = new LootLockerPlayerData()
                {
                    Name = "Papryk"
                },
                Rank = 1,
                Score = 123
            },
            new()
            {
                Player = new LootLockerPlayerData()
                {
                    Name = "Kryspin"
                },
                Rank = 2,
                Score = 139
            },
            new()
            {
                Player = new LootLockerPlayerData()
                {
                    Name = "Cyc"
                },
                Rank = 3,
                Score = 223
            },
            new()
            {
                Player = new LootLockerPlayerData()
                {
                    Name = "Miszel"
                },
                Rank = 12,
                Score = 376
            }
        };
        ReloadData();
    }
}
