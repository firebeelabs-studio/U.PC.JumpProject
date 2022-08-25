using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlatforms : MonoBehaviour
{
    [SerializeField] private GameObject _platforms1;
    [SerializeField] private GameObject _platforms2;

    private void Start()
    {
        PlatformsSetActive(true, _platforms1);
        PlatformsSetActive(false, _platforms2);
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("Player"))
        {
            switch (2)
            {
                case 1:
                    PlatformsSetActive(true, _platforms1);
                    PlatformsSetActive(false, _platforms2);
                    break;

                case 2:
                    PlatformsSetActive(false, _platforms1);
                    PlatformsSetActive(true, _platforms2);
                    break;
            }
        }
    }

    //private void OnTriggerExit2D(Collider2D coll)
    //{
    //    if (coll.CompareTag("Player"))
    //    {
    //        switch (2)
    //        {
    //            case 1:
    //                PlatformsSetActive(true, _platforms1);
    //                PlatformsSetActive(false, _platforms2);
    //                break;

    //            case 2:
    //                PlatformsSetActive(false, _platforms1);
    //                PlatformsSetActive(true, _platforms2);
    //                break;
    //        }
    //    }
    //}

    private void PlatformsSetActive(bool active, GameObject platformsType)
    {
        platformsType.SetActive(active);
    }
}
