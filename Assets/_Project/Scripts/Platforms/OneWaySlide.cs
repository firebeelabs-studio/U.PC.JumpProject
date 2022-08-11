using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWaySlide : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private Animator _animator;
    [SerializeField] private ParticleSystem _particleSystem;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {

            GameObject.Find("Visual").TryGetComponent(out _animator);
            //_animator.transform.FindChild("Visual").TryGetComponent(out _animator);
            _animator.Play("OneWayPlatform");
            print(_animator.gameObject.name);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _particleSystem.gameObject.transform.position = new Vector2(collision.gameObject.transform.position.x, _particleSystem.gameObject.transform.position.y);
            _particleSystem.Play();
        }
    }
}
