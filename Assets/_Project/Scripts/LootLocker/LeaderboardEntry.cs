using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

[Serializable]
public class LeaderboardEntry
{
    [JsonProperty("rank")]
    public int Rank;
    [JsonProperty("member_id")]
    public string MemberId;
    [JsonProperty("score")]
    public int Score;
    [JsonProperty("player")]
    public LootLockerPlayerData Player;
    [JsonProperty("metadata")]
    public string Metadata;
}
