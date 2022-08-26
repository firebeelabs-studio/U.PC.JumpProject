using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ReconcileDataPawn
{
    public Vector2 Position;
    public Vector2 Velocity;
    public bool Grounded;
    public bool IsJumping;
    public bool ColRight;
    public bool ColLeft;
    public float CoyoteTimeBuffer;
    public bool CanCoyotee;
}
