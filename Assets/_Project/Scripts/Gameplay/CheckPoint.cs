using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//setup Interface you bastard
[RequireComponent(typeof(ICheckpointAnim))]
public class CheckPoint : MonoBehaviour
{
    private ICheckpointAnim _checkpointAnim;
    private bool _isActive;
    private Respawn _spawnManager;
    private Transform _respawnPos;
    private void Awake()
    {
        _checkpointAnim = GetComponent<ICheckpointAnim>();

        _spawnManager = FindObjectOfType<Respawn>();
    }
    private void Start()
    {
        _respawnPos = transform.GetChild(0).transform;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if(_isActive) return;
            _isActive = true;
            _spawnManager.ChangeSpawnPos(_respawnPos, this);
        }
    }

    public void ResetCheckPoint()
    {
        _checkpointAnim.ResetToDefaultState();
        _isActive = false;
    }
}
