using FishNet.Broadcast;

public struct ScoreBroadcast : IBroadcast
{
    public string Username;
    public float Score;
    public string LevelName;
    public string SkinsIds;
    public int MemberId;
}
