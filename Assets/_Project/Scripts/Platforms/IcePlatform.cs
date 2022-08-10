using UnityEngine;
using TarodevController;

public class IcePlatform : MonoBehaviour  
{
    private PlayerController _playerController;

    [SerializeField] private float _accReduction; // acceleration reduction value (slide when turn)
    [SerializeField] private float _decReduction; // deceleration reduction value (slide when stop)

    private bool _isTouchingIce = false;
    private bool _isDebuffActivated = false;

    private void OnTriggerStay2D(Collider2D collision) //reduces stats when touching ice
    {
        if (collision.gameObject.CompareTag("Player") && !_isTouchingIce)
        {
            collision.gameObject.TryGetComponent(out _playerController);
            if (_playerController is null) return;
            SetDebuffs(_playerController);
            _isTouchingIce = true;
        }  
    }

    private void OnTriggerExit2D(Collider2D collision) // return basic stats while leaving ice
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.TryGetComponent(out _playerController);
            if (_playerController is null) return;
            RemoveDebuffs(_playerController);
            _isTouchingIce = false;
        }
    }

    private void SetDebuffs(PlayerController playerController)
    {
        //playerController.IceDebuff(_accReduction, _decReduction);
        _isDebuffActivated = true;
    }

    private void RemoveDebuffs(PlayerController playerController)
    {
        if (_isDebuffActivated)
        {
           // playerController.IceDebuff(-_accReduction, -_decReduction);
            _isDebuffActivated = false;
        }
    }
}
