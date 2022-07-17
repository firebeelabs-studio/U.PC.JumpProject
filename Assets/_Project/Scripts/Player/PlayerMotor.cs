using System;
using System.Collections;
using System.Collections.Generic;
using FishNet.Example.Prediction.Transforms;
using FishNet.Object;
using FishNet.Object.Prediction;
using TarodevController;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

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
        public int FixedFrame;

        public ReconcileData(Vector3 position, Quaternion rotation, Vector3 velocity, float angularVelocity, Vector2 speed, int fixedFrame)
        {
            Position = position;
            Rotation = rotation;
            Velocity = velocity;
            AngularVelocity = angularVelocity;
            Speed = speed;
            FixedFrame = fixedFrame;
        }
    }

    private MoveData _clientMoveData;
    private Vector3 _velocity;

    private Vector2 _speed;
    private Vector2 _lastPosition;
    //true if subscribed to events
    private bool _subscribed;
    private Rigidbody2D _rigidbody;
    private PawnMovement _pawn;
    private BoxCollider2D _coll;
    private int _ticks;
    private int _fixedFrame;
    
    [Header("WALKING")]

    [SerializeField] private float _acceleration = 120;
    [SerializeField] private float _moveClamp = 13;
    [SerializeField] private float _deceleration = 60f;
    [SerializeField] private float _apexBonus = 100;
    //this move clamp is used to slow player in crawling etc. (based on default move clamp)
    private float _moveClampUpdatedEveryFrame = 13;
    
    [Header("COLLISION")] [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _detectionRayLength = 0.1f;
    private RaycastHit2D[] _hitsDown = new RaycastHit2D[3];
    private RaycastHit2D[] _hitsUp = new RaycastHit2D[1];
    private RaycastHit2D[] _hitsLeft = new RaycastHit2D[1];
    private RaycastHit2D[] _hitsRight = new RaycastHit2D[1];
    
    private bool _hittingCeiling, _grounded, _colRight, _colLeft;
    
    //used to calculate coyote time - last frame when we were touching ground / first when we left ground
    private float _frameOnWhichPlayerLeftGround;
    
    [Header("JUMPING")]

    [SerializeField] private float _jumpHeight = 35;
    [SerializeField] private float _jumpApexThreshold = 40f;
    [SerializeField] private int _coyoteTimeThreshold = 7;
    [SerializeField] private int _jumpBuffer = 7;
    [SerializeField] private float _jumpEndEarlyGravityModifier = 3;
    
    private bool _jumpToConsume;
    private bool _canUseCoyote;
    //flag preventing using buffered jumps few times
    private bool _executedBufferedJump;
    //if true player is falling faster
    private bool _useShortJumpFallMultiplier = true;
    //number from 0-1 showing where player lies between highest jump point and jump position 
    private float _apexPoint;
    //
    private float _lastFrameJumpPressed = float.MinValue;
    private bool _doubleJumpUsable;
    //if true player has double jump ability 
    public bool AllowDoubleJump;
    
    [Header("GRAVITY")] [SerializeField] private float _fallClamp = -60f;
    [SerializeField] private float _minFallSpeed = 80f;
    [SerializeField] private float _maxFallSpeed = 160f;
    [SerializeField, Range(0, -10)] private float _groundingForce = -1.5f;
    private float _fallSpeed;

    
    #region Initalization

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _input = GetComponent<PlayerInput>();
        _pawn = GetComponent<PawnMovement>();
        _coll = GetComponent<BoxCollider2D>();
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
            _ticks++;
            // if (_ticks % 10 == 0)
            // {
                _fixedFrame++;
            // }
        }

        if (IsServer)
        {
            Move(default, true);
            _ticks++;
            // if (_ticks % 60 == 0)
            // {
                _fixedFrame++;
            // }
        }
    }

    [Server]
    private void TimeManager_OnPostTick()
    {
        if (IsServer)
        {
            ReconcileData rd = new ReconcileData(transform.position, transform.rotation, _velocity, _rigidbody.angularVelocity, _speed, _fixedFrame);
            Reconciliation(rd, true);
        }
    }

    [Replicate]
    private void Move(MoveData md, bool asServer, bool replaying = false)
    {
        if (md.Jump)
        {
            _lastFrameJumpPressed = _fixedFrame;
            _jumpToConsume = true;
        }
        _velocity = (_rigidbody.position - _lastPosition) / (float)TimeManager.TickDelta;
        _lastPosition = _velocity;

        RunCollisionChecks();
        // print($"Is grounded: {_grounded} IsExecutedBuffer {_executedBufferedJump} Last frame jump pressed: {_lastFrameJumpPressed} Fixed frame: {_fixedFrame} Threshold: {_jumpBuffer}");
        // print($"has buffer  " +HasBufferedJump);
        CalculateJumpApex();
        _speed.y = CalculateJump(_jumpToConsume, _speed);
        _speed.y = CalculateGravity();
        _speed = _pawn.CalculateHorizontalMovement(_speed, _acceleration, _deceleration, _moveClampUpdatedEveryFrame, md.Horizontal, (float)TimeManager.TickDelta, _grounded, _colRight, _colLeft);
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
        _fixedFrame = rd.FixedFrame;
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
        // if (horizontal == 0f && !Input.JumpHeld && !pressedJump) return;
        md = new MoveData(horizontal, vertical, jumpHeld, pressedJump);
    }
    
    public void RunCollisionChecks()
    {
        //generate ranges
        Bounds bounds = _coll.bounds;
        
        bool groundCheck = RunDetection(Vector2.down, out _hitsDown, bounds);
        _colLeft = RunDetection(Vector2.left, out _hitsLeft, bounds);
        _colRight = RunDetection(Vector2.right, out _hitsRight, bounds);
        _hittingCeiling = RunDetection(Vector2.up, out _hitsUp, bounds);

        //triggered on first leaving ground
        if (_grounded && !groundCheck)
        {
            //_frameOnWhichPlayerLeftGround = _fixedFrame;
        }
        //triggered on first ground touch
        else if (!_grounded && groundCheck)
        {
            print("landed");
            //let player use coyote
            _canUseCoyote = true;
            //if we are on the ground we are able to buffer jump
            _executedBufferedJump = false;
            //we are on ground, make double jump usable again
            _doubleJumpUsable = true;
            //player don't have to fall anymore if he is already on the ground
            _speed.y = 0;
        }
        
        _grounded = groundCheck;
    }

    private bool RunDetection(Vector2 rayDirection, out RaycastHit2D[] hits, Bounds bounds)
    {
        hits = Physics2D.BoxCastAll(bounds.center, bounds.size, 0, rayDirection, _detectionRayLength, _groundLayer);
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider && !hit.collider.isTrigger)
            {
                return true;
            }
        }
                    
        return false;
    }
    
    private void OnDrawGizmos()
    {
        if (!_coll) _coll = GetComponent<BoxCollider2D>();

        Gizmos.color = Color.blue;
        var b = _coll.bounds;
        b.Expand(_detectionRayLength);

        Gizmos.DrawWireCube(b.center, b.size);
    }
    
    //if can use coyote, is in the air, and time elapsed since last ground touch is in coyoteThreshold range
    private bool CanUseCoyote => _canUseCoyote && !_grounded && _frameOnWhichPlayerLeftGround >= _fixedFrame -20;
    
    //if is grounded and didn't already buffer jumped or isn't stacked AND player last time pressed space in jump buffer threshold time
    private bool HasBufferedJump => ((_grounded && !_executedBufferedJump)) && _lastFrameJumpPressed >= _fixedFrame -20                          ;
    
    //if double jump ability is unlocked and player is on the ground and isn't in coyote time threshold (didn't left ground before jump)
    private bool CanDoubleJump => AllowDoubleJump && _doubleJumpUsable && !_canUseCoyote;
    
    public float CalculateJump(bool jumpHeld, Vector2 speed)
    {
        //if player pressed space, is not grounded, and is allowed to use double jump
        // if (_jumpToConsume && CanDoubleJump)
        // {
        //     _speed.y = _jumpHeight;
        //     _doubleJumpUsable = false;
        //     _useShortJumpFallMultiplier = false;
        //     _jumpToConsume = false;
        //     AllowDoubleJump = false;
        // }
        
        //jump if grounded or within coyote threshold or player did buffered jump
        //print($"Jump: {jumpHeld} Coyote: {CanUseCoyote} HasBufferedJump: {HasBufferedJump}");
        if (jumpHeld && CanUseCoyote || HasBufferedJump)
        {
            speed.y = _jumpHeight;
            print("jump" + " " + speed.y);
            _useShortJumpFallMultiplier = false;
            _canUseCoyote = false;
            _jumpToConsume = false;
            //_frameOnWhichPlayerLeftGround = _fixedFrame;
            _executedBufferedJump = true;
        }

        //if player is in the air, player isn't holding space, jump didn't already ended and player isn't already falling
        if (!_grounded && !jumpHeld && !_useShortJumpFallMultiplier && speed.y > 0)
        {
            _useShortJumpFallMultiplier = true;
        }

        if (_hittingCeiling && speed.y > 0 && !_executedBufferedJump)
        {
            speed.y = 0;
        }

        return speed.y;
    }

    public float CalculateGravity()
    {
        {
            if (_grounded)
            {
            
            }
            else
            {
                //if player stopped jump faster multiply forces
                float fallSpeedCalculated = _useShortJumpFallMultiplier && _speed.y > 0 ? _fallSpeed * _jumpEndEarlyGravityModifier : _fallSpeed;
                _speed.y -= fallSpeedCalculated * (float)TimeManager.TickDelta;
                print(_speed.y);
                //hola hola amigo, don't fall too fast
                if (_speed.y < _fallClamp)
                {
                    _speed.y = _fallClamp;
                }
            }

            return _speed.y;
        }
    }
    private void CalculateJumpApex()
    {
        if (!_grounded)
        {
            // Gets stronger the closer to the top of the jump
            _apexPoint = Mathf.InverseLerp(_jumpApexThreshold, 0, Mathf.Abs(_velocity.y));
            _fallSpeed = Mathf.Lerp(_minFallSpeed, _maxFallSpeed, _apexPoint);
        }
        else
        {
            _apexPoint = 0;
        }
    }
}
