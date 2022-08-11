using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPawnController 
{
    public FrameInput Input { get; }
    public Vector2 RawMovement { get; }
    public Vector2 Speed { get; }
    public Vector2 ForceBuildup { get; }
    public bool Grounded { get; }

    public event Action<bool> OnGroundedChanged;
    public event Action OnJumping, OnDoubleJumping;
    public event Action<bool> OnDashingChanged;
    public event Action<bool> OnCrouchingChanged;

    /// <summary>
    /// Add force to the character
    /// </summary>
    /// <param name="force">Force to be applied to the controller</param>
    /// <param name="mode">The force application mode</param>
    /// <param name="cancelMovement">Cancel the current velocity of the player to provide a reliable reaction</param>
    public void AddForce(Vector2 force, PlayerForce mode = PlayerForce.Burst, bool cancelMovement = true, bool blockMovement = false);
}

public interface IPlayerEffector 
{
    public Vector2 EvaluateEffector();
}

public enum PlayerForce {
    /// <summary>
    /// Added directly to the players movement speed, to be controlled by the standard deceleration
    /// </summary>
    Burst,

    /// <summary>
    /// An additive force handled by the decay system
    /// </summary>
    Decay
}
