using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LeaderboardsPageChanger : MonoBehaviour
{
    [SerializeField] private LeaderboardsPresenter _leaderboardsPresenter;
    [SerializeField] private Button _nextPage;
    [SerializeField] private Button _previousPage;
    private int _totalNumberOfScores;
    private int _currentPage;
    private int _totalNumberOfPages;
    private string _levelName;
    private const int SCORES_PER_PAGE = 8;
    
    private void OnEnable()
    {
        _levelName = SceneManager.GetActiveScene().name;
        _nextPage.onClick.AddListener(()=>
        {
            if (_currentPage >= _totalNumberOfPages - 1) return;
            _currentPage++;
            _leaderboardsPresenter.LoadTopScoresByLevelName(_levelName, 0, 8, _currentPage * SCORES_PER_PAGE);
        });
        _previousPage.onClick.AddListener(()=>
        {
            if (_currentPage == 0) return;
            _currentPage--;
            _leaderboardsPresenter.LoadTopScoresByLevelName(_levelName, 0, 8, _currentPage * SCORES_PER_PAGE);
        });
        _currentPage = 0;
        _totalNumberOfScores = LeaderboardsManagerClient.Instance.Scores[_levelName].Entries.Count;
        _totalNumberOfPages = Mathf.CeilToInt((float)_totalNumberOfScores / SCORES_PER_PAGE);
    }
}
