using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FishNet.Connection;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;

public class LeaderboardsManagerClient : NetworkBehaviour
{

   public Dictionary<string, LootLockerResponseData> test = new();

   [ContextMenu("XXXXXXXXXXXXXXXXXXXXXXXXXXXXXX")]
   [ServerRpc]
   public void Test() 
   {
      StartCoroutine(testy());
   }

   private void Update()
   {
      test.Count();
   }

   private IEnumerator testy()
   {
      bool done = false;
      LeaderboardsManagerServer.Instance.SendScoreToUser(base.Owner, (s, data) =>
      {
         test.Add(s, data);
         done = true;

      }, "Jungle5");
      yield return new WaitWhile( () => done == false);
   }
}
