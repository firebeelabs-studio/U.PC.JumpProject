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
    [SerializeField] private string _homeSceneName;
    [SerializeField] private Button _restartButton;
    [SerializeField] private string _previousLevelName;
    [SerializeField] private Button _previousLevelButton;
    [SerializeField] private Button _nextLevelButton;
    [SerializeField] private string _nextLevelName;
    [SerializeField] private Transform _player;
    [SerializeField] private UIParticleSystem _confettiParticles, _pepeParticles;
    [SerializeField] private StarAnim[] _stars;
    
    //TEMP REMOVE THIS AFTER FINISHING LEADERBOARDS
    [SerializeField] private int _bestStarAmount = 3;
    [SerializeField] private StarAnim[] _mainStars;
    [SerializeField] private StarAnim[] _nextStars;
    [SerializeField] private List<float> _thresholds = new();
    private PlayersInput input;
    private IPawnController _pawnController;

    [SerializeField] private Image _progressBar;

    [SerializeField] private SetSummaryPlaces _summaryPlaces;

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Start()
    {
        _backToMenuButton.onClick.AddListener(() => { LoadingScreenCanvas.Instance.LoadScene(_homeSceneName); });
        _restartButton.onClick.AddListener(() =>
        {
            RestartPlayer();
        });
        if (!string.IsNullOrEmpty(_previousLevelName))
        {
            _previousLevelButton.interactable = true;
            _previousLevelButton.onClick.AddListener(() => { LoadingScreenCanvas.Instance.LoadScene(_previousLevelName); });
        }
        else
        {
            _previousLevelButton.interactable = false;
        }
        if (!string.IsNullOrEmpty(_nextLevelName))
        {
            _nextLevelButton.interactable = true;
            _nextLevelButton.onClick.AddListener(() => { LoadingScreenCanvas.Instance.LoadScene(_nextLevelName); });
        }
        else
        {
            _nextLevelButton.interactable = false;
        }
        input = _player.GetComponent<PlayersInput>();
        _pawnController = _player.GetComponent<IPawnController>();
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
        input.enabled = false;
        _pawnController.ChangeMoveClamp(0);

        // Finish Panel Text
        _yourTimeText.text = $"{(int)_endLevelTimers.TimeInSeconds}s";
        int timeInSeconds = (int)_endLevelTimers.TimeInSeconds;
        _summaryPlaces.UpdatePlaces(timeInSeconds);
        if (timeInSeconds <= _thresholds[2])
        {
            _timeNeededForNextStarText.text = $"Gj!";
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
            _timeNeededForNextStarText.text = $"{timeForNextStar}s";
        }

        // Darkening
        _darkeningImage.DOColor(new Color32(0, 0, 0, 100), 1).SetEase(Ease.Linear).OnComplete(() =>
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
        _pepeParticles.StartParticleEmission();
        RectTransform newScoreTextRect = _newScoreText.GetComponent<RectTransform>();
        newScoreTextRect.localScale = Vector2.zero;
        
        //setup main stars
        //setup max stars from previous runs
        SetupMainStars();
        
        newScoreTextRect.DOScale(1.5f, 1.5f).SetEase(Ease.OutBack).OnComplete(() =>
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
                    SetupNextStars();
                    StartCoroutine(FillBar());
                    SetupThresholdsDescending();
                });
            });
        });
    }

    private int CountStars()
    {
        int stars = 0;
        for (int i = 0; i < _thresholds.Count; i++)
        {
            if ((int)_endLevelTimers.TimeInSeconds <= _thresholds[i])
            {
                stars++;
            }
        }

        return stars;
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
                
                bool isScoreBetterThanGlobalBest = i + 1 > _bestStarAmount;
                _stars[i].RunPunchAnimation(isScoreBetterThanGlobalBest);
            }
            yield return new WaitForSeconds(.75f);
        }
    }

    private void SetupStar(int starIndex)
    {
        _stars[starIndex].gameObject.SetActive(true);
        if (!_stars[starIndex].isActiveAndEnabled) return;
                
        bool isScoreBetterThanGlobalBest = starIndex + 1 > _bestStarAmount;
        _stars[starIndex].RunPunchAnimation(isScoreBetterThanGlobalBest);
    }

    private IEnumerator FillBar()
    {
        yield return new WaitForSeconds(.25f);
        for (int i = 0; i <= CountStars(); i++)
        {
            _progressBar.fillAmount = 0;
            if (CountStars() == 0)
            {
                float secondsToBetterTime = _thresholds[0];
                float missingSeconds = (int)_endLevelTimers.TimeInSeconds - secondsToBetterTime;
                DOTween.To(() => _progressBar.fillAmount, x => _progressBar.fillAmount = x, 1 - (missingSeconds/secondsToBetterTime), 1f);
            }
            else if (i == CountStars() && CountStars() != 3)
            {
                float secondsFromCurrentTimeToBetterTime = _thresholds[i - 1] - _thresholds[i];
                float missingSeconds = (int)_endLevelTimers.TimeInSeconds - _thresholds[i];
                DOTween.To(() => _progressBar.fillAmount, x => _progressBar.fillAmount = x, missingSeconds/secondsFromCurrentTimeToBetterTime, 1f);
            }
            else
            {
                DOTween.To(() => _progressBar.fillAmount, x => _progressBar.fillAmount = x, 1, 0.6f).OnComplete(() =>
                {
                    SetupStar(i);
                });
                yield return new WaitForSeconds(.75f);
            }
        }
    }

    private void SetupMainStars()
    {
        //Get stars from last run
        for (int i = 0; i < _bestStarAmount; i++)
        {
            //set active
            _mainStars[i].gameObject.SetActive(true);
        }
    }

    private void SetupNextStars()
    {
        
        if (CountStars() >= 3)
        {
            //change somehow this shit
            for (int i = 0; i < 3; i++)
            {
                _nextStars[i].gameObject.SetActive(true);
            }
        }
        else
        {
            for (int i = 0; i < CountStars() + 1; i++)
            {
                _nextStars[i].gameObject.SetActive(true);
            }
        }
    }

    private void SetupThresholdsDescending()
    {
        _thresholds.Sort();
        _thresholds.Reverse();
    }
    

    public static void RestartPlayer()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
    }
}
