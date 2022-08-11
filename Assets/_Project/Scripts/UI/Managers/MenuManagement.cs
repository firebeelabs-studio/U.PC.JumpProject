using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManagement : MonoBehaviour
{
    [Header("MAIN MENU")]
    [SerializeField] private GameObject _mainMenuPanel;
    [SerializeField] private GameObject _switchQuests;
    [SerializeField] private TMP_Text _switchQuestsText;
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
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _mainMenuPanel.SetActive(true);
            _pawn.SetActive(true);
            _modeMenuPanel.SetActive(false);
            _levelsMenuPanel.SetActive(false);
        }
    }
    public void SwitchBetweenPanels(GameObject openPanel)
    {
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
            _switchQuestsText.SetText("suggested challenge");
            _DOTweenAnimations.SwitchPanels(_suggestedQuestPanel, _dailyQuestsPanel);
        }
        else
        {
            _switchQuestsText.SetText("daily quests");
            _DOTweenAnimations.SwitchPanels(_dailyQuestsPanel, _suggestedQuestPanel);
        }
    }

    //temp
    public void LoadScene(string sceneName)
    {
        DOTween.KillAll();
        SceneManager.LoadScene(sceneName);
    }
}
