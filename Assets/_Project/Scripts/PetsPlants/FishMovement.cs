using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishMovement : MonoBehaviour
{
    private bool _hasTarget = false;
    private bool _isTurning;
    private Vector3 _waypoint;
    private Vector3 _lastWaypoint = new Vector3(0f, 0f, 0f);
    private Animator _animator;
    [SerializeField]
    private float _speed;
    private float _rotateSpeed = 30f;
    [SerializeField]
    private List<Transform> Waypoints = new List<Transform>();

    private void Start()
    {
        _animator = GetComponent<Animator>();
        GetWaypoints();
    }

    private void Update()
    {
        if (!_hasTarget)
        {
            _hasTarget = CanFindTarget();
        }
        else
        {
            RotateNPC(_waypoint, _rotateSpeed);
            transform.position = Vector3.MoveTowards(transform.position, _waypoint, _speed * Time.deltaTime);
        }

        if (transform.position == _waypoint)
        {
            _hasTarget = false;
        }
    }

    bool CanFindTarget(float start = 1f, float end = 7f)
    {
        _waypoint = RandomWaypoint();

        if (_lastWaypoint == _waypoint)
        {
            _waypoint = RandomWaypoint();
            return false;
        }
        else
        {
            _lastWaypoint = _waypoint;
            _speed = Random.Range(start, end);
            _animator.speed = _speed;
            return true;
        }
    }

    private void RotateNPC(Vector3 waypoint, float currentSpeed)
    {
        float TurnSpeed = currentSpeed * Random.Range(1f, 3f);

        float rotate = waypoint.z - this.transform.position.z;
        //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(LookAt), TurnSpeed * Time.deltaTime);
        transform.localEulerAngles = new Vector3(0, 0, rotate);
    }

    //private void RotateOnZAxis(Vector3 waypoint, float currentSpeed)
    //{
        //Quaternion temp = transform.rotation;
        //float TurnSpeed = currentSpeed * Random.Range(1f, 3f);

        //Vector2 LookAt = waypoint - this.transform.position;
        //temp = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(LookAt), TurnSpeed * Time.deltaTime);
        //transform.Rotate(0, 0, temp.z);
        //Vector3 relativePos = waypoint.position - transform.position;
        //transform.rotation = Quaternion.LookRotation(relativePos);
    //}

    private Vector2 RandomWaypoint()
    {
        int randomWP = Random.Range(0, (Waypoints.Count - 1));
        Vector2 randomWaypoint = Waypoints[randomWP].transform.position;
        return randomWaypoint;
    }

    private void GetWaypoints()
    {
        Transform[] wpList = this.transform.GetComponentsInChildren<Transform>();
        for (int i = 0; i < wpList.Length; i++)
        {
            if (wpList[i].tag == "waypoint")
            {
                Waypoints.Add(wpList[i]);
            }
        }
    }

}
