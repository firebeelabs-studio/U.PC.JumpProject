using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetMenuInteraction : MonoBehaviour
{
    [SerializeField] private float _explosionForce;
    [SerializeField] private float _fieldOfImpact;
    [SerializeField] private ParticleSystem _mouseClickRingParticle;
    [SerializeField] private float _maxMouseSpeed = 10;
    [SerializeField] private float _maxPullDistance;
    [SerializeField] private Animator _animator;

    private Vector3 _mousePos, _mouseForce, _lastMousePosition, _targetStartPos;
    private GameObject _selectedObj;
    private Rigidbody2D _selectedRb;
    private bool _canEnableAnimatorBack = true;
    private float _counter = 0;
    private float _animatorDelay;

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
        if (_counter < 0.5f && Input.GetMouseButtonUp(0) && _selectedObj == null)
        {
            Explode(_mousePos);
            _mouseClickRingParticle.Play();
        }

        Collider2D targetObject = Physics2D.OverlapPoint(_mousePos);
        //if player wants to hold a bone and there's no bone selected
        if (_counter >= 0.5f && _selectedRb == null)
        {
            StartDragging(targetObject);
        }
        //if player realeases the button and was holding the bone
        else if (Input.GetMouseButtonUp(0) && _selectedRb)
        {
            StopDragging();
        }
        if (_canEnableAnimatorBack)
        {
            _animator.enabled = true;
            _canEnableAnimatorBack = false;
        }

        _animatorDelay -= Time.deltaTime;
        EnableAnimatorBack();
    }

    void FixedUpdate()
    {
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
        GameObject explosion = new GameObject("explosion", typeof(Rigidbody2D));
        explosion.transform.position = pos;
        explosion.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        Collider2D[] objects = Physics2D.OverlapCircleAll(explosion.transform.position, _fieldOfImpact);
        foreach (Collider2D obj in objects)
        {
            //hardcoded to work only with the top one bones
            if (obj.gameObject.name == "bone_1" || obj.gameObject.name == "bone_2" || obj.gameObject.name == "bone_8")
            {
                _animator.enabled = false;
            }
            Vector2 direction = obj.transform.position - explosion.transform.position;
            obj.GetComponent<Rigidbody2D>().AddForce(direction * _explosionForce);
        }
        if (_animatorDelay < 2)
        {
            _animatorDelay += 2f - _animatorDelay;
        }
        _mouseClickRingParticle.gameObject.transform.position = pos;
        Destroy(explosion);
    }

    private void StartDragging(Collider2D targetObject)
    {
        if (targetObject)
        {
            foreach (SpringJoint2D targetJoint in targetObject.GetComponents<SpringJoint2D>())
            {
                targetJoint.enabled = false;
            }
            _animator.enabled = false;
            targetObject.GetComponent<DistanceJoint2D>().enabled = false;
            _targetStartPos = targetObject.gameObject.transform.position;
            _selectedObj = targetObject.gameObject;
            _selectedRb = targetObject.transform.gameObject.GetComponent<Rigidbody2D>();
            _selectedRb.isKinematic = true;
        }
        if (_selectedRb)
        {
            _mouseForce = (_mousePos - _lastMousePosition) / Time.deltaTime;
            _mouseForce = Vector2.ClampMagnitude(_mouseForce, _maxMouseSpeed);
            _lastMousePosition = _mousePos;
        }
    }

    private void StopDragging()
    {
        foreach (SpringJoint2D targetJoint in _selectedObj.GetComponents<SpringJoint2D>())
        {
            targetJoint.enabled = true;
        }
        _animator.enabled = true;
        _selectedObj.GetComponent<DistanceJoint2D>().enabled = true;
        _selectedRb.isKinematic = false;
        _selectedRb.velocity = Vector2.zero;
        _selectedObj.transform.position = _targetStartPos;
        _targetStartPos = Vector2.zero;
        _selectedObj = null;
        _selectedRb = null;
    }

    private void EnableAnimatorBack()
    {
        if (_animatorDelay <= 0 && !_animator.isActiveAndEnabled && !_selectedObj)
        {
            _canEnableAnimatorBack = true;
        }
    }
}
