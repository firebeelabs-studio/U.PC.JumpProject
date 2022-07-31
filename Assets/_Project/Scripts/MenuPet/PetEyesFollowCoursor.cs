using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetEyesFollowCoursor : MonoBehaviour
{
    [SerializeField] private float _factor = 0.25f;
    [SerializeField] private float limit = 0.08f;
    private Vector3 _followPosition;
    private Vector3 _center;

    void Start()
    {
        //Capture the starting position as a vector3
        _center = transform.position;
    }

    void Update()
    {
        //Convert mouse pointer cords into a worldspace vector3
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0.0f;

        _followPosition = Vector3.Lerp(_followPosition, pos, Time.deltaTime * _factor);

        //Create a dir target based on mouse vector * factor
        Vector3 dir = _followPosition * _factor;

        //Clamp the dir target
        dir = Vector3.ClampMagnitude(dir, limit);

        //Update the pupil position
        transform.position = _center + dir;
    }
}
