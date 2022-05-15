using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartRun : MonoBehaviour
{
    public static event Action RunStart;

    private void OnTriggerEnter2D(Collider2D col)
    {
        RunStart?.Invoke();
        //start timer
    }
}
