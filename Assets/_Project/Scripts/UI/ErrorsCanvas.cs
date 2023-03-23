using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class ErrorsCanvas : MonoBehaviour
{
    public static ErrorsCanvas Instance { get; private set; }
    [SerializeField] private GameObject _errorPanel;
    [SerializeField] private TMP_Text _errorText;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        
        DontDestroyOnLoad(gameObject);
    }

    public void DisplayError(string message)
    {
        StartCoroutine(DisplayErrorCoroutine(message));
    }
    private IEnumerator DisplayErrorCoroutine(string message)
    {
        yield return new WaitForSeconds(2f);
        _errorPanel.transform.localScale = Vector3.zero;
        _errorText.text = message;
        _errorPanel.SetActive(true);
        _errorPanel.transform.DOScale(1, 0.25f).SetEase(Ease.InCirc);
        yield return new WaitForSeconds(3f);
        _errorPanel.transform.DOScale(0, 0.25f).SetEase(Ease.OutCirc).OnComplete(()=> _errorPanel.SetActive(false));
        
    }
}
