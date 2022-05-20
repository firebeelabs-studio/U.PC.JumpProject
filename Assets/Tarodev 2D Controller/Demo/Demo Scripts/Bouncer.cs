using System;
using TarodevController;
using UnityEngine;

namespace Tarodev {
    public class Bouncer : MonoBehaviour 
    {
        [SerializeField] private float _bounceForce = 70;
        private Animator _anim;

        private void Awake()
        {
            _anim = GetComponent<Animator>();
        }

        private void OnCollisionStay2D(Collision2D other) 
        {
            if (other.collider.TryGetComponent(out IPlayerController controller)) {
                controller.AddForce(transform.up.normalized * _bounceForce);
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                _anim.Play("BouncerAnim");
            }
        }
    }
}