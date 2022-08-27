using System;
using System.Collections;
using System.Collections.Generic;
using FishNet;
using FishNet.Object;
using FishNet.Object.Prediction;
using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D), typeof(PawnStats))]
public class PawnMotor : NetworkBehaviour
{
    private PawnStats _pawnStats;
    private Rigidbody2D _playerRb;
    private PawnInput _input;
    private BoxCollider2D _collider;
    private bool _subscribed;
    
    //movement
    private bool _isJumping;

    private float _coyoteTimeBuffer = 0.025f;
    
    private float _jumpApexPoint;
    
    private readonly RaycastHit2D[] _groundHits = new RaycastHit2D[2];
    [SerializeField] private float _jumpCheckHeight;

    #region Initialize

    private void Awake()
    {
        _playerRb = GetComponent<Rigidbody2D>();
        _pawnStats = GetComponent<PawnStats>();
        _input = GetComponent<PawnInput>();
        _collider = GetComponent<BoxCollider2D>();
    }

    public override void OnStartNetwork()
    {
        base.OnStartNetwork();
        SubscribeToNetworkStuff(true);
    }

    public override void OnStopNetwork()
    {
        base.OnStopNetwork();
        SubscribeToNetworkStuff(false);
    }

    private void SubscribeToNetworkStuff(bool subscribe)
    {
        if (TimeManager is null) return;

        if (_subscribed == subscribe) return;
        
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
    #endregion

    #region Prediction

    //Equivalent to FixedUpdate
    private void TimeManager_OnTick()
    {
        if (IsOwner)
        {
            Reconcilation(default, false);
            PawnMoveData moveData = _input.ConsumeData();
            Move(moveData, false);
        }

        if (IsServer)
        {
            Move(default, true);
        }
    }
    //Run after physic simulation
    private void TimeManager_OnPostTick()
    {
        if (IsServer)
        {
            ReconcileDataPawn rd = new ReconcileDataPawn
            {
                Position = transform.position,
                Velocity = _playerRb.velocity,
                Grounded = _grounded,
                CoyoteTimeBuffer = _coyoteTimeBuffer,
                CanCoyotee = _canCoyotee,
                IsJumping = _isJumping
            };
            Reconcilation(rd, true);
        }
    }

    [Replicate]
    private void Move(PawnMoveData md, bool asServer, bool replaying = false)
    {
        if (asServer || replaying)
        {
            _grounded = RunDetection(Vector2.down, out _hitsDown, _collider.bounds);
            RunCollisionChecks();
        }
        HandleJump(md, replaying);
        HandleHorizontal(md);
        ApplyGravity(md);
    }

    private void ApplyGravity(PawnMoveData md)
    {
        if (_grounded) return;
        float fallSpeed = 20f;
        Vector2 nextVelocity = _playerRb.velocity;
        nextVelocity.y -= fallSpeed * (float)TimeManager.TickDelta;

        _playerRb.velocity = nextVelocity;
    }

    private void HandleJump(PawnMoveData md, bool replaying)
    {
        if (IsServer || replaying)
        {
            if (!_grounded && _coyoteTimeBuffer > 0)
            {
                _coyoteTimeBuffer -= (float)TimeManager.TickDelta;
            }
            else if (_grounded)
            {
                _coyoteTimeBuffer = 0.025f;
                _isJumping = false;
                _canCoyotee = true;
            }
            else if (_coyoteTimeBuffer <= 0)
            {
                _canCoyotee = false;
            }
        }

        if (!md.Jump && !_isJumping) return;

        if (_isJumping)
        {
            //if player is jumping and holds jump change nothing
            if (md.Jump) return;
            
            //If jump is not held make faster fall
            //SlowJump();
            return;
        }

        //Check if we are grounded only when we are not within coyote window
        if (!_canCoyotee)
        {
            //there is no coyote and is not grounded blocking jump request
            if (!_grounded) return;
        }

        _isJumping = true;
        
        Vector2 nextVelocity = _playerRb.velocity;
        nextVelocity.y = _pawnStats.JumpPower;

        _playerRb.velocity = nextVelocity;
    }

    private void CalculateJumpApexPoint()
    {
        
    }

    private void SlowJump()
    {
        float ySpeed = _playerRb.velocity.y;
        float halfSpeed = _pawnStats.AirDecelerationPenalty * _pawnStats.JumpPower;

        if (ySpeed <= halfSpeed) return;

        //TODO: Deceleration is bad use case here have to change
        ySpeed = Mathf.MoveTowards(ySpeed, halfSpeed, _pawnStats.Deceleration * (float)TimeManager.TickDelta);

        _playerRb.velocity = new Vector2(_playerRb.velocity.x, ySpeed);
    }

    private void HandleHorizontal(PawnMoveData md)
    {
        float xSpeed = _playerRb.velocity.x;
        float moveValue = md.Horizontal;

        if (moveValue != 0)
        {
            xSpeed += moveValue * _pawnStats.Deceleration * (float)TimeManager.TickDelta;
        }
        else
        {
            //if player isn't pressing buttons stop him with deceleration
            xSpeed = Mathf.MoveTowards(xSpeed, 0, _pawnStats.Deceleration * (float)TimeManager.TickDelta);
        }

        //Clamp move speed
        xSpeed = Mathf.Clamp(xSpeed, -_pawnStats.MaxSpeed, _pawnStats.MaxSpeed);

        //Apply
        _playerRb.velocity = new Vector2(xSpeed, _playerRb.velocity.y);
    }

    [Reconcile]
    private void Reconcilation(ReconcileDataPawn rd, bool asServer)
    {
        transform.position = rd.Position;
        _playerRb.velocity = rd.Velocity;
        _grounded = rd.Grounded;
        _coyoteTimeBuffer = rd.CoyoteTimeBuffer;
        _canCoyotee = rd.CanCoyotee;
        _isJumping = rd.IsJumping;
    }

    [Header("COLLISION")] [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _detectionRayLength = 0.6f;
    private RaycastHit2D[] _hitsDown = new RaycastHit2D[3];
    private RaycastHit2D[] _hitsUp = new RaycastHit2D[1];
    private RaycastHit2D[] _hitsLeft = new RaycastHit2D[1];
    private RaycastHit2D[] _hitsRight = new RaycastHit2D[1];
    private bool _hittingCeiling, _grounded, _colRight, _colLeft;
    private bool _canCoyotee;

    private void RunCollisionChecks()
    {
        Bounds bounds = _collider.bounds;

        bool groundedCheck = RunDetection(Vector2.down, out _hitsDown, bounds);
        _colLeft = RunDetection(Vector2.left, out _hitsLeft, bounds);
        _colRight = RunDetection(Vector2.right, out _hitsRight, bounds);
        _hittingCeiling = RunDetection(Vector2.up, out _hitsUp, bounds);

        if (groundedCheck)
        {
            _isJumping = false;
        }
        if (_grounded && !groundedCheck)
        {
            //if jumped
        }
        else if (!_grounded && groundedCheck)
        {
            //if landed
            _isJumping = false;

        }
    }
    private void OnDrawGizmos()
    {
        if (!_collider) _collider = GetComponent<BoxCollider2D>();

        Gizmos.color = Color.blue;
        var b = _collider.bounds;
        b.Expand(_detectionRayLength);

        Gizmos.DrawWireCube(b.center, b.size);
    }

    private bool RunDetection(Vector2 direction, out RaycastHit2D[] hits, Bounds bounds)
    {
        hits = Physics2D.BoxCastAll(bounds.center, bounds.size, 0, direction, _detectionRayLength, _groundLayer);
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider && ! hit.collider.isTrigger)
            {
                return true;
            }
        }

        return false;
    }

    #endregion
    

}
