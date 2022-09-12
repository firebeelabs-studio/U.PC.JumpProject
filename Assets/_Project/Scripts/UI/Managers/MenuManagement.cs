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
    [SerializeField] private GameObject _switchQuests;
    [SerializeField] private GameObject _suggestedQuestPanel;
    [SerializeField] private GameObject _dailyQuestsPanel;
    [SerializeField] private GameObject _questsPanel;
    [SerializeField] private GameObject _playButton;
    [SerializeField] private GameObject _pawn;
    [Space(10)]
    [Header("MODE MENU")]
    [SerializeField] private GameObject _modeMenuPanel;
    [SerializeField] private GameObject _singleButton;
    [SerializeField] private GameObject _multiButton;
    [Space(10)]
    [Header("LEVELS MENU")]
    [SerializeField] private GameObject _levelsMenuPanel;
    [SerializeField] private GameObject _levelPanel;
    [SerializeField] private TMP_Text _levelNameText;
    [SerializeField] private TMP_Text _yourScoreText;
    [SerializeField] private GameObject _closeLevelPanelButton;

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
        _switchQuests.GetComponent<Button>().onClick.AddListener(() => { SwitchBetweenQuests(); });
        _playButton.GetComponent<Button>().onClick.AddListener(() => { SwitchBetweenPanels(_modeMenuPanel); });
        _singleButton.GetComponent<Button>().onClick.AddListener(() => { SwitchBetweenPanels(_levelsMenuPanel); });
        //multi button
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

    private void SwitchBetweenQuests()
    {
        if (_suggestedQuestPanel.activeInHierarchy)
        {
            _DOTweenAnimations.SwitchPanels(_suggestedQuestPanel, _dailyQuestsPanel);
        }
        else
        {
            _DOTweenAnimations.SwitchPanels(_dailyQuestsPanel, _suggestedQuestPanel);
        }
    }

    public void LoadScene()
    {
        if (string.IsNullOrEmpty(_levelNameToLoad)) return;

        DOTween.KillAll();
        LoadingScreenCanvas.Instance?.LoadScene(_levelNameToLoad);
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

    public void OpenLevelPanel(string levelName)
    {
        _levelPanel.transform.localScale = Vector3.zero;
        _levelNameText.text = levelName;
        //TO DO: read score of current player and display here
        //_yourScoreText.text = ;
        _levelPanel.SetActive(true);
        _levelPanel.transform.DOScale(1, 0.5f).SetEase(Ease.InOutCubic);
    }
    public void CloseLevelPanel()
    {
        _levelPanel.transform.DOScale(0, 0.5f).SetEase(Ease.InOutCubic).OnComplete(() => { _levelPanel.SetActive(false); });
    }
    public void SetLevelToLoad(string levelName)
    {
        _levelNameToLoad = levelName;
    }
}