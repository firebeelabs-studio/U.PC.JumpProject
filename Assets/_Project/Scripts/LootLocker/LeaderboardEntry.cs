using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LeaderboardEntry
{
    public LootLockerScoreData Score;
    public string LevelName;
    public List<string> SkinIds;
    public int UserId;
}
