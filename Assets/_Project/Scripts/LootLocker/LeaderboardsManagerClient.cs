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
   public ConcurrentDictionary<string, LootLockerResponseData> Scores = new();

   private void Awake()
   {
      DontDestroyOnLoad(gameObject);
   }

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
         MemberId = FindObjectOfType<LoginManager>().PlayerId
      };
        
      InstanceFinder.ClientManager.Broadcast(msg);
   }
   
   //Called everytime on server when score list is updated
   private void OnScoreBroadcast(JsonLeaderboardsBroadcast broadcast)
   {
      Scores = JsonConvert.DeserializeObject<ConcurrentDictionary<string, LootLockerResponseData>>(broadcast.Json);
   }
}
