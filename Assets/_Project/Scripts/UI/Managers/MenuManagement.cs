using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManagement : MonoBehaviour
{
    [SerializeField] private GameObject _questsButton, _questsPanel, _playButton;

    private void Start()
    {
        _questsButton.GetComponent<Button>().onClick.AddListener(() => { OpenQuestsPanel(); });
        _playButton.GetComponent<Button>().onClick.AddListener(() => { OpenModePanel(); });
    }

    private void OpenQuestsPanel()
    {
        _questsButton.SetActive(false);
        _questsPanel.SetActive(true);
    }
    public void OpenModePanel()
    {

    }

}
