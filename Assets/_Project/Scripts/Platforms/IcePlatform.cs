using UnityEngine;
using TarodevController;

public class IcePlatform : MonoBehaviour  
{
    [SerializeField] private float _iceDeceleration = 10;
    [SerializeField] private float _iceHeightLimit = 30;
    
    private void OnCollisionStay2D(Collision2D collision) //reduces acceleration when touching ice
    {
        if (!collision.gameObject.CompareTag("Player")) return;
        if (collision.gameObject.TryGetComponent(out PlayerController controller))
        {
            controller.Acceleration = _iceDeceleration;
            controller.JumpHeight = _iceHeightLimit;
        }
    }

    private void OnCollisionExit2D(Collision2D collision) // return basic acceleration while leaving ice
    {
        if (!collision.gameObject.CompareTag("Player")) return;
        if (collision.gameObject.TryGetComponent(out PlayerController controller))
        {
            controller.ReturnBasicStats();
        }
    }
}
