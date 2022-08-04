using UnityEngine;

public struct ReconcileData
{
    public Vector3 Position;
    public Quaternion Rotation;
    public Vector3 Velocity;
    public float AngularVelocity;
    public Vector2 Speed;
    // public bool _isGrounded;

    public ReconcileData(Vector3 position, Quaternion rotation, Vector3 velocity, float angularVelocity, Vector2 speed)
    {
        Position = position;
        Rotation = rotation;
        Velocity = velocity;
        AngularVelocity = angularVelocity;
        Speed = speed;
    }
}
