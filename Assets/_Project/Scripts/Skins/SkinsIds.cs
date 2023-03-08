public class SkinsIds 
{
    public string BodyId { get; set; }
    public string HatId { get; set; }
    public string EyesId { get; set; }
    public string MouthId { get; set; }
    public string JacketId { get; set; }

    public SkinsIds(string bodyId, string hatId, string eyesId, string mouthId, string jacketId)
    {
        BodyId = bodyId;
        HatId = hatId;
        EyesId = eyesId;
        MouthId = mouthId;
        JacketId = jacketId;
    }
}
