using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishSinglePlayer : MonoBehaviour
{
    public static event Action RunFinish;
    private bool _isFinished;
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (_isFinished) return;
        _isFinished = true;
        RunFinish?.Invoke();
    }
}
