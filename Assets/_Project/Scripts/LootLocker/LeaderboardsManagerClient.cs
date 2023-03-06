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

   [ContextMenu("XXXXXXXXXXXXXXXXXXXX")]
   public void Test()
   {
      ScoreBroadcast msg = new ScoreBroadcast()
      {
         Username = "test",
         Score = 125.3f,
         SkinsIds = "testskinsids",
         LevelName = "Jungle1",
         MemberId = FindObjectOfType<LoginManager>().PlayerId
      };
        
      InstanceFinder.ClientManager.Broadcast(msg);
   }
   
   private void OnEnable()
   {
      InstanceFinder.ClientManager.RegisterBroadcast<JsonLeaderboardsBroadcast>(OnScoreBroadcast);;
   }
   
   private void OnDisable()
   {
      InstanceFinder.ClientManager.UnregisterBroadcast<JsonLeaderboardsBroadcast>(OnScoreBroadcast);;
   }

   private void OnScoreBroadcast(JsonLeaderboardsBroadcast broadcast)
   {
      Scores = JsonConvert.DeserializeObject<ConcurrentDictionary<string, LootLockerResponseData>>(broadcast.Json);
      print(Scores?.Count);
   }
}
