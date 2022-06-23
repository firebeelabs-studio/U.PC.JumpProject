using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TarodevController;

public class TriangularBouncer : MonoBehaviour
{
    [SerializeField] private float _bounceForce = 20;
    private Vector2 _bounceDirectionVector;
    [SerializeField] private Direction _bouncerDirection;
    private bool _cancelMovement = true;
    private enum Direction
    {
        Up,
        Side
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (_bouncerDirection == Direction.Side)
        {
            _bounceDirectionVector = new Vector2(-transform.localScale.y, transform.localScale.x);
        }
        else
        {
            _bounceDirectionVector = transform.up.normalized;
            _cancelMovement = false;
        }
        if (!other.gameObject.CompareTag("Player")) return;
        if (other.collider.TryGetComponent(out IPlayerController controller))
        {
            controller.AddForce(_bounceDirectionVector * _bounceForce, PlayerForce.Burst, _cancelMovement);
        }
    }
}
