using System;
using System.Collections;
using System.Collections.Generic;
using FishNet;
using FishNet.Object;
using FishNet.Object.Prediction;
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
        //HandleJump
        HandleHorizontal(md);
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

    #endregion
    

}
