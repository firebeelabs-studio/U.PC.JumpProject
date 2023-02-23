using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplayData
{
    public string LevelName { get; private set; }
    public string Hash { get; private set; }
    public string BodyId { get; private set; }
    public string HatId { get; private set; }
    public string EyesId { get; private set; }
    public string MouthId { get; private set; }
    public string JacketId { get; private set; }

    public ReplayData(string levelName, string hash, string bodyId, string hatId, string eyesId, string mouthId, string jacketId)
    {
        LevelName = levelName;
        Hash = hash;
        BodyId = bodyId;
        HatId = hatId;
        EyesId = eyesId;
        MouthId = mouthId;
        JacketId = jacketId;
    }
}
