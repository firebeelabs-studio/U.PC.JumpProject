using UnityEngine;
using TarodevController;

public class BoostsNFT : MonoBehaviour
{
    private PlayerController _playerController;
    
    [SerializeField] private bool _hasAccelerationNFT;
    [SerializeField] private bool _hasMaxSpeedNFT;
    [SerializeField] private bool _hasBetterControlNFT;

    [SerializeField] private float _accelerationBoost;
    [SerializeField] private float _maxSpeedBoost;
    [SerializeField] private float _betterControlBoost;

    [ContextMenu("Do Something")]
    public void Boost()
    {
        if (!_hasAccelerationNFT)
        {
            _accelerationBoost = 0;
        }
        if (!_hasMaxSpeedNFT)
        {
            _maxSpeedBoost = 0;
        }
        if (!_hasBetterControlNFT)
        {
            _betterControlBoost = 0;
        }

        _playerController = FindObjectOfType<PlayerController>();
        if (_playerController is null) return;
        _playerController.SetBoosts(_accelerationBoost, _maxSpeedBoost, _betterControlBoost);

    }
}
