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
    public struct MoveData
    {
        public FrameInput Input;
        public float TimeLastJumpPressed;
       
        public MoveData(FrameInput input, float timeLastJumpPressed)
        {
            Input = input;
            TimeLastJumpPressed = timeLastJumpPressed;
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
            GatherInput(out MoveData md);
            Move(md, false);
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
            ReconcileData rd = new ReconcileData(transform.position, transform.rotation, _rb.velocity, _rb.angularVelocity, _speed, _lastJumpPressed);
            Reconcilation(rd,true);
        }
    }

    private void Update()
    {
        if (IsOwner)
        {
            //OnTick is working as FixedUpdate so checking inputs OnTick sucks
            
        }
    }

    private decimal _i = 0;
    private bool _bufferedJumpUsable;
    private float _timeLeftGrounded;
    private float _lastJumpPressed;

    [Replicate]
    private void Move(MoveData md, bool asServer, bool replaying = false)
    {
        //_i++;
        //frame count
        //externalvelocity
        CheckCollisions();
        //TODO: cache time here

        HandleHorizontal(md.Input);
        print(_lastJumpPressed);

        HandleJump(md);

        HandleGravity(replaying);
        //print(_speed.y + "IsServer: " +asServer + " " + _i);
        //apply velocity in better way and we are gucci
        _rb.velocity = _speed;
    }

    private bool HasBufferedJump => _grounded && _bufferedJumpUsable & _lastJumpPressed + _stats.JumpBufferSeconds > Time.time;
    
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

        //if landed
        if (!_grounded && _groundHitCount > 0)
        {
            _grounded = true;
            _bufferedJumpUsable = true;
            //it's for coyotee actualy
            _timeLeftGrounded = Time.time;
        }
        else if (_grounded && _groundHitCount == 0)
        {
            _grounded = false;
        }
    }

    private void HandleGravity(bool isReplaying)
    {
        //if (isReplaying) return;
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
            //_i++;
            var fallSpeed = -_stats.FallSpeed;
            _speed.y += fallSpeed * (float)TimeManager.TickDelta;

            //clamp fall speed
            if (_speed.y < -_stats.MaxFallSpeed)
            {
                _speed.y = -_stats.MaxFallSpeed;
            }
            //print($"Falling speed: {_speed.y}, Server: {IsServer} Iteration {_i}");
        }
    }

    private void HandleJump(MoveData md)
    {
        if (md.Input.JumpDown)
        {
            _lastJumpPressed = Time.time;
        }
        //TODO: add here Coyote and buffer
        if ((md.Input.JumpDown && _grounded) || HasBufferedJump)
        {
            _bufferedJumpUsable = false;
            _speed.y = _stats.JumpPower;
        }

        if (false)
        {
           //est frame on which player left ground here 
        }
    }

    [Reconcile]
    private void Reconcilation(ReconcileData rd, bool asServer)
    {
        transform.position = rd.Position;
        transform.rotation = rd.Rotation;
        _rb.velocity = rd.Velocity;
        _rb.angularVelocity = rd.AngularVelocity;
        _speed = rd.Speed;
        //_lastJumpPressed = rd.TimeLastJumpPressed;
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
    
    protected virtual void GatherInput(out MoveData md)
    {
        md = default;
        md.Input = _input.FrameInput;
    }
}
