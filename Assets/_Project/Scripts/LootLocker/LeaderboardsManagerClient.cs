using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using FishNet;
using FishNet.Connection;
using FishNet.Object;
using Newtonsoft.Json;
using UnityEngine;

public class LeaderboardsManagerClient : MonoBehaviour
{
   #region Singleton

   private static LeaderboardsManagerClient _instance;
   public static LeaderboardsManagerClient Instance { get { return _instance; } }
   private void Awake()
   {
      if (_instance != null && _instance != this)
      {
         Destroy(gameObject);
      } 
      else 
      {
         _instance = this;
         DontDestroyOnLoad(gameObject);
      }
   }

   #endregion
   
   public ConcurrentDictionary<string, LootLockerResponseData> Scores = new();

   private void OnEnable()
   {
      InstanceFinder.ClientManager.RegisterBroadcast<JsonLeaderboardsBroadcast>(OnScoreBroadcast);;
   }
   
   private void OnDisable()
   {
      InstanceFinder.ClientManager.UnregisterBroadcast<JsonLeaderboardsBroadcast>(OnScoreBroadcast);;
   }

   public void SendNewScoreToServer(float score, string skinsIds, string levelName)
   {
      ScoreBroadcast msg = new ScoreBroadcast()
      {
         Score = score,
         SkinsIds = skinsIds,
         LevelName = levelName,
         MemberId = LoginManager.Instance.PlayerId
      };
        
      InstanceFinder.ClientManager.Broadcast(msg);
   }
   
   //Called everytime on server when score list is updated
   private void OnScoreBroadcast(JsonLeaderboardsBroadcast broadcast)
   {
      Scores = JsonConvert.DeserializeObject<ConcurrentDictionary<string, LootLockerResponseData>>(broadcast.Json);
   }
}
