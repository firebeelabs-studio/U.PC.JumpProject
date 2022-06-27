using UnityEngine;
using TarodevController;

public class MudPlatform : MonoBehaviour
{
    [SerializeField] private float _mudDeceleration = 100;
    [SerializeField] private float _reduceSpeed = 10;
    [SerializeField] private float _mudHeightLimit = 20;

    private void OnCollisionStay2D(Collision2D collision) //reduces acceleration, speed & jump range  when touching mud
    {
        if (!collision.gameObject.CompareTag("Player")) return;
        if (collision.gameObject.TryGetComponent(out PlayerController controller))
        {
            controller.Acceleration = _mudDeceleration;
            controller.MoveClamp = _reduceSpeed;
            controller.JumpHeight = _mudHeightLimit;
        }
    }

    private void OnCollisionExit2D(Collision2D collision) // return basic acceleration while leaving mud
    {
        if (!collision.gameObject.CompareTag("Player")) return;
        if (collision.gameObject.TryGetComponent(out PlayerController controller))
        {
            controller.ReturnBasicStats();
        }
    }
}
