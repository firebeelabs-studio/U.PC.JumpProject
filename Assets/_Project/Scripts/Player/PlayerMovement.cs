using System;
using System.Collections;
using System.Collections.Generic;
using FishNet;
using FishNet.Managing.Timing;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //if true player has double jump ability 
    public bool AllowDoubleJump;
    private Rigidbody2D _rb;
    //used for drawing raycasts for collision checks
    private BoxCollider2D _collider;
    private Vector2 _speed;
    private TimeManager _timeManager;
    private int _fixedFrame;
    private Vector2 _velocity;
    private Vector2 _lastPosition;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<BoxCollider2D>();
        _timeManager = InstanceFinder.TimeManager;
    }

    //TODO: move this to playermotor OnUpdate
    private void FixedUpdate()
    {
        _fixedFrame++;
        _velocity = (_rb.position - _lastPosition) / Time.fixedDeltaTime;
        _lastPosition = _rb.position;
    }

    //we can't do this inside update prob.
    public void SetMoveClamp()
    {
        _moveClampUpdatedEveryFrame = _moveClamp;
    }

    public void PlayerPressedJump(bool jumpPressed)
    {
        if (!jumpPressed) return;
        _lastFrameJumpPressed = _fixedFrame;
        _jumpToConsume = true;
    }
    public void MoveCharacter(float delta)
    {
        var move = _speed * delta;
        _rb.MovePosition(_rb.position + move);
    }

    #region Moving

    [Header("WALKING")]

    [SerializeField] private float _acceleration = 120;
    [SerializeField] private float _moveClamp = 13;
    [SerializeField] private float _deceleration = 60f;
    [SerializeField] private float _apexBonus = 100;
    
    //this move clamp is used to slow player in crawling etc. (based on default move clamp)
    private float _moveClampUpdatedEveryFrame;

    public void CalculateHorizontalMovement(float inputX, float delta)
    {
        if (inputX != 0)
        {
            //set horizontal move speed
            _speed.x += inputX * _acceleration * delta;
             
            //this clamp prevents infinitely stacking speed, it's based on frameClamp so crawling etc. won't break anything 
            _speed.x = Mathf.Clamp(_speed.x, -_moveClampUpdatedEveryFrame, _moveClampUpdatedEveryFrame);
             
            //apply bonus at the apex of a jump
            var apexBonus = Mathf.Sign(inputX) * _apexBonus * _apexPoint;
            _speed.x += apexBonus * delta;
        }
        else
        {
            //no input slow down player using deceleration (mario like stop)
            _speed.x = Mathf.MoveTowards(_speed.x, 0, _deceleration * delta);
        }
        
        if (!_grounded && (_speed.x > 0 && _colRight || _speed.x < 0 && _colLeft))
        {
            // Don't pile up useless horizontal (prevents sticking to walls mid-air)
            _speed.x = 0;
        }
    }

    #endregion

    #region Jump

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

    public void CalculateJumpApex()
    {
        if (!_grounded)
        {
            //check how close is player to apex of the jump
            _apexPoint = Mathf.InverseLerp(_jumpApexThreshold, 0, Mathf.Abs(_velocity.y));
            //Gets stronger the closer to the top of the jump
            _fallSpeed = Mathf.Lerp(_minFallSpeed, _maxFallSpeed, _apexPoint);
        }
        else
        {
            _apexPoint = 0;
        }
    }

    public void CalculateJump(bool jumpHeld)
    {
        //if player pressed space, is not grounded, and is allowed to use double jump
        if (_jumpToConsume && CanDoubleJump)
        {
            _speed.y = _jumpHeight;
            _doubleJumpUsable = false;
            _useShortJumpFallMultiplier = false;
            _jumpToConsume = false;
            AllowDoubleJump = false;
        }
        
        //jump if grounded or within coyote threshold or player did buffered jump
        if (_jumpToConsume && CanUseCoyote || HasBufferedJump)
        {
            _speed.y = _jumpHeight;
            _useShortJumpFallMultiplier = false;
            _canUseCoyote = false;
            _jumpToConsume = false;
            _frameOnWhichPlayerLeftGround = _fixedFrame;
            _executedBufferedJump = true;
        }

        //if player is in the air, player isn't holding space, jump didn't already ended and player isn't already falling
        if (!_grounded && !jumpHeld && !_useShortJumpFallMultiplier && _velocity.y > 0)
        {
            _useShortJumpFallMultiplier = true;
        }

        if (_hittingCeiling && _speed.y > 0)
        {
            _speed.y = 0;
        }
    }

    //if can use coyote, is in the air, and time elapsed since last ground touch is in coyoteThreshold range
    private bool CanUseCoyote => _canUseCoyote && !_grounded && _frameOnWhichPlayerLeftGround + _coyoteTimeThreshold > _fixedFrame;
    
    //if is grounded and didn't already buffer jumped or isn't stacked AND player last time pressed space in jump buffer threshold time
    private bool HasBufferedJump => ((_grounded && !_executedBufferedJump) || _cornerStuck) && _lastFrameJumpPressed + _jumpBuffer > _fixedFrame;
    
    //if double jump ability is unlocked and player is on the ground and isn't in coyote time threshold (didn't left ground before jump)
    private bool CanDoubleJump => AllowDoubleJump && _doubleJumpUsable && !_canUseCoyote;
    
    #endregion

    #region Corner Stuck Prevention

    private Vector2 _lastPos;
    private bool _cornerStuck;

    #endregion
    
    #region Gravity

    [Header("GRAVITY")] [SerializeField] private float _fallClamp = -60f;
    [SerializeField] private float _minFallSpeed = 80f;
    [SerializeField] private float _maxFallSpeed = 160f;
    [SerializeField, Range(0, -10)] private float _groundingForce = -1.5f;
    private float _fallSpeed;

    public void CalculateGravity()
    {
        if (_grounded)
        {
            
        }
        else
        {
            //if player stopped jump faster multiply forces
            float fallSpeed = _useShortJumpFallMultiplier && _speed.y > 0 ? _fallSpeed * _jumpEndEarlyGravityModifier : _fallSpeed;
            _speed.y -= fallSpeed * Time.fixedDeltaTime;
            //hola hola amigo, don't fall too fast
            if (_speed.y < _fallClamp)
            {
                _speed.y = _fallClamp;
            }
        }
    }

    #endregion

    #region Collisions

    [Header("COLLISION")] [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _detectionRayLength = 0.1f;
    private RaycastHit2D[] _hitsDown = new RaycastHit2D[3];
    private RaycastHit2D[] _hitsUp = new RaycastHit2D[1];
    private RaycastHit2D[] _hitsLeft = new RaycastHit2D[1];
    private RaycastHit2D[] _hitsRight = new RaycastHit2D[1];
    
    private bool _hittingCeiling, _grounded, _colRight, _colLeft;

    //used to calculate coyote time - last frame when we were touching ground / first when we left ground
    private float _frameOnWhichPlayerLeftGround;

    public void RunCollisionChecks()
    {
        //generate ranges
        Bounds bounds = _collider.bounds;

        bool groundCheck = RunDetection(Vector2.down, out _hitsDown, bounds);
        _colLeft = RunDetection(Vector2.left, out _hitsLeft, bounds);
        _colRight = RunDetection(Vector2.right, out _hitsRight, bounds);
        _hittingCeiling = RunDetection(Vector2.up, out _hitsUp, bounds);

        //triggered on first leaving ground
        if (_grounded && !groundCheck)
        {
            _frameOnWhichPlayerLeftGround = _fixedFrame;
        }
        //triggered on first ground touch
        else if (!_grounded && groundCheck)
        {
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

    #endregion
    

    
}
