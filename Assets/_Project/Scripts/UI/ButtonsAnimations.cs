using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonsAnimations : MonoBehaviour
{
    [SerializeField] private float _speed = 2f;
    private RectTransform _button;
    public void ButtonOnEnter(RectTransform buttonRect)
    {
        buttonRect.DOScale(new Vector3(1.1f, 1.1f, 1), _speed).SetEase(Ease.OutBack);
    }
    public void ButtonOnExit(RectTransform buttonRect)
    {
        buttonRect.DOScale(new Vector3(1, 1, 1), _speed).SetEase(Ease.OutBack);
    }
}
