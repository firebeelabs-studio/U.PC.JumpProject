using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FinishPanelManagement : MonoBehaviour
{
    [SerializeField] private GameObject _finishPanel;
    [SerializeField] private TMP_Text _timerText;
    [SerializeField] private TMP_Text _yourTimeText;
    [SerializeField] private TMP_Text _previousTimeText;
    [SerializeField] private TMP_Text _timeNeededForNextStarText;
    [SerializeField] private TimerSinglePlayer _endLevelTimers;
    [SerializeField] private Button _backToMenuButton;
    [SerializeField] private Button _restartButton;
    [SerializeField] private Respawn _spawnManager;
    [SerializeField] private Transform _player;
    [SerializeField] private FinishSinglePlayer _finish;
    [SerializeField] private StarAnim[] _stars;
    [SerializeField] private List<float> _thresholds = new();

    private void Awake()
    {
        _spawnManager = FindObjectOfType<Respawn>();
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _finish = GameObject.FindGameObjectWithTag("Finish").GetComponent<FinishSinglePlayer>();
    }

    private void Start()
    {
        _backToMenuButton.onClick.AddListener(() => { SceneManager.LoadScene("Feature-MenuPet"); });
        _restartButton.onClick.AddListener(() => 
        {
            _finishPanel.SetActive(false);
            StartCoroutine(_spawnManager.RespawnPlayer(_player));
            _finish.IsFinished = false;
            _timerText.SetText("00:00");
        });
    }

    private void OnEnable()
    {
        FinishSinglePlayer.RunFinish += OnRunFinish;
    }
    private void OnDisable()
    {
        FinishSinglePlayer.RunFinish -= OnRunFinish;
    }

    private void OnRunFinish()
    {
        _finishPanel.SetActive(true);
        SetupThresholdsDescending();
        _yourTimeText.text = $"Your time: {(int)_endLevelTimers.TimeInSeconds}";
        _previousTimeText.text = _endLevelTimers.Times.Count > 1 ? $"Previous time: {(int)_endLevelTimers.Times[^2]}s" : "Your first try was Swamptastic!";
        StartCoroutine(SetupStars());
    }

    private IEnumerator SetupStars()
    {
        yield return new WaitForSeconds(.25f);
        for (int i = 0; i < _thresholds.Count; i++)
        {
            if (_endLevelTimers.TimeInSeconds <= _thresholds[i])
            {
                _stars[i].gameObject.SetActive(true);
                _stars[i].RunPunchAnimation();
            }
            yield return new WaitForSeconds(.75f);
        }
    }

    private void SetupThresholdsDescending()
    {
        _thresholds.Sort();
        _thresholds.Reverse();
    }


    public void Reset()
    {
        
    }
}
