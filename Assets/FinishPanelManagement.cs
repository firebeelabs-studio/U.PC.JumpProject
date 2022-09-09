using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;
using System;
using TarodevController;

public class FinishPanelManagement : MonoBehaviour
{
    [SerializeField] private GameObject _finishPanel;
    [SerializeField] private Image _darkeningImage;
    [SerializeField] private TMP_Text _newScoreText;
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
    [SerializeField] private UIParticleSystem _confettiParticles;
    [SerializeField] private StarAnim[] _stars;
    [SerializeField] private List<float> _thresholds = new();
    private TimerSinglePlayer _timerSinglePlayer;
    public static event Action PlayerRestart;
    private PlayersInput input;
    private IPawnController _pawnController;
    private float _previousMoveClamp;

    private void Awake()
    {
        _timerSinglePlayer = GetComponentInParent<TimerSinglePlayer>();
        _spawnManager = FindObjectOfType<Respawn>();
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _finish = GameObject.FindGameObjectWithTag("Finish").GetComponent<FinishSinglePlayer>();
    }

    private void Start()
    {
        _backToMenuButton.onClick.AddListener(() => { SceneManager.LoadScene("Feature-MenuPet"); });
        _restartButton.onClick.AddListener(() => 
        {
            _newScoreText.gameObject.SetActive(false);
            _finishPanel.SetActive(false);
            RestartPlayer();
        });
        input = _player.GetComponent<PlayersInput>();
        _pawnController = _player.GetComponent<IPawnController>();
        _previousMoveClamp = _player.GetComponent<PawnController>().MoveClamp;
    }

    private void OnEnable()
    {
        FinishSinglePlayer.RunFinish += OnRunFinish;
        PlayerRestart += OnPlayerRestart;
    }

    private void OnDisable()
    {
        FinishSinglePlayer.RunFinish -= OnRunFinish;
        PlayerRestart -= OnPlayerRestart;
    }

    private void OnRunFinish()
    {
        input.enabled = false;
        _pawnController.ChangeMoveClamp(0);

        // Finish Panel Text
        _yourTimeText.text = $"Your time: {(int)_endLevelTimers.TimeInSeconds}s";
        _previousTimeText.text = _endLevelTimers.Times.Count > 1 ? $"Previous time: {(int)_endLevelTimers.Times[^2]}s" : "Your first try was Swamptastic!";
        int timeInSeconds = (int)_endLevelTimers.TimeInSeconds;
        if (timeInSeconds <= _thresholds[2])
        {
            _timeNeededForNextStarText.text = $"Congratulations! You've achieved all stars!";
        }
        else
        {
            float timeForNextStar = 0;
            foreach (var threshold in _thresholds.OrderBy(x => x))
            {
                if (timeInSeconds > threshold)
                {
                    timeForNextStar = threshold;
                }
                else
                {
                    break;
                }
            }
            _timeNeededForNextStarText.text = $"Time needed for next star: {timeForNextStar}s";
        }

        // Darkening
        _darkeningImage.DOColor(new Color32(0, 0, 0, 100), 3).SetEase(Ease.Linear).OnComplete(() =>
        {
            //_darkeningImage.DOColor(new Color32(0, 0, 0, 0), 2).SetEase(Ease.Linear);
        });

        // Timer Text
        RectTransform timerTextRect = _timerText.GetComponent<RectTransform>();
        timerTextRect.DOScale(0, 0.5f).SetEase(Ease.Linear);

        // New Record Text
        _newScoreText.text = "NEW RECORD! " + _timerText.text;
        _newScoreText.gameObject.SetActive(true);
        _confettiParticles.StartParticleEmission();
        RectTransform newScoreTextRect = _newScoreText.GetComponent<RectTransform>();
        newScoreTextRect.localScale = Vector2.zero;
        newScoreTextRect.DOScale(1.5f, 3).SetEase(Ease.OutBack).OnComplete(() =>
        {
            newScoreTextRect.DOScale(0, 1).SetEase(Ease.InBack).OnComplete(() =>
            {
                _newScoreText.gameObject.SetActive(false);
                foreach (StarAnim star in _stars)
                {
                    star.gameObject.SetActive(false);
                }
                _finishPanel.SetActive(true);
                _finishPanel.transform.localScale = Vector2.zero;
                _finishPanel.transform.DOScale(1, 0.5f).SetEase(Ease.OutBack).OnComplete(() =>
                {
                    StartCoroutine(SetupStars());
                    SetupThresholdsDescending();
                });
            });
        });
    }
    
    private IEnumerator SetupStars()
    {
        yield return new WaitForSeconds(.25f);
        for (int i = 0; i < _thresholds.Count; i++)
        {
            if ((int)_endLevelTimers.TimeInSeconds <= _thresholds[i])
            {
                _stars[i].gameObject.SetActive(true);
                if (!_stars[i].isActiveAndEnabled) break;

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

    private void OnPlayerRestart()
    {
        DOTween.KillAll();

        // Timer Text & Darkening
        RectTransform timerTextRect = _timerText.GetComponent<RectTransform>();
        timerTextRect.DOScale(1, 1).SetEase(Ease.Linear);
        _darkeningImage.DOColor(new Color32(0, 0, 0, 0), 1).SetEase(Ease.Linear);

        StopCoroutine(SetupStars());
        PlayerAnimator playerAnimator = _player.GetComponentInChildren<PlayerAnimator>();
        BoxCollider2D boxCollider = _player.GetComponent<BoxCollider2D>();
        input.enabled = true;
        _pawnController.ChangeMoveClamp(_previousMoveClamp);
        boxCollider.enabled = false;
        _finish.IsFinished = false;
        _spawnManager.ChangeSpawnPos(_spawnManager.StartPos, null);
        _player.DOMove(_spawnManager.StartPos.position, 0).OnComplete(() =>
        {
            boxCollider.enabled = true;
            _timerSinglePlayer?.ChangeRunStartedBool(false);
            _timerText.text = "00:00";
            playerAnimator?.ClearTrail();
        });
    }

    public static void RestartPlayer()
    {
        PlayerRestart?.Invoke();
    }
}
