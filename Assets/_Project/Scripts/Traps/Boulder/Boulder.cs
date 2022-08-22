using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
public class Boulder : MonoBehaviour
{
    private IObjectPool<Boulder> _pool;
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
}
