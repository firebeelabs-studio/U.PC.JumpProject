using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TarodevController;

public class TriangularBouncer : MonoBehaviour
{
    [SerializeField] float _bounceForce = 20;

    private void OnCollisionStay2D(Collision2D other)
    {
        Vector2 _bounceDirection = new Vector2(-transform.localScale.y, transform.localScale.x); 
        if (other.collider.TryGetComponent(out IPlayerController controller))
        {
            controller.AddForce(_bounceDirection * _bounceForce);
        }
    }
}
