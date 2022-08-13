using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResumeMenuButtons : MonoBehaviour
{
    [SerializeField] private Respawn _spawnManager;
    [SerializeField] private Transform _player;
    [SerializeField] private FinishSinglePlayer _finish;
    public void Reset()
    {
        StartCoroutine(_spawnManager.RespawnPlayer(_player));
        _finish.IsFinished = false;
    }

    public void LoadMainLobby()
    {
        SceneManager.LoadScene("Feature-MenuPet");
    }
}
