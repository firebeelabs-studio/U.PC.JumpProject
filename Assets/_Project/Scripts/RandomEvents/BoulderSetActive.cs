using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoulderSetActive : MonoBehaviour
{
    [SerializeField] private GameObject _boulder;
    [SerializeField] private Transform _position;
    [SerializeField] private float _interpolationTime = 5f;
    private float _time = 0f;

    private void Start()
    {
        _boulder.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.name.Equals("Player"))
        {
            _boulder.SetActive(true);
        }
    }

    private void RespawnBoulder()
    {
        if (_boulder.transform.position != _position.transform.position && _time >= _interpolationTime)
        {
            Instantiate(_boulder, _position.position, _position.rotation);
            _time = 0.0f;
        }
    }

    private void Update() 
    {
        _time += Time.deltaTime;
        RespawnBoulder();
    }
}
