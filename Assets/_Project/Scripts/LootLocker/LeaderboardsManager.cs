using System;
using System.Collections;
using System.Collections.Generic;
using LootLocker.Requests;
using UnityEngine;

//TODO: Move it to serverside
[RequireComponent(typeof(LoginManager))]
public class LeaderboardsManager : MonoBehaviour
{
    public event Action LoadTop10Score;
    public event Action LoadPlayerBestScore;

    private LoginManager _loginManager;
    [field: SerializeField] public List<LootLockerScoreData> Scores { get; private set; }
    [field: SerializeField] public LootLockerScoreData UserBestScore { get; private set; }

    private void Awake()
    {
        _loginManager = GetComponent<LoginManager>();
    }

    /// <summary>
    /// Submits user score
    /// </summary>
    /// <param name="seconds"></param>
    /// /// <param name="levelName">Paste here scene name</param>
    public void SendHighScore(int seconds, string levelName)
    {
        StartCoroutine(SubmitScoreRoutine(seconds, levelName));
    }
    
    /// <summary>
    /// Populate list "Scores"
    /// </summary>
    /// <param name="count">How many users should download</param>
    /// <param name="afterPlace">After which place should download positions 0 -> starts from 1, 5 -> starts from 6</param>
    /// <param name="levelName">Paste here scene name</param>
    public void GetScores(int count, int afterPlace, string levelName)
    {
        StartCoroutine(FetchScoresRoutine(count, afterPlace, levelName));
    }
    
    /// <summary>
    /// Populate UserBestScore variable
    /// </summary>
    /// /// <param name="levelName">Paste here scene name</param>
    public void GetLoggedUserBestScore(string levelName)
    {
        StartCoroutine(FetchLoggedUserHighScoreRoutine(levelName));
    }
   
    private IEnumerator SubmitScoreRoutine(int scoreToUpload, string levelName)
    {
        bool done = false;
        string playerId = _loginManager.PlayerId.ToString();
        LootLockerSDKManager.SubmitScore(playerId, scoreToUpload, levelName, (response) =>
        {
            if (response.success)
            {
                Debug.Log("successfully uploaded score");
                done = true;
                //TODO: update leaderboards
            }
            else
            {
                Debug.Log("Failed" + response.Error);
                done = true;
            }
        });
        yield return new WaitWhile(() => done == false);
    }

    private IEnumerator FetchScoresRoutine(int count, int afterPlace, string levelName)
    {
        bool done = false;
        LootLockerSDKManager.GetScoreList(levelName, count, afterPlace, (response) =>
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

                Scores = tempPlayers;
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
    
    private IEnumerator FetchLoggedUserHighScoreRoutine(string levelName)
    {
        bool done = false;
        string playerId = _loginManager.PlayerId.ToString();
        LootLockerSDKManager.GetByListOfMembers(new string[]{playerId} , levelName,(response) =>
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
                        UserName = playerId
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
