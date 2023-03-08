using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using FishNet;
using FishNet.Connection;
using FishNet.Managing.Server;
using FishNet.Transporting;
using LiteNetLib;
using Newtonsoft.Json;
using UnityEngine;

[RequireComponent(typeof(RestClient), typeof(LLServerManager))]
public class LeaderboardsManagerServer : MonoBehaviour
{
    private ConcurrentDictionary<string, LootLockerResponseData> Scores = new();
    private RestClient _restClient;
    private LLServerManager _serverManager;
    
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

    private void OnEnable()
    {
        InstanceFinder.ServerManager.RegisterBroadcast<ScoreBroadcast>(OnScoreBroadcast);
        InstanceFinder.ServerManager.OnRemoteConnectionState += SendScoresOnConnection;
    }
    private void OnDisable()
    {
        InstanceFinder.ServerManager.UnregisterBroadcast<ScoreBroadcast>(OnScoreBroadcast);
        InstanceFinder.ServerManager.OnRemoteConnectionState -= SendScoresOnConnection;
    }
    
    public IEnumerator DownloadLeaderboards()
    {
        GetLeaderboards();
        yield return new WaitForSecondsRealtime(10);
        SendScoresToBroadcastWatchers();
        yield return new WaitForSecondsRealtime(290);
    }

    [ContextMenu("YYYYYYYYYYYYY")]
    public void SendScoresToBroadcastWatchers()
    {
        JsonLeaderboardsBroadcast json = new JsonLeaderboardsBroadcast
        {
            Json = JsonConvert.SerializeObject(Scores)
        };
        
        InstanceFinder.ServerManager.Broadcast(json, false, Channel.Reliable);
    }

    private void SendScoresOnConnection(NetworkConnection conn, RemoteConnectionStateArgs state) 
    {
        if (state.ConnectionState == RemoteConnectionState.Started)
        {
            SendScoresToBroadcastWatchers();
        }
    }
    private void OnScoreBroadcast(NetworkConnection conn, ScoreBroadcast msg)
    {
        SendHighScore((int)msg.Score, msg.LevelName, msg.SkinsIds, msg.MemberId);
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
        
        if (Scores[levelName] is null || Scores[levelName].Entries is null || Scores[levelName].Entries.Count(p => p.Score < seconds) < 25)
        {
            StartCoroutine(GetScoresForCertainLevelAfterTime(2000,0,levelName, 3));
        }
    }

    private IEnumerator GetScoresForCertainLevelAfterTime(int count, int afterPlace, string levelName, int seconds)
    {
        yield return new WaitForSeconds(seconds);
        GetScoresForCertainLevel(count, afterPlace, levelName);
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
        //TODO: Limit this with flags
        SendScoresToBroadcastWatchers();
    }

    private void SendScoreResponse(string json)
    {
        print(json);
    }
}
