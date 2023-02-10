using System;
using System.Collections;
using System.Collections.Generic;
using LootLocker.Requests;
using UnityEngine;

//TODO: Move it to serverside
public class LeaderboardsManager : MonoBehaviour
{
    public event Action LoadTop10Score;
    public event Action LoadPlayerBestScore;

    [field: SerializeField] public List<LootLockerScoreData> Top10Scores { get; private set; }
    [field: SerializeField] public LootLockerScoreData UserBestScore { get; private set; }
    
    private const int LEADERBOARD_ID = 10814;

    private void Start()
    {
        GetTop10Scores();
        GetLoggedUserBestScore();
    }

    public void SendHighScore(int score)
    {
        StartCoroutine(SubmitScoreRoutine(score));
    }
    
    public void GetTop10Scores()
    {
        StartCoroutine(FetchTop10HighScoresRoutine());
    }
    
    public void GetLoggedUserBestScore()
    {
        StartCoroutine(FetchLoggedUserHighScoreRoutine());
    }
   
    private IEnumerator SubmitScoreRoutine(int scoreToUpload)
    {
        bool done = false;
        string playerId = PlayerPrefs.GetString("PlayerId");
        LootLockerSDKManager.SubmitScore(playerId, scoreToUpload, LEADERBOARD_ID, (response) =>
        {
            if (response.success)
            {
                Debug.Log("successfully uploaded score");
                done = true;
                GetTop10Scores();
                GetLoggedUserBestScore();
            }
            else
            {
                Debug.Log("Failed" + response.Error);
                done = true;
            }
        });
        yield return new WaitWhile(() => done == false);
    }

    private IEnumerator FetchTop10HighScoresRoutine()
    {
        bool done = false;
        //TODO: Change leaderboard id to leaderboard key
        LootLockerSDKManager.GetScoreList(LEADERBOARD_ID,10,0, (response) =>
        {
            if (response.success)
            {
                List<LootLockerScoreData> tempPlayers = new();
                LootLockerLeaderboardMember[] members = response.items;

                for (int i = 0; i < members.Length; i++)
                {
                    tempPlayers.Add(new LootLockerScoreData
                    {
                        Rank = members[i].rank,
                        Score = members[i].score,
                        UserName = members[i].player.name != "" ? members[i].player.name : members[i].player.public_uid
                    });
                }

                Top10Scores = tempPlayers;
                LoadTop10Score?.Invoke();
                done = true;
            }
            else
            {
                Debug.Log("Failed" + response.Error);
                done = true;
            }
        });
        yield return new WaitWhile(() => done == false);
    }
    private IEnumerator FetchLoggedUserHighScoreRoutine()
    {
        bool done = false;
        string playerId = PlayerPrefs.GetString("PlayerId");
        LootLockerSDKManager.GetByListOfMembers(new string[]{playerId} , LEADERBOARD_ID,(response) =>
        {
            if (response.success)
            {
                LootLockerLeaderboardMember[] members = response.members;

                if (members[0] is not null)
                {
                    UserBestScore = new LootLockerScoreData
                    {
                        Rank = members[0].rank,
                        Score = members[0].score,
                        UserName = members[0].player.name != "" ? members[0].player.name : members[0].player.public_uid
                    };
                }
                else
                {
                    UserBestScore = new LootLockerScoreData
                    {
                        Rank = 0,
                        Score = 0,
                        UserName = PlayerPrefs.GetString("PlayerId")
                    };
                }

                LoadPlayerBestScore?.Invoke();
                done = true;
            }
            else
            {
                Debug.Log("Failed" + response.Error);
                done = true;
            }
        });
        yield return new WaitWhile(() => done == false);
    }
}
