using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedPlatforms : MonoBehaviour
{
    [SerializeField] private GameObject _platforms1;
    [SerializeField] private GameObject _platforms2;
    [SerializeField] private float _timeToSwap = 5f;

    private void Start()
    {
        PlatformsSetActive(true, _platforms1);
        PlatformsSetActive(false, _platforms2);
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("Player"))
        {
            StartCoroutine("PlatformSpawn");
        }
    }

    IEnumerator PlatformSpawn()
    {
        PlatformsSetActive(false, _platforms1);
        PlatformsSetActive(true, _platforms2);

        yield return new WaitForSeconds(_timeToSwap);

        PlatformsSetActive(true, _platforms1);
        PlatformsSetActive(false, _platforms2);
    }

    private void PlatformsSetActive(bool active, GameObject platformsType)
    {
        platformsType.SetActive(active);
    }
}
