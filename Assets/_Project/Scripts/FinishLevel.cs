using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLevel : MonoBehaviour
{
    public static event Action EndRun;

    private void OnTriggerEnter2D(Collider2D col)
    {
        EndRun?.Invoke();
    }
}
