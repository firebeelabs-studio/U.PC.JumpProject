using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResumeMenuButtons : MonoBehaviour
{
    [SerializeField] private Respawn _spawnManager;
    [SerializeField] private Transform _player;
    public void Reset()
    {
        StartCoroutine(_spawnManager.RespawnPlayer(_player));
    }

    public void LoadMainLobby()
    {
        SceneManager.LoadScene("Feature-MenuPet");
    }
}
