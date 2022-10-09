using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenBuildWindowsFix : MonoBehaviour
{
    private void Awake()
    {
        QualitySettings.vSyncCount = 0;  // VSync must be disabled
    }
}
