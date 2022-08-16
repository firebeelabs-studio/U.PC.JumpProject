using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnStats : MonoBehaviour
{
    [Header("MOVEMENT")] [Tooltip("The players capacity to gain speed")]
    public float Acceleration = 120;
    
    [Tooltip("The top horizontal movement speed")]
    public float MaxSpeed = 6;

    [Tooltip("The pace at which the player comes to a stop")]
    public float Deceleration = 60;
    
    [Tooltip("The fixed frames before coyote jump becomes unusable. Coyote jump allows jump to execute even after leaving a ledge")]
    public float CoyoteSeconds = 0.1f;
    
    [Header("JUMP")] [Tooltip("The immediate velocity applied when jumping")]
    public float JumpPower = 8;
    
    [Tooltip("Movement loss after stopping input mid-air")]
    public float AirDecelerationPenalty = 0.5f;
}
