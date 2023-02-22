using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplayData
{
    public Vector2 Position { get; private set; }
    public Vector2 Scale { get; private set; }
    public string BodyId { get; private set; }
    public string HatId { get; private set; }
    public string EyesId { get; private set; }
    public string MouthId { get; private set; }
    public string JacketId { get; private set; }

    public ReplayData(Vector2 position, Vector2 scale)
    {
        Position = position;
        Scale = scale;
    }

    public ReplayData(Vector2 position, Vector2 scale, string bodyId, string hatId, string eyesId, string mouthId, string jacketId)
    {
        Position = position;
        Scale = scale;
        BodyId = bodyId;
        HatId = hatId;
        EyesId = eyesId;
        MouthId = mouthId;
        JacketId = jacketId;
    }
}
