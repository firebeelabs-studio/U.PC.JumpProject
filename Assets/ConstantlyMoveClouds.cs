using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantlyMoveClouds : MonoBehaviour
{
    [SerializeField] private Transform[] _clouds;
    [SerializeField] private float _speed;
    [SerializeField] private Transform _startPos;
    [SerializeField] private Transform _endPos;


    private void Update()
    {
        foreach (Transform cloud in _clouds)
        {
            cloud.position += new Vector3(_speed * Time.deltaTime, 0);
            if (cloud.position.x > _endPos.position.x)
            {
                cloud.position = _startPos.position;
            }
        }
    }
}
