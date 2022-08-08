using System.Collections;
using TarodevController;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    [Header("Spawning")]
    [SerializeField] private GameObject _spawnPosition;
    [SerializeField] private float _spawnRange;
    [Space(10)]
    [Header("RocketVariables")]
    [SerializeField] private GameObject _rocketObj;
    [SerializeField] private float _rocketSpeed;
    [SerializeField] private Direction _direction;
    [SerializeField] private bool _useSinWave;
    [SerializeField] private float _explosionForce;
    [Space(10)]
    [SerializeField] private ParticleSystem _explosionParticle;
    [Space(10)]
    [Header("Sin properties")]
    [SerializeField] private float _frequency = 5f;
    [SerializeField] private float _amplitude = 0.05f;
    [SerializeField] private float _maxAngle = 30f;

    private float _randomPosition; //TO DO: remove
    private bool _shouldMove = false;
    private Vector3 _directionVector = Vector3.left;
    private float _localScaleX;
    private float _localScaleY;
    private void Start()
    {
        _localScaleX = _rocketObj.transform.localScale.x;
        _localScaleY = _rocketObj.transform.localScale.y;
    }
    private enum Direction
    {
        Left,
        Right
    }
    private void Update()
    {
        if (!_shouldMove) return;
        MoveRocket();
    }
    [ContextMenu("SpawnRocket")]
    private void SpawnRocket()
    {
        RocketSetup();
        if (_rocketObj.activeInHierarchy)
        {
            DestroyRocket();
        }
        _randomPosition = Random.Range(_spawnPosition.transform.position.y - _spawnRange, _spawnPosition.transform.position.y + _spawnRange); //TO DO: change it to the position of currently winning player (server side)
        _rocketObj.transform.position = new Vector2(_spawnPosition.transform.position.x, _randomPosition); //sets the starting position for rocket; random position will be replaced with position of first player
        _rocketObj.transform.localRotation = Quaternion.identity; //resets the rotation
        _rocketObj.SetActive(true);
        _shouldMove = true;
    }
    private void DestroyRocket()
    {
        _explosionParticle.gameObject.transform.position = _rocketObj.transform.position; //sets position for particle system
        _explosionParticle.Play();
        _rocketObj.SetActive(false);
        _shouldMove = false;
    }
    private void MoveRocket()
    {
        if (_useSinWave)
        {
            _rocketObj.transform.position += (_directionVector * _rocketSpeed * Time.deltaTime +  (transform.up * Mathf.Sin(Time.time * _frequency) * _amplitude)); //constantly moves rocket to the specified direction in sin wave
            _rocketObj.transform.localRotation = Quaternion.Euler(0, 0, _maxAngle * Mathf.Sin(Time.time * _frequency)); //constantly rotates rocket to match the direction
        }
        else
        {
            _rocketObj.transform.position += _directionVector * _rocketSpeed * Time.deltaTime; //constantly moves rocket to the specified direction
        }
    }
    private void RocketSetup()
    {
        if (_direction == Direction.Right)
        {
            _directionVector = Vector3.right;
            if (_rocketObj.transform.localScale.x > 0)
            {
                _rocketObj.gameObject.transform.localScale = new Vector2(-_localScaleX, _localScaleY);
            }
            if (_maxAngle < 0)
            {
                _maxAngle *= -1;
            }
        }
        else
        {
            _directionVector = Vector3.left;
            if (_rocketObj.transform.localScale.x < 0)
            {
                _rocketObj.gameObject.transform.localScale = new Vector2(_localScaleX, _localScaleY);
            }
            if (_maxAngle > 0)
            {
                _maxAngle *= -1;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (collision.TryGetComponent(out PlayerController playerController))
            {
                Vector2 direction = collision.transform.position - _rocketObj.transform.position; //gets the direction to which player should be pushed
                if (playerController.Speed.magnitude > 0)
                {
                    playerController.AddForce(direction.normalized * _explosionForce, PlayerForce.Decay, true); //if players moves the explosion is stronger to eliminate the balancing of opposing forces
                }
                else
                {
                    playerController.AddForce(direction * (_explosionForce / 4), PlayerForce.Decay, true); //if player doesnt move the explosion is weaker
                }
                DestroyRocket();
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(new Vector2(_spawnPosition.transform.position.x, _spawnPosition.transform.position.y - _spawnRange), new Vector2(_spawnPosition.transform.position.x, _spawnPosition.transform.position.y + _spawnRange));
        Gizmos.color = Color.red;
        Gizmos.DrawRay(_spawnPosition.transform.position, Vector3.left);
        Gizmos.DrawRay(_spawnPosition.transform.position, Vector3.right);
    }
}
