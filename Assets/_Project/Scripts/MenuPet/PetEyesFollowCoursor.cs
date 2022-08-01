using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetEyesFollowCoursor : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _maxDistance = 1f;
    private Vector3 _origin;

    private void Awake()
    {
        if (!_camera)
        {
            _camera = Camera.main;
        }
    }
    void Start()
    {
        _origin = transform.position;
    }
    void Update()
    {
        // Get the mouse position in world space rather than screen space
        var mouseWorldCoord = _camera.ScreenPointToRay(Input.mousePosition).origin;

        // Get a vector pointing from initialPosition to the target. Vector shouldn't be longer than maxDistance
        var originToMouse = mouseWorldCoord - _origin;
        originToMouse = Vector3.ClampMagnitude(originToMouse, _maxDistance);

        // Linearly interpolate from current position to mouse's position
        transform.position = Vector3.Lerp(transform.position, _origin + originToMouse, _speed * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_origin, _maxDistance);
    }
}
