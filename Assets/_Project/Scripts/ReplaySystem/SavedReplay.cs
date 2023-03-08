public class SavedReplay
{
    public string LevelName { get; private set; }
    public string BodyId { get; private set; }
    public string HatId { get; private set; }
    public string EyesId { get; private set; }
    public string MouthId { get; private set; }
    public string JacketId { get; private set; }
    public string SerializedReplay { get; private set; }

    public SavedReplay(string serializedReplay, string levelName, string bodyId, string hatId, string eyesId, string mouthId, string jacketId)
    {
        SerializedReplay = serializedReplay;
        LevelName = levelName;
        BodyId = bodyId;
        HatId = hatId;
        EyesId = eyesId;
        MouthId = mouthId;
        JacketId = jacketId;
    }
}
