using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using FishNet.Connection;
using LootLocker.Requests;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using UnityEditor.ShaderGraph.Serialization;
using UnityEngine;

[RequireComponent(typeof(RestClient), typeof(LLServerManager))]
public class LeaderboardsManagerServer : MonoBehaviour
{
    private ConcurrentDictionary<string, LootLockerResponseData> Scores = new();
    private RestClient _restClient;
    private LLServerManager _serverManager;
    public LeaderboardsManagerClient ManagerClient;
    
    #region Singleton

    private static LeaderboardsManagerServer _instance;
    public static LeaderboardsManagerServer Instance { get { return _instance; } }
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } 
        else 
        {
            _instance = this;
        }
        _restClient = GetComponent<RestClient>();
        _serverManager = GetComponent<LLServerManager>();
    }

    #endregion

    private IEnumerator Start()
    {
        GetLeaderboards();
        yield return new WaitForSecondsRealtime(300);
    }

    [ContextMenu("XXXXXXXXXXXXXXXXXXX")]
    public void xxx()
    {
        GetLeaderboards();
    }
    private void GetLeaderboards()
    {
        GetScoresForCertainLevel(2000,0,"Jungle5");
        GetScoresForCertainLevel(2000,0,"Jungle4");
        GetScoresForCertainLevel(2000,0,"Jungle3");
        GetScoresForCertainLevel(2000,0,"Jungle2");
        GetScoresForCertainLevel(2000,0,"Jungle1");
    }

    /// <summary>
    /// Submits user score
    /// </summary>
    /// <param name="seconds"></param>
    /// <param name="levelName">Paste here scene name</param>
    /// <param name="skinsIds">Paste here skins ids</param>
    public void SendHighScore(int seconds, string levelName, string skinsIds, int memberId)
    {
        string lastPartUrl = $"server/leaderboards/{levelName}/submit";
        byte[] s = System.Text.Encoding.UTF8.GetBytes($@"{{""member_id"":""{memberId}"",""score"":{seconds},""metadata"":""{skinsIds}""}}");
        StartCoroutine(_restClient.SendPostRequest(SendScoreResponse, lastPartUrl,s, _serverManager.Token));
        
        if (Scores[levelName].Entries.Count(p => p.Score < seconds) < 25)
        {
            GetScoresForCertainLevel(2000,0,levelName);
        }
    }

    /// <summary>
    /// Populate list "BestScoresForCertainLevel"
    /// </summary>
    /// <param name="count">How many users should download</param>
    /// <param name="afterPlace">After which place should download positions 0 -> starts from 1, 5 -> starts from 6</param>
    /// <param name="levelName">Paste here scene name</param>
    private void GetScoresForCertainLevel(int count, int afterPlace, string levelName)
    {
        string lastPartUrl = $"server/leaderboards/{levelName}/list?count={count}&after={afterPlace}";
        StartCoroutine(_restClient.SendGetRequest(GetLevelsResponse, lastPartUrl, _serverManager.Token, levelName));
    }
    
    /// <summary>
    /// Populate list "BestScoresForFewLevels"
    /// </summary>
    /// <param name="count">How many users should download</param>
    /// <param name="afterPlace">After which place should download positions 0 -> starts from 1, 5 -> starts from 6</param>
    /// <param name="levelNames">Paste here scene names</param>
    public void GetScoresForFewLevels(int count, int afterPlace, List<string> levelNames)
    {
        print("GetScoresForFewLevels");

        //StartCoroutine(FetchScoresForFewLevelsRoutine(count, afterPlace, levelNames));
    }
    
    
    private void GetLevelsResponse(string json, string levelName)
    {
        var temp = JsonConvert.DeserializeObject<LootLockerResponseData>(json);
        if (Scores.ContainsKey(levelName))
        {
            Scores[levelName] = temp;
        }
        else
        {
            Scores.TryAdd(levelName, temp);
        }
    }

    public void SendScoreToUser(NetworkConnection conn, Action<string, LootLockerResponseData> finishDelegate, string levelName)
    {
        finishDelegate.Invoke(levelName, Scores[levelName]);
    }
    
    private void SendScoreResponse(string json)
    {
        print(json);
    }
}
