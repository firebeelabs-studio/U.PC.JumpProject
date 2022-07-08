using System;
using System.Collections;
using System.Collections.Generic;
using FishNet.Example.Prediction.Transforms;
using FishNet.Object;
using FishNet.Object.Prediction;
using TarodevController;
using UnityEngine;

public class PlayerMotor : NetworkBehaviour
{
    public FrameInput Input { get; private set; }
    private PlayerInput _input;

    public struct MoveData
    {
        public float Horizontal;
        public float Vertical;
        public bool JumpHeld;

        public MoveData(float horizontal, float vertical, bool jumpHeld)
        {
            Horizontal = horizontal;
            Vertical = vertical;
            JumpHeld = jumpHeld;
        }
    }
    public struct ReconcileData
    {
        public Vector3 Position;
        public Quaternion Rotation;
        public Vector3 Velocity;
        public Vector3 AngularVelocity;

        public ReconcileData(Vector3 position, Quaternion rotation, Vector3 velocity, Vector3 angularVelocity)
        {
            Position = position;
            Rotation = rotation;
            Velocity = velocity;
            AngularVelocity = angularVelocity;
        }
    }
    
    private Vector3 _velocity;
    private Vector3 _angularVelocity;
    //true if subscribed to events
    private bool _subscribed;
    private Rigidbody2D _rigidbody;
    private PlayerMovement _playerMovement;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _input = GetComponent<PlayerInput>();
        _playerMovement = GetComponent<PlayerMovement>();
    }

    public override void OnStartNetwork()
    {
        base.OnStartNetwork();
        if (IsServer || IsOwner)
        {
            Subscribe(true);
        }
    }

    public override void OnStopNetwork()
    {
        base.OnStopNetwork();
        Subscribe(false);
    }

    private void Subscribe(bool subscribe)
    {
        if (subscribe == _subscribed) return;
        if (TimeManager is null) return;

        _subscribed = subscribe;
        if (subscribe)
        {
            TimeManager.OnTick += TimeManager_OnTick;
            TimeManager.OnPostTick += TimeManager_OnPostTick;
        }
        else
        {
            TimeManager.OnTick += TimeManager_OnTick;
            TimeManager.OnPostTick += TimeManager_OnPostTick;
        }
    }
    
    private void TimeManager_OnTick()
    {
        if (IsOwner)
        {
            Reconciliation(default, false);
            CheckInput(out MoveData md);
            Move(md, false);
        }

        if (IsServer)
        {
            Move(default, true);
        }
    }

    [Reconcile]
    private void Reconciliation(ReconcileData rd, bool asServer)
    {
        transform.position = rd.Position;
        transform.rotation = rd.Rotation;

    }

    [Server]
    private void TimeManager_OnPostTick()
    {
        if (IsServer)
        {
            ReconcileData rd = new ReconcileData(transform.position, transform.rotation, _velocity, _angularVelocity);
            Reconciliation(rd, true);
        }
    }

    [Replicate]
    private void Move(MoveData md, bool asServer, bool replaying = false)
    {
        _playerMovement.RunCollisionChecks();
        _playerMovement.CalculateJumpApex();
        _playerMovement.CalculateGravity();
        _playerMovement.CalculateJump(md.JumpHeld);
        _playerMovement.SetMoveClamp();
        _playerMovement.CalculateHorizontalMovement(md.Horizontal);
        _playerMovement.MoveCharacter();
        
        // Vector2 forces = new Vector2(md.Horizontal, md.Vertical) * 5;
        // _rigidbody.AddForce(forces);
        // _playerController.JumpAndGravity();
        // _playerController.CalculateHorizontal();
        // _playerController.RunCollisionChecks();
        // _playerController.MoveCharacter();
    }

    
    private void CheckInput(out MoveData md)
    {
        md = default;
        Input = _input.GatherInput();
        
        // if (Input.DashDown) _dashToConsume = true;
        if (Input.JumpDown)
        {
            _playerMovement.PlayerPressedJump();
        }

        float horizontal = Input.X;
        float vertical = Input.Y;
        
        //if player is not moving, and not jumping we don't wanna update anything
        if (Input.X == 0f && !Input.JumpHeld) return;
        
        md = new MoveData(horizontal, vertical, Input.JumpHeld);
    }
}
