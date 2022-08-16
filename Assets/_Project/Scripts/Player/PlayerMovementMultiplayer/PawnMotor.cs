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
    private bool _subscribed;
    
    //movement
    private bool _isJumping;
    private double _timeLastGrounded;
    
    private readonly RaycastHit2D[] _groundHits = new RaycastHit2D[2];
    [SerializeField] private float _jumpCheckHeight;

    #region Initialize

    private void Awake()
    {
        _playerRb = GetComponent<Rigidbody2D>();
        _pawnStats = GetComponent<PawnStats>();
        _input = GetComponent<PawnInput>();
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
                Velocity = _playerRb.velocity
            };
            Reconcilation(rd, true);
        }
    }

    [Replicate]
    private void Move(PawnMoveData md, bool asServer, bool replaying = false)
    {
        HandleJump(md);
        HandleHorizontal(md);
    }

    private void HandleJump(PawnMoveData md)
    {
        if (!md.Jump && !_isJumping) return;

        if (_isJumping)
        {
            //if player is jumping and holds jump change nothing
            if (md.Jump) return;
            
            //If jump is not held make faster fall
            SlowJump();
            return;
        }

        //Check if we are grounded only when we are not within coyote window
        if (Time.time - _timeLastGrounded > _pawnStats.CoyoteSeconds)
        {
            Vector3 startPosition = transform.position;
            Vector2 endPosition = new Vector2(startPosition.x, startPosition.y - _jumpCheckHeight);
            Debug.DrawLine(startPosition, endPosition, Color.red, 0.1f);

            int groundHitCount = GetGroundHits();
            if (groundHitCount == 0)
            {
                //there is no coyote and is not grounded blocking jump request
                return;
            }
        }

        _isJumping = true;

        Vector2 nextVelocity = _playerRb.velocity;
        nextVelocity.y = _pawnStats.JumpPower;

        _playerRb.velocity = nextVelocity;

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
    }

    private int GetGroundHits()
    {
        Vector3 startPosition = transform.position;
        Vector2 endPosition = new Vector2(startPosition.x, startPosition.y - _jumpCheckHeight);

        return Physics2D.LinecastNonAlloc(startPosition, endPosition, _groundHits);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer != 3) return;
                
        // Debug.Log($"Is Touching Ground!");
        _isJumping = false; // Note: IF there is desync, this could be made a sync var? But ideally it should be fine.
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Note down the time at which we last touched the ground.
        var groundHits = GetGroundHits();
        if (groundHits > 0) return;
            
        Debug.Log($"Noting down timeLastGrounded.");
        _timeLastGrounded = Time.time;
    }

    #endregion
    

}
