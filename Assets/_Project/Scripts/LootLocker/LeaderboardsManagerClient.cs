using System;
using System.Collections.Generic;
using FishNet.Connection;
using FishNet.Object;
using UnityEngine;

public class LeaderboardsManagerClient : NetworkBehaviour
{
    public event Action BestScoresForFewLevelsLoaded;
    public event Action BestScoresForCertainLevelLoaded;
    public event Action UserBestScoreOnCertainLevelLoaded;
    public event Action UserBestScoresForFewLevelsLoaded;
    
    [field: SerializeField] public List<LeaderboardEntry> BestScoresForCertainLevel { get; private set; }
    [field: SerializeField] public List<LeaderboardEntry> BestScoresForFewLevels { get; private set; }
    [field: SerializeField] public LeaderboardEntry UserBestScoreForCertainLevel { get; private set; }
    [field: SerializeField] public List<LeaderboardEntry> UserBestScoresForFewLevels { get; private set; }



    [ContextMenu("SendHighScore")]
    public void Test()
    {
        SendHighScore(2, "ss", "x");
    }
    
    /// <summary>
    /// Submits user score
    /// </summary>
    /// <param name="seconds"></param>
    /// /// <param name="levelName">Paste here scene name</param>
    /// <param name="skinsIds">Paste here skins ids</param>
    [ServerRpc]
    public void SendHighScore(int seconds, string levelName, string skinsIds)
    {
        SendHighScoreToServer(base.Owner, seconds, levelName, skinsIds);
    }

    [Server]
    private void SendHighScoreToServer(NetworkConnection conn, int seconds, string levelName, string skinsIds)
    {
        LeaderboardsManagerServer.Instance.SendHighScore(seconds, levelName, skinsIds);
    }
    
    /// <summary>
    /// Populate list "BestScoresForCertainLevel"
    /// </summary>
    /// <param name="count">How many users should download</param>
    /// <param name="afterPlace">After which place should download positions 0 -> starts from 1, 5 -> starts from 6</param>
    /// <param name="levelName">Paste here scene name</param>
    [ServerRpc]
    [ContextMenu("GetScoresForCertainLevel")]
    public void GetScoresForCertainLevel(int count, int afterPlace, string levelName)
    {
        GetScoresForCertainLevelFromServer(base.Owner, count, afterPlace, levelName);
    }
    
    [Server]
    private void GetScoresForCertainLevelFromServer(NetworkConnection conn, int count, int afterPlace, string levelName)
    {
        LeaderboardsManagerServer.Instance.GetScoresForCertainLevel(count, afterPlace, levelName);
    }
    
    /// <summary>
    /// Populate list "BestScoresForFewLevels"
    /// </summary>
    /// <param name="count">How many users should download</param>
    /// <param name="afterPlace">After which place should download positions 0 -> starts from 1, 5 -> starts from 6</param>
    /// <param name="levelNames">Paste here scene names</param>
    [ServerRpc]
    [ContextMenu("GetScoresForFewLevels")]
    public void GetScoresForFewLevels(int count, int afterPlace, List<string> levelNames)
    {
        GetScoresForFewLevelsFromServer(base.Owner, count, afterPlace, levelNames);
    }
    
    [Server]
    private void GetScoresForFewLevelsFromServer(NetworkConnection conn, int count, int afterPlace, List<string> levelNames)
    {
        LeaderboardsManagerServer.Instance.GetScoresForFewLevels(count, afterPlace, levelNames);
    }
    
    /// <summary>
    /// Populate UserBestScoreOnCertainLevel variable
    /// </summary>
    /// /// <param name="levelName">Paste here scene name</param>
    [ServerRpc]
    [ContextMenu("GetLoggedUserBestScoreForCertainLevel")]
    public void GetLoggedUserBestScoreForCertainLevel(string levelName)
    {
        GetLoggedUserBestScoreForCertainLevelFromServer(base.Owner, levelName);
    }
    
    [Server]
    private void GetLoggedUserBestScoreForCertainLevelFromServer(NetworkConnection conn, string levelName)
    {
        LeaderboardsManagerServer.Instance.GetLoggedUserBestScoreForCertainLevel(levelName);
    }
    
    /// <summary>
    /// Populate UserBestScoresOnFewLevels variable
    /// </summary>
    /// /// <param name="levelNames">Paste here scenes name</param>
    [ServerRpc]
    [ContextMenu("GetLoggedUserBestScoresForFewLevels")]
    public void GetLoggedUserBestScoresForFewLevels(List<string> levelNames)
    {
        GetLoggedUserBestScoresForFewLevelsFromServer(base.Owner, levelNames);
    }
    
    [Server]
    private void GetLoggedUserBestScoresForFewLevelsFromServer(NetworkConnection conn, List<string> levelNames)
    {
        LeaderboardsManagerServer.Instance.GetLoggedUserBestScoresForFewLevels(levelNames);
    }
}
