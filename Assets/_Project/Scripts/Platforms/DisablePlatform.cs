using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisablePlatform : MonoBehaviour
{
    [SerializeField] private GameObject _platform;
    [SerializeField] private float _hidePlatformTime = 1.5f;
    [SerializeField] private float _showPlatformTime = 1f;
    [SerializeField] private ParticleSystem _destroyParticle;

    public void Disable() => StartCoroutine(SetUnActive());

    public void Enable() => StartCoroutine(SetActive());

    private IEnumerator SetActive()
    {
        yield return new WaitForSeconds(_hidePlatformTime);
        _platform.SetActive(true);
    }
    private IEnumerator SetUnActive()
    {
        yield return new WaitForSeconds(_showPlatformTime);
        _platform.SetActive(false);
        _destroyParticle.Play();
        StartCoroutine(SetActive());
    }
}
