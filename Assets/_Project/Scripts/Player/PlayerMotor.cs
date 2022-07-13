using System;
using System.Collections;
using System.Collections.Generic;
using FishNet.Example.Prediction.Transforms;
using FishNet.Object;
using FishNet.Object.Prediction;
using TarodevController;
using TMPro;
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
        public bool Jump;

        public MoveData(float horizontal, float vertical, bool jumpHeld, bool jump)
        {
            Horizontal = horizontal;
            Vertical = vertical;
            JumpHeld = jumpHeld;
            Jump = jump;
        }
    }
    public struct ReconcileData
    {
        public Vector3 Position;
        public Quaternion Rotation;
        public Vector3 Velocity;
        public Vector2 Speed;
        public float AngularVelocity;

        public ReconcileData(Vector3 position, Quaternion rotation, Vector3 velocity, float angularVelocity, Vector2 speed)
        {
            Position = position;
            Rotation = rotation;
            Velocity = velocity;
            AngularVelocity = angularVelocity;
            Speed = speed;
        }
    }

    private MoveData _clientMoveData;
    private Vector3 _velocity;

    private Vector2 _speed;
    //true if subscribed to events
    private bool _subscribed;
    private Rigidbody2D _rigidbody;
    private PawnMovement _pawn;
    
    [Header("WALKING")]

    [SerializeField] private float _acceleration = 120;
    [SerializeField] private float _moveClamp = 13;
    [SerializeField] private float _deceleration = 60f;
    [SerializeField] private float _apexBonus = 100;
    //this move clamp is used to slow player in crawling etc. (based on default move clamp)
    private float _moveClampUpdatedEveryFrame = 13;
    
    #region Initalization

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _input = GetComponent<PlayerInput>();
        _pawn = GetComponent<PawnMovement>();
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
            TimeManager.OnTick -= TimeManager_OnTick;
            TimeManager.OnPostTick -= TimeManager_OnPostTick;
        }
    }

    #endregion
    
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

    [Server]
    private void TimeManager_OnPostTick()
    {
        if (IsServer)
        {
            ReconcileData rd = new ReconcileData(transform.position, transform.rotation, _rigidbody.velocity, _rigidbody.angularVelocity, _speed);
            Reconciliation(rd, true);
        }
    }

    [Replicate]
    private void Move(MoveData md, bool asServer, bool replaying = false)
    {
        _speed = _pawn.CalculateHorizontalMovement(_speed, _acceleration, _deceleration, _moveClampUpdatedEveryFrame, md.Horizontal, (float)TimeManager.TickDelta);
        _pawn.MoveCharacter(_rigidbody, _speed, (float)TimeManager.TickDelta);
    }

    [Reconcile]
    private void Reconciliation(ReconcileData rd, bool asServer)
    {
        transform.position = rd.Position;
        transform.rotation = rd.Rotation;
        _rigidbody.velocity = rd.Velocity;
        _rigidbody.angularVelocity = rd.AngularVelocity;
        _speed = rd.Speed;
    }
    
    private void CheckInput(out MoveData md)
    {
        md = default;
        Input = _input.GatherInput();

        bool pressedJump = Input.JumpDown;
        bool jumpHeld = Input.JumpHeld;
        float horizontal = Input.X;
        float vertical = Input.Y;
        
        //if player is not moving, and not jumping we don't wanna update anything
        if (horizontal == 0f && !Input.JumpHeld && !pressedJump) return;
        md = new MoveData(horizontal, vertical, jumpHeld, pressedJump);
    }
}
