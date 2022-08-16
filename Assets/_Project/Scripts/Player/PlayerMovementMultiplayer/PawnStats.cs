using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnStats : MonoBehaviour
{
    [Header("MOVEMENT")] [Tooltip("The players capacity to gain speed")]
    public float Acceleration = 120;
    
    [Tooltip("The top horizontal movement speed")]
    public float MaxSpeed = 14;

    [Tooltip("The pace at which the player comes to a stop")]
    public float Deceleration = 60;
}
