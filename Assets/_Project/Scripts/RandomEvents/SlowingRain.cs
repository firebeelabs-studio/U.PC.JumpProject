using UnityEngine;
using TarodevController;

public class SlowingRain : MonoBehaviour
{
    [SerializeField] private float _slowingPower;
    [SerializeField] private float _slowDuration;
    [SerializeField] private float _timeBtwSlows;
    [SerializeField] private ParticleSystem _rainParticleSystem;
    private float _timer;
    private bool _shouldResetMoveClamp = false;
    private PlayerController _playerController;
    private void Start()
    {
        _timer = 0;
    }
    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Player"))
        {
            if (_timer < 0)
            {
                other.TryGetComponent(out _playerController);
                if (_playerController is null) return;
                SlowDownPlayer(_playerController);
                _shouldResetMoveClamp = true;
            }
            else
            {
                _timer += _timeBtwSlows;
            }
        }
    }
    private void Update()
    {
        _timer -= Time.deltaTime;
        if (_shouldResetMoveClamp && _timer < 0)
        {
            _playerController.ChangeMoveClamp(-_slowingPower);
            _shouldResetMoveClamp = false;
        }
    }
    private void SlowDownPlayer(PlayerController playerController)
    {
        playerController.ChangeMoveClamp(_slowingPower);
        _timer = _timeBtwSlows;
    }
}
