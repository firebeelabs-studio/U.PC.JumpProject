using UnityEngine;
using TarodevController;

public class MudPlatform : MonoBehaviour
{
    private PlayerController _playerController;

    [SerializeField] private float _accReduction; // acceleration reduction value (slide when turn - harder to move)
    [SerializeField] private float _clampReduction; // reduces max speed - harder to move through
    [SerializeField] private float _jumpReduction; // reduces height of jump - as higher

    private bool _isTouchingMud = false;
    private bool _isDebuffActivated = false;
    
    private void OnTriggerStay2D(Collider2D collision) //reduces stats when touching mud
    {
        if (collision.gameObject.CompareTag("Player") && !_isTouchingMud)
        {
            collision.gameObject.TryGetComponent(out _playerController);
            if (_playerController is null) return;
            SetDebuffs(_playerController);
            _isTouchingMud = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) // return basic stats while leaving mud
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.TryGetComponent(out _playerController);
            if (_playerController is null) return;
            RemoveDebuffs(_playerController);
            _isTouchingMud = false;
        }
    }

    private void SetDebuffs(PlayerController playerController)
    {
        //playerController.MudDebuff(_accReduction, _jumpReduction, _clampReduction);
        _isDebuffActivated = true;
    }

    private void RemoveDebuffs(PlayerController playerController)
    {
        if (_isDebuffActivated)
        {
            //playerController.MudDebuff(-_accReduction, -_jumpReduction, -_clampReduction);
        }
        _isDebuffActivated = false;
    }
}
