using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class MenuManagement : MonoBehaviour
{
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

    private ButtonsAnimations _DOTweenAnimations;

    private void Awake()
    {
        _DOTweenAnimations = GetComponent<ButtonsAnimations>();
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

        openPanel.SetActive(true);

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

    public void LoadScene(string sceneName)
    {
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
    }
}
