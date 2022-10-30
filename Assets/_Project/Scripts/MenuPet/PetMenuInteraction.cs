using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PetMenuInteraction : MonoBehaviour
{
    [Header("Pawn")]
    [SerializeField] private GameObject _pawn;
    [Space(10)]

    [Header("OnClickEffect")]
    [SerializeField] private float _explosionForce;
    [SerializeField] private float _fieldOfImpact;
    [SerializeField] private ParticleSystem _mouseClickRingParticle;
    [SerializeField] private AudioClip _petClickSound;
    [SerializeField] private float _pitch;
    [Space(10)]

    [Header("Holding")]
    [SerializeField] private float _maxMouseSpeed = 10;
    [SerializeField] private float _maxPullDistance;
    [Space(10)]

    [Header("IdleAnimation")]
    [SerializeField] private float _speed;
    [SerializeField] private float _idleAnimPower;
    [SerializeField] private List<Transform> _animatedBonesTransforms = new();
    private List<BonesWithOrigins> _animatedBones = new();
    [SerializeField] private Transform _eyes;
    private Vector2 _eyesStartPos;
    private bool _stopIdle = false;
    private float _idleAnimDelay;
    [Space(10)]

    [Header("NonAnimatedBones")]
    [SerializeField] private List<Transform> _nonAnimatedBonesTransforms = new();
    private List<BonesWithOrigins> _nonAnimatedBones = new();
    private bool _startTimerToResetBones;
    private float _resetBonesTimer = 2;

    private Vector3 _mousePos, _mouseForce, _lastMousePosition, _targetStartPos;
    private GameObject _selectedObj;
    private Rigidbody2D _selectedRb;
    private AudioPlayer _audioPlayer;
    private float _counter = 0;
    private float _angle;
    private struct BonesWithOrigins
    {
        public Transform Transform { get; set; }
        public Vector3 Origin { get; set; }

        public BonesWithOrigins(Transform transform, Vector3 origin)
        {
            Transform = transform;
            Origin = origin;
        }
    }

    private void Awake()
    {
        _audioPlayer = GetComponent<AudioPlayer>();
    }

    private void Start()
    {
        foreach (Transform bone in _animatedBonesTransforms)
        {
            _animatedBones.Add(new(bone, bone.position));
        }
        foreach (Transform transform in _nonAnimatedBonesTransforms)
        {
            _nonAnimatedBones.Add(new(transform, transform.position));
        }
        _eyesStartPos = _eyes.position;
    }

    void Update()
    {
        _mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _mousePos.z = 0;

        if (Input.GetMouseButton(0))
        {
            _counter += Time.deltaTime;
        }

        if (Input.GetMouseButtonUp(0))
        {
            _counter = 0;
        }
        //if player doesn't want to hold any bone and there's no currently selected bone to move
        if (_counter < 0.05f && Input.GetMouseButtonUp(0) && _selectedObj == null)
        {
            Explode(_mousePos);
            _mouseClickRingParticle.Play();
        }

        Collider2D targetObject = Physics2D.OverlapPoint(_mousePos);
        //if player wants to hold a bone and there's no bone selected
        if (_counter >= 0.05f && _selectedRb == null)
        {
            StartDragging(targetObject);
        }
        //if player realeases the button and was holding the bone
        else if (Input.GetMouseButtonUp(0) && _selectedRb)
        {
            StopDragging();
        }

        _idleAnimDelay -= Time.deltaTime;
        EnableAnimatorBack();
        IdleAnimation();
        ResetBones();
    }

    void FixedUpdate()
    {
        //moving any selected bone
        if (_selectedRb)
        {
            Vector3 direction = _mousePos - _targetStartPos;
            direction = Vector3.ClampMagnitude(direction, _maxPullDistance);
            Vector2 nextPos = Vector3.Lerp(_selectedRb.transform.position, _targetStartPos + direction, Time.fixedDeltaTime * 10f);
            _selectedRb.MovePosition(nextPos);
        }
    }

    private void Explode(Vector3 pos)
    {
        //creating new object with kinematic rigidbody
        GameObject explosion = new GameObject("explosion", typeof(Rigidbody2D));
        explosion.transform.position = pos;
        explosion.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;

        //checks if there is any collider to which we need to apply force
        Collider2D[] objects = Physics2D.OverlapCircleAll(explosion.transform.position, _fieldOfImpact);

        foreach (Collider2D obj in objects)
        {
            if (objects.Length > 0)
            {
                _audioPlayer.PlayOneShotSound(_petClickSound, 1, _pitch);
            }

            //checks if any of bones that will be moved by explosion is affected by idle animation
            foreach (var bone in _animatedBones)
            {
                if (obj.gameObject.name == bone.Transform.gameObject.name)
                {
                    //if it is stop the animation
                    _stopIdle = true;
                }
            }
            
            //calculate the explosion direction and apply it
            Vector2 direction = obj.transform.position - explosion.transform.position;
            
            if (obj.CompareTag("IgnorePetInteraction")) continue;
            obj.GetComponent<Rigidbody2D>().AddForce(direction * _explosionForce);
        }
        
        //reset the delay
        _idleAnimDelay = 2f;

        //play onclick particle
        _mouseClickRingParticle.gameObject.transform.position = pos;
        _startTimerToResetBones = true;
        _resetBonesTimer = 2f;

        Destroy(explosion);
    }

    private void StartDragging(Collider2D targetObject)
    {
        //checks if target object is not null
        if (targetObject)
        {

            //disable all target's spring joints to prevent glitching
            foreach (SpringJoint2D targetJoint in targetObject.GetComponents<SpringJoint2D>())
            {
                targetJoint.enabled = false;
            }

            //checks if target object is affected by idle animation
            foreach (var bone in _animatedBones)
            {
                if (targetObject.gameObject.name == bone.Transform.gameObject.name)
                {
                    _stopIdle = true;
                }
            }
            
            //disable target's distance joint to prevent glitching
            targetObject.GetComponent<DistanceJoint2D>().enabled = false;

            _targetStartPos = targetObject.gameObject.transform.position;
            _selectedObj = targetObject.gameObject;
            _selectedRb = targetObject.transform.gameObject.GetComponent<Rigidbody2D>();

            //change target's rigidbody to kinematic to stop it from saving applied force
            _selectedRb.isKinematic = true;
        }
        if (_selectedRb)
        {
            //calculate mouse position change
            _mouseForce = (_mousePos - _lastMousePosition) / Time.deltaTime;
            _mouseForce = Vector2.ClampMagnitude(_mouseForce, _maxMouseSpeed);
            _lastMousePosition = _mousePos;
        }
    }

    private void StopDragging()
    {
        //enable all spring joints back
        foreach (SpringJoint2D targetJoint in _selectedObj.GetComponents<SpringJoint2D>())
        {
            targetJoint.enabled = true;
        }

        //if idle animation was disabled turn it on
        if (_stopIdle)
        {
            _stopIdle = false;
        }

        //enable distance joint back
        _selectedObj.GetComponent<DistanceJoint2D>().enabled = true;

        //set back the rigidbody to dynamic
        _selectedRb.isKinematic = false;

        //reset the velocity just in case
        _selectedRb.velocity = Vector2.zero;

        //move bone back to the origin
        _selectedObj.transform.position = _targetStartPos;

        _selectedObj = null;
        _selectedRb = null;
        _startTimerToResetBones = true;
    }

    private void EnableAnimatorBack()
    {
        if (_pawn && !_pawn.activeInHierarchy) return;
        
        if (_idleAnimDelay <= 0 && !_selectedObj)
        {
            _stopIdle = false;
        }
    }

    private void IdleAnimation()
    {
        if (_stopIdle) return;
        _angle += Time.deltaTime * _speed;
        foreach (var bone in _animatedBones)
        {
            bone.Transform.position = new Vector2(bone.Origin.x, bone.Origin.y + Mathf.Sin(_angle) * _idleAnimPower);
        }
        _eyes.position = new Vector2(_eyesStartPos.x, _eyesStartPos.y + Mathf.Sin(_angle) * (_idleAnimPower/2));
    }
    protected internal void ToggleIdle(bool doDisable)
    {
        _stopIdle = doDisable;
    }
    private void ResetBones()
    {
        if (!_startTimerToResetBones) return;
        
        _resetBonesTimer -= Time.deltaTime;

        if (_resetBonesTimer <= 0)
        {
            foreach (var nonAnimatedBone in _nonAnimatedBones)
            {
                if (Vector2.Distance(nonAnimatedBone.Transform.position, nonAnimatedBone.Origin) > _maxPullDistance/2)
                {
                    nonAnimatedBone.Transform.DOMove(nonAnimatedBone.Origin, 2f).SetEase(Ease.InOutCubic);
                }
            }
            _startTimerToResetBones = false;
            _resetBonesTimer = 2f;
        }
    }
}
