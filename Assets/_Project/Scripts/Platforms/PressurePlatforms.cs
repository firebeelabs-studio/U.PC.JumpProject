using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlatforms : MonoBehaviour
{
    [SerializeField] private GameObject _platforms1;
    [SerializeField] private GameObject _platforms2;
    private bool _platformsActivated;

    private void Start()
    {
        PlatformsSetActive(true, _platforms1);
        PlatformsSetActive(false, _platforms2);
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("Player"))
        {
            if (!_platformsActivated)
            {
                PlatformsSetActive(false, _platforms1);
                PlatformsSetActive(true, _platforms2);
                _platformsActivated = true;
            }
            else if (_platformsActivated)
            {
                PlatformsSetActive(true, _platforms1);
                PlatformsSetActive(false, _platforms2);
                _platformsActivated = false;
            }
        }
    }
    private void PlatformsSetActive(bool active, GameObject platformsType)
    {
        platformsType.SetActive(active);
    }
}
