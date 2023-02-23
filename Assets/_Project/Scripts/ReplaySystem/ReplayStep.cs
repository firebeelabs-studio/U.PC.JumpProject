using UnityEngine;

public class ReplayStep 
{
    public Vector2 Position { get; private set; }
    public Vector2 Scale { get; private set; }

    public ReplayStep(Vector2 position, Vector2 scale)
    {
        Position = position;
        Scale = scale;
    }
}
