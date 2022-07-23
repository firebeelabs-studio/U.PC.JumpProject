using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCompletedSingle : MonoBehaviour
{
    public static event Action RunFinish;
    private bool isFinished;
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (isFinished) return;
        isFinished = true;
        RunFinish?.Invoke();
        FindObjectOfType<AudioManager>().Play("Finish");
    }
}
