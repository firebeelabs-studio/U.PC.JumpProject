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
    [SerializeField]private CapsuleCollider2D _col; // Current collider
    private PlayerInput _input;
    
    private bool _grounded;
    private Vector2 _groundNormal;
    private readonly RaycastHit2D[] _groundHits = new RaycastHit2D[2];
    private readonly RaycastHit2D[] _ceilingHits = new RaycastHit2D[1];
    private readonly Collider2D[] _crouchHits = new Collider2D[5];
    private int _groundHitCount;

    private bool _subscribed;
    public override void OnStartClient()
    {
        base.OnStartClient();
        _rb = GetComponent<Rigidbody2D>();
        _input = GetComponent<PlayerInput>();
        _col = GetComponent<CapsuleCollider2D>();
        Subscribe(true);
    }
    public override void OnStartServer()
    {
        base.OnStartServer();
        _rb = GetComponent<Rigidbody2D>();
        _input = GetComponent<PlayerInput>();
        _col = GetComponent<CapsuleCollider2D>();
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

    private decimal _i;
    [Replicate]
    private void Move(FrameInput fm, bool asServer, bool replaying = false)
    {
        _i++;
        //frame count
        //externalvelocity
        
        CheckCollisions();

        HandleHorizontal(fm);
        
        HandleJump(fm);

        HandleGravity();
        print(_speed.y + "IsServer: " +asServer + " " + _i);
        //apply velocity in better way and we are gucci
        _rb.velocity = _speed;
    }

    private void CheckCollisions()
    {
        var offset = (Vector2)transform.position + _col.offset;
        _groundHitCount = Physics2D.CapsuleCastNonAlloc(offset, _col.size, _col.direction, 0, Vector2.down, _groundHits, _stats.GrounderDistance);
        var ceilingHits = Physics2D.CapsuleCastNonAlloc(offset, _col.size, _col.direction, 0, Vector2.up, _ceilingHits, _stats.GrounderDistance);

        //if player hit roof
        if (ceilingHits > 0 && _speed.y > 0)
        {
            _speed.y = 0;
        }

        if (!_grounded && _groundHitCount > 0)
        {
            _grounded = true;
        }
        else if (_grounded && _groundHitCount == 0)
        {
            _grounded = false;
        }
    }

    private void HandleGravity()
    {
        //have to handle slopes here
        if (_grounded && _speed.y <= 0)
        {
            _speed.y = _stats.GroundingForce;
            _groundNormal = Vector2.zero;
            for (var i = 0; i < _groundHitCount; i++)
            {
                var hit = _groundHits[i];
                if (hit.collider.isTrigger) continue;
                _groundNormal = hit.normal;
            
                var slopePerp = Vector2.Perpendicular(_groundNormal).normalized;
                var slopeAngle = Vector2.Angle(_groundNormal, Vector2.up);
            
                if (slopeAngle != 0)
                {
                    if (_speed.x == 0)
                    {
                        _speed.y = 0;
                    }
                    else
                    {
                        _speed.y = _speed.x * -slopePerp.y;
                        _speed.y += _stats.GroundingForce;
                    }
            
                    break;
                }
            }
        }
        else
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
        if (fm.JumpDown && _grounded)
        {
            print("jump");
            _speed.y = _stats.JumpPower;
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
