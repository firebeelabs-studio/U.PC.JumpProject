using System;
using Newtonsoft.Json;

[Serializable]
public class LootLockerPlayerData
{
    [JsonProperty("name")]
    public string Name;
    [JsonProperty("id")]
    public int Id;
    [JsonProperty("public_uid")]
    public string PublicUid;
}