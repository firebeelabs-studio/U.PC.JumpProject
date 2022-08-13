using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] private ICheckpointAnim _checkpointAnimation;
    private bool _isActive;
    private Respawn _spawnManager;
    private Transform _respawnPos;
    private void Awake()
    {
        _spawnManager = FindObjectOfType<Respawn>();
    }
    private void Start()
    {
        _respawnPos = transform.GetChild(0).transform;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        _spawnManager.ChangeSpawnPos(_respawnPos, this);
    }

    public void RestetCheckPoint()
    {
        _checkpointAnimation.ResetToDefaultState();
    }
}
