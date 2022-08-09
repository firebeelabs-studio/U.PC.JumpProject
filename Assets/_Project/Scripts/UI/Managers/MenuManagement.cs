using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManagement : MonoBehaviour
{
    [Header("MAIN MENU")]
    [SerializeField] private GameObject _mainMenuPanel;
    [SerializeField] private GameObject _questsButton;
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
    private enum Panels
    {
        MAIN_MENU,
        MODE_MENU,
        LEVELS_MENU
    }
    private Panels _currentPanel;
    private void Start()
    {
        _currentPanel = Panels.MAIN_MENU;
        _questsButton.GetComponent<Button>().onClick.AddListener(() => { OpenQuestsPanel(); });
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

    private void OpenQuestsPanel()
    {
        _questsButton.SetActive(false);
        _questsPanel.SetActive(true);
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

        if (_currentPanel == Panels.MAIN_MENU)
        {
            _mainMenuPanel.SetActive(false);
        }
        else if (_currentPanel == Panels.MODE_MENU)
        {
            _modeMenuPanel.SetActive(false);
        }
        else if (_currentPanel == Panels.LEVELS_MENU)
        {
            _levelsMenuPanel.SetActive(false);
        }
    }
}
