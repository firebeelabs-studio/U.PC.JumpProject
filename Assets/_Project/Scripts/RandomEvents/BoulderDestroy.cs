using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoulderDestroy : MonoBehaviour
{
    [SerializeField] private GameObject _boulder;
    [SerializeField] private Transform _position;

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.CompareTag("DestroyLine"))
        {
            _boulder.SetActive(false);
            _boulder.transform.position = _position.transform.position;
        }
    }
}
