using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuManagement : MonoBehaviour
{
    [Header("STARTING CROSSFADE")]
    [SerializeField] private Image _startingCrossfadeImg;
    [SerializeField] private Color32 _startingCrossfadeColor;
    [Space(10   )]
    [Header("MAIN MENU")]
    [SerializeField] private GameObject _mainMenuPanel;
    [SerializeField] private Button _enableSettingsButton;
    [SerializeField] private Button _disableSettingsButton;
    [SerializeField] private Button _battlepassButton;
    [SerializeField] private GameObject _settingsPanel;
    [SerializeField] private GameObject _settingsBgPanel;
    [SerializeField] private Button _dailyQuestsButton;
    [SerializeField] private Sprite[] _dailyQuestsBarsWithArrows = new Sprite[2];
    [SerializeField] private GameObject _dailyQuestsScrollView;
    [SerializeField] private GameObject _playButton;
    [SerializeField] public GameObject _pawn;
    [SerializeField] private Button _secretSceneButton;
    [SerializeField] private Button _logoutButton;
    [Space(10)]
    [Header("MODE MENU")]
    [SerializeField] private GameObject _modeMenuPanel;
    [SerializeField] private Button _singleButton;
    [SerializeField] private Button _multiButton;
    [Space(10)] 
    [Header("LEVELS MENU")]
    [SerializeField] private GameObject _levelsMenuPanel;
    [SerializeField] private GameObject _levelPanel;
    [SerializeField] private TMP_Text _levelNameText;
    [SerializeField] private Image _levelBg;
    [SerializeField] private GameObject _closeLevelPanelButton;
    [SerializeField] private LevelsInfoHolder _levelsInfoHolder;
    [SerializeField] private Button _leftArrow;
    [SerializeField] private Button _rightArrow;
    [SerializeField] private LeaderboardsPresenter _leaderboardsPresenter;
    private int _currentLevelIndex = 0;
    [Space(10)]
    [Header("SWAMPIE COMPETITION LEVELS")] 
    [SerializeField] private GameObject _swampieCompetitionLevels;
    [Space(10)]
    [Header("SETTINGS MENU")]
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private TMP_Dropdown _dropdown;


    private ButtonsAnimations _DOTweenAnimations;
    private GameObject _currentPanel;
    private string _levelNameToLoad;

    private void Awake()
    {
        _DOTweenAnimations = GetComponent<ButtonsAnimations>();
    }

    private void OnEnable()
    {
        _currentPanel = _mainMenuPanel;
        _startingCrossfadeImg.enabled = true;
        _startingCrossfadeImg.color = _startingCrossfadeColor;
        SpriteRenderer[] pawnSprites = _pawn.GetComponentsInChildren<SpriteRenderer>();
        if (pawnSprites.Length > 0)
        {
            foreach (SpriteRenderer pawnSprite in pawnSprites)
            {
                var pawnColor = pawnSprite.color;
                pawnSprite.color = new Color(pawnColor.r, pawnColor.g, pawnColor.b, 0);
                pawnSprite.DOColor(new Color(pawnColor.r, pawnColor.g, pawnColor.b, 1f), 1.5f);
            }
        }
        _startingCrossfadeImg.DOColor(new Color32(_startingCrossfadeColor.r, _startingCrossfadeColor.g, _startingCrossfadeColor.b, 0), 1.5f).OnComplete(() =>
        {
            _startingCrossfadeImg.enabled = false;
            
        });
    }

    private void Start()
    {
        _enableSettingsButton.onClick.AddListener(() =>
        {
            if (_settingsPanel.activeInHierarchy)
            {
                ClosePanel(_settingsPanel, _settingsBgPanel);
            }
            else
            {
                OpenPanel(_settingsPanel, _settingsBgPanel);
            }
        });
        _disableSettingsButton.onClick.AddListener(() => ClosePanel(_settingsPanel, _settingsBgPanel));
        _playButton.GetComponent<Button>().onClick.AddListener(() => SwitchBetweenPanels(_modeMenuPanel));
        _singleButton.onClick.AddListener(() => SwitchBetweenPanels(_levelsMenuPanel));
        _multiButton.onClick.AddListener(() => SwitchBetweenPanels(_swampieCompetitionLevels));
        _dailyQuestsButton.onClick.AddListener(() =>
        {
            if (_dailyQuestsScrollView.activeInHierarchy)
            { 
                _dailyQuestsScrollView.SetActive(false);
                _dailyQuestsButton.GetComponent<Image>().sprite = _dailyQuestsBarsWithArrows[0];
            }
            else
            {
                _dailyQuestsScrollView.SetActive(true);
                _dailyQuestsButton.GetComponent<Image>().sprite = _dailyQuestsBarsWithArrows[1];
            }
        });
        _secretSceneButton.onClick.AddListener((() => LoadSceneWithName("SecretScene")));
        _battlepassButton.onClick.AddListener((() => LoadSceneWithName("Battlepass")));
        _logoutButton.onClick.AddListener(()=> LoginManager.Instance.Logout());
    }

    public void SwitchBetweenPanels(GameObject openPanel)
    {
        DOTween.KillAll();
        _currentPanel.SetActive(false);
        openPanel.SetActive(true);
        _currentPanel = openPanel;
        if (openPanel == _mainMenuPanel)
        {
            _pawn.SetActive(true);
        }
        else
        {
            _pawn.SetActive(false);
        }
    }

    public void LoadScene()
    {
        if (string.IsNullOrEmpty(_levelNameToLoad)) return;

        DOTween.KillAll();
        LoadingScreenCanvas.Instance?.LoadScene(_levelNameToLoad);
    }
    public void LoadSceneWithName(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName)) return;

        DOTween.KillAll();
        LoadingScreenCanvas.Instance?.LoadScene(sceneName);
    }

    public void BackToMenu(RectTransform button)
    {
        DOTween.KillAll();
        button.localScale = Vector3.one;
        _mainMenuPanel.SetActive(true);
        _pawn.SetActive(true);
        _modeMenuPanel.SetActive(false);
        _levelsMenuPanel.SetActive(false);
        _currentPanel = _mainMenuPanel;
        _levelPanel.SetActive(false);
    }

    public void OpenLevelPanel(int levelIndex)
    {
        _currentLevelIndex = levelIndex;
        LoadLevelData(_currentLevelIndex);
        OpenPanel(_levelPanel);
    }
    public void ChangeLevelPanel(int incrementIndex)
    {
        if (_currentLevelIndex + incrementIndex >= _levelsInfoHolder.LevelsInfo.Count || _currentLevelIndex + incrementIndex < 0) return;
            
        if (!_levelsInfoHolder.LevelsInfo[_currentLevelIndex + incrementIndex].IsAvailable) return;
        
        _currentLevelIndex += incrementIndex;
        
        LoadLevelData(_currentLevelIndex);
    }

    private void LoadLevelData(int levelIndex)
    {
        if (levelIndex == 0)
        {
            _leftArrow.interactable = false;
        }
        else
        {
            _leftArrow.interactable = true;
        }

        if (levelIndex == (_levelsInfoHolder.LevelsInfo.Count - 1))
        {
            _rightArrow.interactable = false;
        }
        else
        {
            _rightArrow.interactable = true;
        }

        _levelNameText.text = _levelsInfoHolder.LevelsInfo[levelIndex].LevelName;
        //load scores
        //load stars info
        _levelBg.sprite = _levelsInfoHolder.LevelsInfo[levelIndex].LevelImage;
        SetLevelToLoad(_levelsInfoHolder.LevelsInfo[levelIndex].SceneName);
    }
    public void CloseLevelPanel()
    {
        ClosePanel(_levelPanel);
    }
    private void SetLevelToLoad(string levelName)
    {
        _levelNameToLoad = levelName;
        _leaderboardsPresenter.LoadTopScoresByLevelName(levelName);
    }
    private void OpenPanel(GameObject panel, GameObject anotherPanel = null)
    {
        if (anotherPanel != null)
        {
            anotherPanel.SetActive(true);
        }
        panel.transform.localScale = Vector2.zero;
        panel.SetActive(true);
        panel.transform.DOScale(1, 0.5f).SetEase(Ease.InOutCubic);
    }
    private void ClosePanel(GameObject panel, GameObject anotherPanel = null)
    {
        if (anotherPanel != null)
        {
            anotherPanel.SetActive(false);
        }
        panel.transform.DOScale(0, 0.5f).SetEase(Ease.InOutCubic).OnComplete(() => { panel.SetActive(false); });
    }
}