using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LeaderboardsPageChanger : MonoBehaviour
{
    [SerializeField] private LeaderboardsPresenter _leaderboardsPresenter;
    [SerializeField] private Button _nextPage;
    [SerializeField] private Button _previousPage;
    [SerializeField] private TMP_Text _currentPageText;
    [SerializeField] private int _totalNumberOfScores;
    [SerializeField] private int _currentPage;
    [SerializeField] private int _totalNumberOfPages;
    [SerializeField] private string _levelName;
    private const int SCORES_PER_PAGE = 8;
    
    private void OnEnable()
    {
        if (LeaderboardsManagerClient.Instance.Scores.TryGetValue(_levelName, out LootLockerResponseData responseData))
        {
            _levelName = SceneManager.GetActiveScene().name;
            _nextPage.onClick.AddListener(LoadNextPage);
            _previousPage.onClick.AddListener(LoadPreviousPage);
            _currentPage = 0;
            _totalNumberOfScores = LeaderboardsManagerClient.Instance.Scores[_levelName].Entries.Count;
            _totalNumberOfPages = Mathf.CeilToInt((float)_totalNumberOfScores / SCORES_PER_PAGE);
            ChangePageText();
            ValidateButtons();
        }
    }

    private void ChangePageText()
    {
        if (_currentPageText != null)
        {
            _currentPageText.text = $"{_currentPage + 1}/{_totalNumberOfPages}";
        }
    }

    private void LoadNextPage()
    {
        if (_currentPage >= _totalNumberOfPages - 1) return;
        _currentPage++;
        ChangePageText();
        ValidateButtons();
        _leaderboardsPresenter.LoadTopScoresByLevelName(_levelName, 0, SCORES_PER_PAGE, _currentPage * SCORES_PER_PAGE);
    }

    private void LoadPreviousPage()
    {
        if (_currentPage == 0) return;
        _currentPage--;
        ChangePageText();
        ValidateButtons();
        _leaderboardsPresenter.LoadTopScoresByLevelName(_levelName, 0, SCORES_PER_PAGE, _currentPage * SCORES_PER_PAGE);
    }

    private void ValidateButtons()
    {
        _nextPage.enabled = _currentPage < _totalNumberOfPages - 1;
        _previousPage.enabled = _currentPage != 0;
    }
}
