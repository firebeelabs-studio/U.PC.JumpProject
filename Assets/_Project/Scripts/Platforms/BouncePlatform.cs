using UnityEngine;
using TarodevController;

public class BouncePlatform : MonoBehaviour
{
    [SerializeField] private float _bounceForce = 20;
    [SerializeField] private Direction _bouncerDirection;
    private Vector2 _bounceDirectionVector;
    private bool _cancelMovement = true;

    private enum Direction
    {
        Up,
        Side
    }

    private void Start()
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
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        if (collision.TryGetComponent(out IPlayerController controller))
        {
            controller.AddForce(_bounceDirectionVector * _bounceForce, PlayerForce.Burst, _cancelMovement);
        }
    }
}
