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
    //tutaj inputy

    public struct MoveData
    {
        public float Horizontal;
        public float Vertical;
        public bool Jump;

        public MoveData(float horizontal, float vertical, bool jump)
        {
            Horizontal = horizontal;
            Vertical = vertical;
            Jump = jump;
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

    private PlayerController _playerController;
    private Vector3 _velocity;
    private Vector3 _angularVelocity;
    //true if subscribed to events
    private bool _subscribed;
    private Rigidbody2D _rigidbody;

    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
        _rigidbody = GetComponent<Rigidbody2D>();
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
        _rigidbody.velocity = rd.Velocity;
        _rigidbody.velocity = rd.AngularVelocity;
        
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
        _playerController.JumpAndGravity();
        _playerController.CalculateHorizontal();
        _playerController.RunCollisionChecks();
        _playerController.MoveCharacter();
    }

    private void CheckInput(out MoveData md)
    {
        md = new MoveData(_playerController.Input.Y, _playerController.Input.X, _playerController.Input.JumpDown);
    }
}
