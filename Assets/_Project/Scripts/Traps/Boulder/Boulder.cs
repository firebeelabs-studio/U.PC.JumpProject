using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class Boulder : MonoBehaviour
{
    [SerializeField] private float _xForce = 20f;
    private IObjectPool<Boulder> _pool;
    private ConstantForce2D _constantForce2D;
    [SerializeField] private bool _randomDirection;
    private int _value;

    private void Start()
    {
        _constantForce2D = GetComponent<ConstantForce2D>();
    }
    public void SetPool(IObjectPool<Boulder> pool)
    {
        _pool = pool;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("DestroyLine"))
        {
            _pool.Release(this);
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Ground")
        {
            _constantForce2D.force = new Vector2(_xForce, 0f);
        }
    }

    private void OnEnable()
    {
        if (_randomDirection)
        {
            _value = Random.Range(0, 2);
            if (_value == 0)
            {
                _xForce = _xForce * (-1);
            }
            else if (_value == 1)
            {
                _xForce = _xForce * 1;
            }
        }
    }
}
