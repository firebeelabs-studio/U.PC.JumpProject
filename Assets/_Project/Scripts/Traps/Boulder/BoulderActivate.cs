using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoulderActivate : MonoBehaviour
{
    [SerializeField] private GameObject _boulder;
    [SerializeField] private Transform _position;

    private void Start()
    {
        _boulder.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _boulder.SetActive(true);
        }
    }
}
