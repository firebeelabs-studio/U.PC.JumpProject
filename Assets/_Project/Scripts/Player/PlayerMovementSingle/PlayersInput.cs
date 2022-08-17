using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersInput : MonoBehaviour
{
    public FrameInput FrameInput { get; private set; }

    private void Update() {
        FrameInput = Gather();
    }
    //TODO: Change this to new input system
    private FrameInput Gather() 
    {
        return new FrameInput 
        {
            JumpDown = Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.C),
            JumpHeld = Input.GetButton("Jump") || Input.GetKey(KeyCode.C),
            DashDown = Input.GetKeyDown(KeyCode.X),
            AttackDown = Input.GetKeyDown(KeyCode.Z),
            Move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")),
        };
    }
}

public struct FrameInput 
{
    public Vector2 Move;
    public bool JumpDown;
    public bool JumpHeld;
    public bool DashDown;
    public bool AttackDown;
}


