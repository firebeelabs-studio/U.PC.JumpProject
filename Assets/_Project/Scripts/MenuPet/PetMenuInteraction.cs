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

    private Vector3 _mousePos, offset, _mouseForce, _lastMousePosition, _targetStartPos;
    private GameObject _selectedObj;
    private Rigidbody2D _selectedRb;

    private float _counter = 0;

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

        if (_counter < 0.5f && Input.GetMouseButtonUp(0) && _selectedObj == null)
        {
            Explode(_mousePos);
            _mouseClickRingParticle.Play();
        }

        Collider2D targetObject = Physics2D.OverlapPoint(_mousePos);
        if (_counter >= 0.5f && _selectedRb == null)
        {
            StartDragging(targetObject);
        }
        else if (Input.GetMouseButtonUp(0) && _selectedRb)
        {
            StopDragging();
        }
    }
    void FixedUpdate()
    {
        if (_selectedRb)
        {
            _selectedRb.MovePosition(Vector3.Lerp(_selectedRb.transform.position, _mousePos + offset, Time.fixedDeltaTime * 10f));
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
            Vector2 direction = obj.transform.position - explosion.transform.position;
            obj.GetComponent<Rigidbody2D>().AddForce(direction * _explosionForce);
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
            targetObject.GetComponent<DistanceJoint2D>().enabled = false;
            _targetStartPos = targetObject.gameObject.transform.position;
            _selectedObj = targetObject.gameObject;
            _selectedRb = targetObject.transform.gameObject.GetComponent<Rigidbody2D>();
            _selectedRb.isKinematic = true;
            offset = _selectedRb.transform.position - _mousePos;
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
        _selectedObj.GetComponent<DistanceJoint2D>().enabled = true;
        _selectedRb.isKinematic = true;
        _selectedRb.velocity = Vector2.zero;
        _selectedObj.transform.position = _targetStartPos;
        _targetStartPos = Vector2.zero;
        _selectedObj = null;
        _selectedRb = null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_mousePos, 0.5f);
    }
}
