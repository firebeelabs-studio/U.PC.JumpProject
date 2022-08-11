using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonsAnimations : MonoBehaviour
{
    [SerializeField] private float duration = 2f;
    private RectTransform _button;
    public void ButtonOnEnter(RectTransform buttonRect)
    {
        buttonRect.DOScale(new Vector3(1.1f, 1.1f, 1), duration).SetEase(Ease.OutBack);
    }
    public void ButtonOnExit(RectTransform buttonRect)
    {
        buttonRect.DOScale(new Vector3(1, 1, 1), duration).SetEase(Ease.OutBack);
    }
    public void OpenPanel(GameObject panelToOpen, GameObject panelToClose)
    {
        panelToClose.SetActive(false);
        panelToOpen.SetActive(true);
        panelToOpen.GetComponent<RectTransform>().DOScale(new Vector3(1, 1, 1), duration/3).SetEase(Ease.InOutExpo);
    }
    public void SwitchPanels(GameObject panelToClose, GameObject panelToOpen)
    {
        panelToClose.GetComponent<RectTransform>().DOScale(new Vector3(1, 0, 1), duration/3).SetEase(Ease.InOutExpo).OnStepComplete(() => OpenPanel(panelToOpen, panelToClose));
    }
}
