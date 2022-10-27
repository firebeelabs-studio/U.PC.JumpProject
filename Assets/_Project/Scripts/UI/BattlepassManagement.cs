using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class BattlepassManagement : MonoBehaviour
{
    [SerializeField] private Button _backToMenuButton;
    [SerializeField] private GameObject _battlepassElementPrefab;
    [SerializeField] private RectTransform _battlepassElementsHolder;
    [SerializeField] private Button _buttonLeft;
    [SerializeField] private Button _buttonRight;
    private List<BattlepassElement> _battlepassElements = new ();
    private float _stepLength = 143;
    private bool _canMove = true;
    private int _maxMoves;
    private int _currentMove = 0;

    private void Start()
    {
        _buttonLeft.interactable = false;
        _buttonLeft.onClick.AddListener((MoveLeft));
        _buttonRight.onClick.AddListener((MoveRight));
        _backToMenuButton.onClick.AddListener(() => LoadingScreenCanvas.Instance.LoadScene("MainMenu"));
        _maxMoves = 3;//_battlepassElements.Count - 9;
        if (_maxMoves < 0)
        {
            _maxMoves = 0;
        }
        if (_maxMoves == 0)
        {
            _buttonRight.interactable = false;
        }
    }

    private void MoveRight()
    {
        if (!_canMove) return;

        if (_currentMove == _maxMoves) return;

        float newPos = _battlepassElementsHolder.anchoredPosition.x - _stepLength;
        _canMove = false;
        _currentMove++;
        if (_currentMove == _maxMoves)
        {
            _buttonRight.interactable = false;
        }
        if (!_buttonLeft.interactable)
        {
            _buttonLeft.interactable = true;
        }
        _battlepassElementsHolder.DOAnchorPosX(newPos,0.25f).SetEase(Ease.InCubic).OnComplete(() => _canMove = true);
    }

    private void MoveLeft()
    {
        if (!_canMove) return;
        
        if (_currentMove == 0) return;
        
        float newPos = _battlepassElementsHolder.anchoredPosition.x + _stepLength;
        _canMove = false;
        _currentMove--;
        if (_currentMove == 0)
        {
            _buttonLeft.interactable = false;
        }
        if (!_buttonRight.interactable)
        {
            _buttonRight.interactable = true;
        }
        _battlepassElementsHolder.DOAnchorPosX(newPos, 0.25f).SetEase(Ease.InCubic).OnComplete(() => _canMove = true);
    }
}
