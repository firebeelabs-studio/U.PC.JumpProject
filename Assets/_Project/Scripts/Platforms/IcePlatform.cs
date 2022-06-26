using UnityEngine;
using TarodevController;
using System.Collections;

public class IcePlatform : MonoBehaviour  
{
    //PlayerController _controller = new PlayerController();
    [SerializeField] private float _iceDeceleration = 10;
    [SerializeField] private float _iceHeightLimit = 35;
    
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
