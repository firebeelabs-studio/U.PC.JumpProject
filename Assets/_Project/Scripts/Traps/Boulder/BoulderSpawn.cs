using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class BoulderSpawn : MonoBehaviour
{
    [SerializeField] private Boulder _boulderPrefab;
    [SerializeField] private GameObject _boulder;
    [SerializeField] private Transform _position;
    //[SerializeField] private float _timeToSpawn = 5f;
    [SerializeField] private float _timeToDestroy = 10f;
    //private float _spawningTime;
    private float _aliveTime;
    private bool _isAlive = false;
    [SerializeField] private float _xForce;
    [SerializeField] private bool _randomDirection;

    private float _timer = 0f;
    [SerializeField] private float _timeToSpawn = 5f;
    private ObjectPool<Boulder> _bouldersPool;

    private void Start()
    {
        _bouldersPool = new ObjectPool<Boulder>(CreateBoulder, OnGetBoulderFromPool, OnReturnBallToPool);
    }

    private Boulder CreateBoulder()
    {
        Boulder boulder = Instantiate(_boulderPrefab);
        boulder.XForce = _xForce;
        boulder.RandomDirection = _randomDirection;
        boulder.SetPool(_bouldersPool);
        return boulder;
    }

    private void OnGetBoulderFromPool(Boulder boulder)
    {
        boulder.gameObject.SetActive(true);
        boulder.transform.position = transform.position;
    }

    private void OnReturnBallToPool(Boulder boulder)
    {
        boulder.gameObject.SetActive(false);
    }

    private void Update()
    {
        _timer += Time.deltaTime;
        
        if (_timer >= _timeToSpawn)
        {
            _timer = 0;
            _bouldersPool.Get();
        }
        
        
        // if (_isAlive == false)
        // {
        //     _isAlive = true;
        //     _boulder.SetActive(false);
        // }
        //
        // if (_isAlive == true)
        // {
        //     _boulder.SetActive(true);
        //     _aliveTime += Time.deltaTime;
        // }
        //
        // if (_aliveTime >= _timeToDestroy)
        // {
        //     _isAlive = false;
        //     _boulder.transform.position = _position.transform.position;
        //     _aliveTime = 0.0f;
        // }
    }
}