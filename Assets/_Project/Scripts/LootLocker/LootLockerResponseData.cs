using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

[Serializable]
public class LootLockerResponseData
{
    public string LevelName;
    [JsonProperty("pagination")]
    public Pagination Pagination;
    [JsonProperty("items")]
    public List<LeaderboardEntry> Entries;
}
