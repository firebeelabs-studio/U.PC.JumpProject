using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformShake : MonoBehaviour
{
    private bool _shaking = false;
    [SerializeField] private float _shakeAmount;

    private void Update()
    {
        if (_shaking)
        {
            Vector3 newPos = Random.insideUnitSphere * (Time.deltaTime * _shakeAmount);
            newPos.y = transform.position.y;
            newPos.z = transform.position.z;

            transform.position = newPos;
        }
    }

    public void Shake()
    {
        StartCoroutine("ShakeNow");
    }

    IEnumerator ShakeNow()
    {
        Vector3 originalPos = transform.position;

        if (_shaking == false)
        {
            _shaking = true;
        }

        yield return new WaitForSeconds(0.25f);

        _shaking = false;
        transform.position = originalPos;
    }

}
