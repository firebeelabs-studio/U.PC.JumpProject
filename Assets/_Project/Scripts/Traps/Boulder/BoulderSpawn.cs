using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoulderSpawn : MonoBehaviour
{
    [SerializeField] private GameObject _boulder;
    [SerializeField] private Transform _position;
    //[SerializeField] private float _timeToSpawn = 5f;
    [SerializeField] private float _timeToDestroy = 10f;
    //private float _spawningTime;
    private float _aliveTime;
    private bool _isAlive = false;

    private void Update()
    {
        if (_isAlive == false)
        {
            _isAlive = true;
            _boulder.SetActive(false);
        }

        if (_isAlive == true)
        {
            _boulder.SetActive(true);
            _aliveTime += Time.deltaTime;
        }

        if (_aliveTime >= _timeToDestroy)
        {
            _isAlive = false;
            _boulder.transform.position = _position.transform.position;
            _aliveTime = 0.0f;
        }
    }
}