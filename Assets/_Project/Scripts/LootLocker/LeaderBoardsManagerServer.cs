using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LootLocker.Requests;
using UnityEngine;

[RequireComponent(typeof(RestClient), typeof(LLServerManager))]
public class LeaderboardsManagerServer : MonoBehaviour
{
    [field: SerializeField] public List<LeaderboardEntry> BestScoresForCertainLevel { get; private set; } = new();
    [field: SerializeField] public List<LeaderboardEntry> BestScoresForFewLevels { get; private set; }
    [field: SerializeField] public LeaderboardEntry UserBestScoreForCertainLevel { get; private set; }
    [field: SerializeField] public List<LeaderboardEntry> UserBestScoresForFewLevels { get; private set; }

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
        _loginManager = GetComponent<LoginManager>();
        _serverManager = GetComponent<LLServerManager>();
    }

    #endregion
    
    private LoginManager _loginManager;
    private RestClient _restClient;
    private LLServerManager _serverManager;
    

    [ContextMenu("XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX")]
    public void TESTY()
    {
        GetScoresForCertainLevel(2000,0,"Jungle5");
        //SendHighScore(1, "Jungle1", "s", 3431948);
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
        StartCoroutine(_restClient.SendPostRequest(Testaa, lastPartUrl,s, _serverManager.Token));
        
        if (BestScoresForCertainLevel.Count(p => p.Score.Score < seconds) < 25)
        {
            GetScoresForCertainLevel(2000,0,levelName);
        }
    }

    public void Test(string json)
    {
        print(json);
    }
    
    /// <summary>
    /// Populate list "BestScoresForCertainLevel"
    /// </summary>
    /// <param name="count">How many users should download</param>
    /// <param name="afterPlace">After which place should download positions 0 -> starts from 1, 5 -> starts from 6</param>
    /// <param name="levelName">Paste here scene name</param>
    public void GetScoresForCertainLevel(int count, int afterPlace, string levelName)
    {
        string lastPartUrl = $"server/leaderboards/{levelName}/list?count={count}&after={afterPlace}";
        StartCoroutine(_restClient.SendGetRequest(Testaa, lastPartUrl, _serverManager.Token));
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
    
    /// <summary>
    /// Populate UserBestScoreOnCertainLevel variable
    /// </summary>
    /// /// <param name="levelName">Paste here scene name</param>
    public void GetLoggedUserBestScoreForCertainLevel(string levelName)
    {
        print("GetLoggedUserBestScoreForCertainLevel");
        //StartCoroutine(FetchLoggedUserHighScoreRoutine(levelName));
    }
    
    /// <summary>
    /// Populate UserBestScoresOnFewLevels variable
    /// </summary>
    /// /// <param name="levelNames">Paste here scenes name</param>
    public void GetLoggedUserBestScoresForFewLevels(List<string> levelNames)
    {
        print("GetLoggedUserBestScoresForFewLevels");
        //StartCoroutine(FetchLoggedUserHighScoreForFewLevelsRoutine(levelNames));
    }

    private void Testaa(string json)
    {
        print(json);
    }
   
    // private IEnumerator SubmitScoreRoutine(int scoreToUpload, string levelName, string skinsIds)
    // {
    //     string lastPartUrl = $"/server/leaderboards/<{levelName}>/submit";
    //     byte[] s = System.Text.Encoding.UTF8.GetBytes("{\"member_id\": \"3431948\", \"score\": 12, \"metadata\": \"test\"}");
    //     StartCoroutine(_restClient.SendPostRequest(Testaa, lastPartUrl,s));
    //     // bool done = false;
    //     // string playerId = _loginManager.PlayerId.ToString();
    //     // LootLockerSDKManager.SubmitScore(playerId, scoreToUpload, levelName, skinsIds, (response) =>
    //     // {
    //     //     if (response.success)
    //     //     {
    //     //         Debug.Log("successfully uploaded score");
    //     //         done = true;
    //     //         //TODO: update leaderboards
    //     //     }
    //     //     else
    //     //     {
    //     //         Debug.Log("Failed" + response.Error);
    //     //         done = true;
    //     //     }
    //     // });
    //     // yield return new WaitWhile(() => done == false);
    // }

    private IEnumerator FetchScoresRoutine(int count, int afterPlace, string levelName)
    {
        bool done = false;
        LootLockerSDKManager.GetScoreList(levelName, count, afterPlace, (response) =>
        {
            if (response.success)
            {
                List<LeaderboardEntry> leaderboardEntries = new();
                LootLockerLeaderboardMember[] members = response.items;

                for (int i = 0; i < members.Length; i++)
                {
                    leaderboardEntries.Add(new LeaderboardEntry
                    {
                        LevelName = levelName,
                        Score = new LootLockerScoreData
                        {
                            Rank = members[i].rank,
                            Score = members[i].score,
                            UserName = members[i].player.name != "" ? members[i].player.name : members[i].player.public_uid
                        },
                        SkinIds = members[i].metadata.Split(',').ToList()
                    });
                }

                BestScoresForCertainLevel = leaderboardEntries;
                //BestScoresForCertainLevelLoaded?.Invoke();
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
    
    private IEnumerator FetchScoresForFewLevelsRoutine(int count, int afterPlace, List<string> levelNames)
    {
        ResponseFlags responseFlags = new ResponseFlags(levelNames.Count);
        List<LeaderboardEntry> leaderboardEntries = new();
        foreach (var levelName in levelNames)
        {
            LootLockerSDKManager.GetScoreList(levelName, count, afterPlace, (response) =>
            {
                if (response.success)
                {
                    LootLockerLeaderboardMember[] members = response.items;

                    for (int i = 0; i < members.Length; i++)
                    {
                        leaderboardEntries.Add(new LeaderboardEntry
                        {
                            LevelName = levelName,
                            Score = new LootLockerScoreData
                            {
                                Rank = members[i].rank,
                                Score = members[i].score,
                                UserName = members[i].player.name != "" ? members[i].player.name : members[i].player.public_uid
                            },
                            SkinIds = members[i].metadata.Split(',').ToList()
                        });
                    }
                    responseFlags.MarkNextFlagAsReached();
                }
                else
                {
                    Debug.Log("Failed" + response.Error);
                    responseFlags.MarkNextFlagAsReached();
                }
            });
        }

        yield return new WaitWhile(() => !responseFlags.IsEverythingTrue());

        BestScoresForFewLevels = leaderboardEntries;
        //BestScoresForFewLevelsLoaded?.Invoke();
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
                    UserBestScoreForCertainLevel = new LeaderboardEntry
                    {
                        LevelName = levelName,
                        Score = new LootLockerScoreData
                        {
                            Rank = members[0].rank,
                            Score = members[0].score,
                            UserName = members[0].player.name != "" ? members[0].player.name : members[0].player.public_uid
                        },
                        SkinIds = members[0].metadata.Split(',').ToList()
                    };

                }
                else
                {
                    UserBestScoreForCertainLevel = new LeaderboardEntry
                    {
                        LevelName = levelName,
                        Score = new LootLockerScoreData
                        {
                            Rank = 0,
                            Score = 0,
                            UserName = playerId
                        }
                    };
                }

                //UserBestScoreOnCertainLevelLoaded?.Invoke();
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
    
    private IEnumerator FetchLoggedUserHighScoreForFewLevelsRoutine(List<string> levelNames)
    {
        ResponseFlags responseFlags = new ResponseFlags(levelNames.Count);
        List<LeaderboardEntry> leaderboardEntries = new();
        string playerId = _loginManager.PlayerId.ToString();
        foreach (var levelName in levelNames)
        {
            LootLockerSDKManager.GetByListOfMembers(new string[]{playerId} , levelName,(response) =>
            {
                if (response.success)
                {
                    LootLockerLeaderboardMember[] members = response.members;

                    if (members[0] is not null)
                    {
                        leaderboardEntries.Add( new LeaderboardEntry
                        {
                            LevelName = levelName,
                            Score = new LootLockerScoreData
                            {
                                Rank = members[0].rank,
                                Score = members[0].score,
                                UserName = members[0].player.name != "" ? members[0].player.name : members[0].player.public_uid
                            },
                            SkinIds = members[0].metadata.Split(',').ToList()
                        });
                    }
                    else
                    {
                        leaderboardEntries.Add( new LeaderboardEntry
                        {
                            LevelName = levelName,
                            Score = new LootLockerScoreData
                            {
                                Rank = 0,
                                Score = 0,
                                UserName = playerId
                            }
                        });
                    }
                    responseFlags.MarkNextFlagAsReached();
                }
                else
                {
                    Debug.Log("Failed" + response.Error);
                    responseFlags.MarkNextFlagAsReached();
                }
            });
        }
        
        yield return new WaitWhile(() => !responseFlags.IsEverythingTrue());
        UserBestScoresForFewLevels = leaderboardEntries;     
        //UserBestScoresForFewLevelsLoaded?.Invoke();
    }
}
