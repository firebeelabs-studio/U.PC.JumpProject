using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingClouds : MonoBehaviour
{
    [SerializeField] private GameObject[] _objects;
    [SerializeField] private float _speed;
    [SerializeField] private Transform _startPos;
    [SerializeField] private Transform _endPos;


    private void Update()
    {
        foreach (GameObject obj in _objects)
        {
            obj.transform.position += new Vector3(_speed * Time.deltaTime, 0);
            if (obj.transform.position.x > _endPos.position.x)
            {
                obj.transform.position = _startPos.position;
            }
        }
    }
}
