using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishSinglePlayer : MonoBehaviour
{
    public static event Action RunFinish;
    public bool IsFinished;
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (IsFinished) return;
        IsFinished = true;
        RunFinish?.Invoke();
    }
}
