using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PausePanel : MonoBehaviour
{
    [Header("PAUSE")]
    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private Button _resumeButton;
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _homeButton;
    [SerializeField] private string _homeSceneName;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Respawn _spawnManager;
    [SerializeField] private Transform _player;
    [Space(20)]
    [Header("SETTINGS")]
    [SerializeField] private GameObject _settingsPanel;
    [SerializeField] private Button _backToPauseMenuPanel;
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
            _restartButton.GetComponent<Button>().onClick.AddListener(() =>
            {
                FinishPanelManagement.RestartPlayer();
                TogglePanel();
            });
        }
        _homeButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            Time.timeScale = 1;
            LoadingScreenCanvas.Instance.LoadScene(_homeSceneName);
        } );
        _settingsButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            _settingsPanel.SetActive(true);
            _pausePanel.SetActive(false);
        });
        _backToPauseMenuPanel.onClick.AddListener(() =>
        {
            _settingsPanel.SetActive(false);
            _pausePanel.SetActive(true);
        });
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
        if (_pausePanel.activeInHierarchy || _settingsPanel.activeInHierarchy)
        {
            _settingsPanel.gameObject.transform.
                DOScale(0, 0.15f).
                SetEase(Ease.InOutCubic).
                OnComplete(() =>
                {
                    _settingsPanel.gameObject.transform.localScale = Vector3.one;
                    _settingsPanel.SetActive(false);
                }).SetUpdate(true);
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
