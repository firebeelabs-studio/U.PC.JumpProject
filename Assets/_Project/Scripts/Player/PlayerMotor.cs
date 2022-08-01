using System;
using System.Collections;
using System.Collections.Generic;
using FishNet;
using FishNet.Example.Prediction.Transforms;
using FishNet.Object;
using FishNet.Object.Prediction;
using TarodevController;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMotor : NetworkBehaviour
{
    public struct ReconcileData
    {
        public Vector3 Position;
        public Quaternion Rotation;
        public Vector3 Velocity;
        public float AngularVelocity;

        public ReconcileData(Vector3 position, Quaternion rotation, Vector3 velocity, float angularVelocity)
        {
            Position = position;
            Rotation = rotation;
            Velocity = velocity;
            AngularVelocity = angularVelocity;
        }
    }
    public struct MoveData
    {
        public bool Jump;
        public float Horizontal;
        public float Vertical;

        public MoveData(bool jump, float horizontal, float vertical)
        {
            Jump = jump;
            Horizontal = horizontal;
            Vertical = vertical;
        }
    }
    
    Vector2 _speed;
    
    [SerializeField] private ScriptableStats _stats;
    private FrameInput _frameInput;
    private Rigidbody2D _rb;
    private CapsuleCollider2D[] _cols; // Standing and crouching colliders
    private CapsuleCollider2D _col; // Current collider
    private PlayerInput _input;

    private bool _subscribed;
    public override void OnStartClient()
    {
        base.OnStartClient();
        _rb = GetComponent<Rigidbody2D>();
        _input = GetComponent<PlayerInput>();
        _rb = GetComponent<Rigidbody2D>();
        Subscribe(true);
    }
    public override void OnStartServer()
    {
        base.OnStartServer();
        _rb = GetComponent<Rigidbody2D>();
        _input = GetComponent<PlayerInput>();
        _rb = GetComponent<Rigidbody2D>();
        Subscribe(true);
    }

    private void OnDestroy()
    {
        if (InstanceFinder.TimeManager is not null)
        {
            Subscribe(false);
        }
    }

    private void Subscribe(bool subscribe)
    {
        if (TimeManager is null) return;

        if (subscribe == _subscribed) return;

        _subscribed = subscribe;

        if (subscribe)
        {
            InstanceFinder.TimeManager.OnTick += TimeManager_OnTick;
            InstanceFinder.TimeManager.OnPostTick += TimeManager_OnPostTick;
        }
        else
        {
            InstanceFinder.TimeManager.OnTick -= TimeManager_OnTick;
            InstanceFinder.TimeManager.OnPostTick -= TimeManager_OnPostTick;
        }
    }

    //OnTick is equivalent to FixedUpdate
    private void TimeManager_OnTick()
    {
        if (IsOwner)
        {
            Reconcilation(default, false);
            Move(_frameInput, false);
        }

        if (IsServer)
        {
            Move(default, true);
        }
    }

    private void TimeManager_OnPostTick()
    {
        if (IsServer)
        {
            ReconcileData rd = new ReconcileData(transform.position, transform.rotation, _rb.velocity, _rb.angularVelocity);
            Reconcilation(rd,true);
        }
    }

    private void Update()
    {
        if (IsOwner)
        {
            //OnTick is working as FixedUpdate so checking inputs OnTick sucks
            GatherInput(out _frameInput);
        }
    }

    private bool _jumped = false;
    [Replicate]
    private void Move(FrameInput fm, bool asServer, bool replaying = false)
    {
        //frame count
        //externalvelocity
        
        //collisions checks here
        
        HandleHorizontal(fm);
        
        HandleJump(fm);

        HandleGravity();
        
        //apply velocity in better way and we are gucci
        _rb.AddForce(_speed);
    }

    private void HandleGravity()
    {
        //have to handle slopes here
        if (_speed.y > 0)
        {
            var fallSpeed = -_stats.FallSpeed;
            _speed.y += fallSpeed * (float)TimeManager.TickDelta;

            //clamp fall speed
            if (_speed.y < -_stats.MaxFallSpeed)
            {
                _speed.y = -_stats.MaxFallSpeed;
            }
        }
    }

    private void HandleJump(FrameInput fm)
    {
        //TODO: add here Coyote and buffer
        if (fm.JumpDown && !_jumped)
        {
            _jumped = true;
            _speed.y = _stats.JumpPower * 3;
        }
    }

    [Reconcile]
    private void Reconcilation(ReconcileData rd, bool asServer)
    {
        transform.position = rd.Position;
        transform.rotation = rd.Rotation;
        _rb.velocity = rd.Velocity;
        _rb.angularVelocity = rd.AngularVelocity;
    }
    private void HandleHorizontal(FrameInput fm)
    {
        //i'm ignoring crouching and creeping atm
        if (fm.Move.x != 0)
        {
            _speed.x += fm.Move.x * _stats.Acceleration * (float)TimeManager.TickDelta;
        }
        else
        {
            //if player is not pressing buttons just stop him with deceleration
            _speed.x = Mathf.MoveTowards(_speed.x, 0, _stats.Deceleration * (float)TimeManager.TickDelta);
        }
        //limit speed
        _speed.x = Mathf.Clamp(_speed.x, -_stats.MaxSpeed, _stats.MaxSpeed);
    }
    
    protected virtual void GatherInput(out FrameInput fm)
    {
        fm = default;
        fm = _input.FrameInput;
        if (_frameInput.JumpDown)
        {
            //jump   
        }
    }
}
