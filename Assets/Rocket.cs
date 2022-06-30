using TarodevController;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    [Space(10)]
    [Header("Spawning")]
    [SerializeField] private GameObject _spawnPosition;
    [SerializeField] private float _spawnRange;

    [Header("RocketVariables")]
    [SerializeField] private GameObject _rocketObj;
    [SerializeField] private float _rocketSpeed;
    [SerializeField] private float _explosionForce;
    [SerializeField] private ParticleSystem _explosionParticle;

    private float _randomPosition;
    private float _playerPosY;
    private bool _shouldMove = false;

    private void Update()
    {
        if (!_shouldMove) return;
        MoveRocket();
    }


    [ContextMenu("SpawnRocket")]
    private void SpawnRocket()
    {
        _randomPosition = Random.Range(_spawnPosition.transform.position.y - _spawnRange, _spawnPosition.transform.position.y + _spawnRange); //change it to the position of currently winning player (server side)
        _rocketObj.gameObject.transform.position = new Vector2(_spawnPosition.transform.position.x, _randomPosition);
        _rocketObj.SetActive(true);
        _shouldMove = true;
    }
    private void DestroyRocket()
    {
        _explosionParticle.gameObject.transform.position = _rocketObj.transform.position;
        _explosionParticle.Play();
        _rocketObj.SetActive(false);
        _shouldMove = false;
    }
    private void MoveRocket()
    {
        _rocketObj.gameObject.transform.position += Vector3.left * _rocketSpeed * Time.deltaTime;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController playerController;
            collision.TryGetComponent(out playerController);
            if (playerController is null) return;
            var dir = collision.transform.position - _rocketObj.transform.position;
            playerController.AddForce(dir * _explosionForce, PlayerForce.Burst, true);
            DestroyRocket();
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(new Vector2(_spawnPosition.transform.position.x, _spawnPosition.transform.position.y - _spawnRange), new Vector2(_spawnPosition.transform.position.x, _spawnPosition.transform.position.y + _spawnRange));
        Gizmos.color = Color.red;
        Gizmos.DrawRay(_spawnPosition.transform.position, Vector3.left);
    }
}
