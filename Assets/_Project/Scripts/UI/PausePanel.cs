using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PausePanel : MonoBehaviour
{
    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private Button _resumeButton;
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _homeButton;
    [SerializeField] private string _homeSceneName;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Respawn _spawnManager;
    [SerializeField] private Transform _player;

    private PlayersInput _inputs;

    void Start()
    {
        if (_player)
        {
            _inputs = _player.GetComponent<PlayersInput>();
        }

        if (_resumeButton)
        {
            _resumeButton.GetComponent<Button>().onClick.AddListener(() => { _pausePanel.SetActive(false); });
        }

        if (_restartButton)
        {
            if (!_spawnManager)
            {
                Debug.Log("SpawnManager reference is missing");
            }
            else if (!_player)
            {
                Debug.Log("Player reference is missing");
            }
            else
            {
                _restartButton.GetComponent<Button>().onClick.AddListener(() => { StartCoroutine(_spawnManager.RespawnPlayer(_player));
                                                                                  _pausePanel.SetActive(false); });
            }
        }

        if (_homeButton)
        {
            _homeButton.GetComponent<Button>().onClick.AddListener(() => { SceneManager.LoadScene(_homeSceneName); });
        }
        //_settingsButton.GetComponent<Button>().onClick.AddListener(() => {  });
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            _pausePanel.SetActive(_pausePanel.activeInHierarchy ? false : true);
        }
        if (_pausePanel.activeInHierarchy && _inputs.enabled)
        {
            _inputs.enabled = false;
            Time.timeScale = 0;
        }
        else if (!_pausePanel.activeInHierarchy && !_inputs.enabled)
        {
            _inputs.enabled = true;
            Time.timeScale = 1;
        }
    }
}
