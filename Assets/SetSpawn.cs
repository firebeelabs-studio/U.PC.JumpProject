using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSpawn : MonoBehaviour
{
    Transform _pos;

    private void Awake()
    {
        _pos = GetComponent<Transform>();
    }

    void Start()
    { 
        _pos.position = Vector3.zero;
    }
}
