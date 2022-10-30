using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class MovingClouds : MonoBehaviour
{
    [SerializeField] private GameObject[] _objects;
    [SerializeField] private float _speed;
    [SerializeField] private bool _moveLeft = false;
    [SerializeField] private Transform _leftBorder;
    [SerializeField] private Transform _rightBorder;

    private float _cloudsObjXSize;

    private void Start()
    {
        
        //IDIOT FILTER
        if (_speed < 0)
        {
            _speed *= -1;
        }
        
        _cloudsObjXSize = _objects.FirstOrDefault().GetComponent<SpriteRenderer>().size.x;
        for (int i = 0; i < _objects.Length; i++)
        {
            _objects[i].transform.position = new Vector2((_moveLeft ? _rightBorder.position.x : _leftBorder.position.x) + (_moveLeft ? -1.5f*_cloudsObjXSize * i : 1.5f*_cloudsObjXSize * i), _leftBorder.position.y);
        }
    }

    private void Update()
    {
        MoveClouds(_moveLeft);
    }

    private void MoveClouds(bool moveLeft)
    {
        foreach (var cloud in _objects)
        {
            cloud.transform.position = Vector2.MoveTowards(cloud.transform.position, moveLeft ? _leftBorder.transform.position : _rightBorder.transform.position, _speed * Time.deltaTime);
            
            Vector3 position = Vector3.zero;
            if (moveLeft && cloud.transform.position.x <= _leftBorder.transform.position.x)
            {
                position = _rightBorder.position;
                cloud.transform.position = new Vector2(position.x, position.y);
            }

            if (!_moveLeft && cloud.transform.position.x >= _rightBorder.transform.position.x)
            {
                position = _leftBorder.position;
                cloud.transform.position = new Vector2(position.x, position.y);
            }
        }
    }
    
}
