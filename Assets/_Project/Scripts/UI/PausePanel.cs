using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

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

    private void Awake()
    {
        //TODO: change it before launch
        _spawnManager = FindObjectOfType<Respawn>();
        _player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Start()
    {
        _pausePanel.transform.localScale = Vector3.zero;
        if (_player)
        {
            _inputs = _player.GetComponent<PlayersInput>();
        }
        _resumeButton.GetComponent<Button>().onClick.AddListener(() => { TogglePanel(); });
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
            _restartButton.GetComponent<Button>().onClick.AddListener(() => {
                                                                                _spawnManager.ChangeSpawnPos(_spawnManager.StartPos, null);
                                                                                StartCoroutine(_spawnManager.RespawnPlayer(_player));
                                                                                TogglePanel(); 
                                                                            });
        }
        _homeButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            Time.timeScale = 1;
            LoadingScreenCanvas.Instance.LoadScene(_homeSceneName);
        } );
        //_settingsButton.GetComponent<Button>().onClick.AddListener(() => {  });
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            TogglePanel();
        }
    }
    private void TogglePanel()
    {
        if (_pausePanel.activeInHierarchy)
        {
            
            _pausePanel.gameObject.transform.
                DOScale(0, 0.15f).
                SetEase(Ease.InOutCubic).
                OnComplete(() =>
                {
                    if (_inputs && !_inputs.enabled)
                    {
                        _inputs.enabled = true;
                    }
                    Time.timeScale = 1;
                    _pausePanel.SetActive(false);
                }).SetUpdate(true);
        }
        else
        {
            _pausePanel.SetActive(true);
            _pausePanel.gameObject.transform.
               DOScale(1, 0.15f).
               SetEase(Ease.InOutCubic).
               OnComplete(() =>
               {
                   if (_inputs && _inputs.enabled)
                   {
                       _inputs.enabled = false;
                   }
                   Time.timeScale = 0;
               }).SetUpdate(true);
        }
    }
}
