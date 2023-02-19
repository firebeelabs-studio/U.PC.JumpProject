using System;
using System.Collections.Generic;
using FishNet.Object;
using UnityEngine;

public class LeaderBoardsManagerClient : NetworkBehaviour
{
    public event Action BestScoresForFewLevelsLoaded;
    public event Action BestScoresForCertainLevelLoaded;
    public event Action UserBestScoreOnCertainLevelLoaded;
    public event Action UserBestScoresForFewLevelsLoaded;
    
    [field: SerializeField] public List<LeaderboardEntry> BestScoresForCertainLevel { get; private set; }
    [field: SerializeField] public List<LeaderboardEntry> BestScoresForFewLevels { get; private set; }
    [field: SerializeField] public LeaderboardEntry UserBestScoreForCertainLevel { get; private set; }
    [field: SerializeField] public List<LeaderboardEntry> UserBestScoresForFewLevels { get; private set; }
    
    private LoginManager _loginManager;
    private void Awake()
    {
        _loginManager = GetComponent<LoginManager>();
    }

    /// <summary>
    /// Submits user score
    /// </summary>
    /// <param name="seconds"></param>
    /// /// <param name="levelName">Paste here scene name</param>
    /// <param name="skinsIds">Paste here skins ids</param>
    public void SendHighScore(int seconds, string levelName, string skinsIds)
    {
        //StartCoroutine(SubmitScoreRoutine(seconds, levelName, skinsIds));
    }
    
    /// <summary>
    /// Populate list "BestScoresForCertainLevel"
    /// </summary>
    /// <param name="count">How many users should download</param>
    /// <param name="afterPlace">After which place should download positions 0 -> starts from 1, 5 -> starts from 6</param>
    /// <param name="levelName">Paste here scene name</param>
    public void GetScoresForCertainLevel(int count, int afterPlace, string levelName)
    {
        //StartCoroutine(FetchScoresRoutine(count, afterPlace, levelName));
    }
    
    /// <summary>
    /// Populate list "BestScoresForFewLevels"
    /// </summary>
    /// <param name="count">How many users should download</param>
    /// <param name="afterPlace">After which place should download positions 0 -> starts from 1, 5 -> starts from 6</param>
    /// <param name="levelNames">Paste here scene names</param>
    public void GetScoresForFewLevels(int count, int afterPlace, List<string> levelNames)
    {
        //StartCoroutine(FetchScoresForFewLevelsRoutine(count, afterPlace, levelNames));
    }
    
    /// <summary>
    /// Populate UserBestScoreOnCertainLevel variable
    /// </summary>
    /// /// <param name="levelName">Paste here scene name</param>
    public void GetLoggedUserBestScoreForCertainLevel(string levelName)
    {
        //StartCoroutine(FetchLoggedUserHighScoreRoutine(levelName));
    }
    
    /// <summary>
    /// Populate UserBestScoresOnFewLevels variable
    /// </summary>
    /// /// <param name="levelNames">Paste here scenes name</param>
    public void GetLoggedUserBestScoresForFewLevels(List<string> levelNames)
    {
        //StartCoroutine(FetchLoggedUserHighScoreForFewLevelsRoutine(levelNames));
    }
}
