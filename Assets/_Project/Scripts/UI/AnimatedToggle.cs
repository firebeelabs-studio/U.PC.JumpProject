using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AnimatedToggle : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private bool _isOn = false;
    
    [SerializeField] private RectTransform _toggleIndicator;

    [SerializeField] private Image _backgroundImage;
    [SerializeField] private HoldChildPositions _childholder;

    [SerializeField] private float _holdTogglePosAfterSeconds = 0.4f;
    private float _offX;
    private float _onX;

    [SerializeField] private TMP_Text _on;
    [SerializeField] private TMP_Text _off;

    [SerializeField] private Color _colorOn;
    [SerializeField] private Color _colorOff;
    
    [SerializeField] private float _tweenTime = 0.1f;
    [SerializeField] private bool _getPosOnEnable;

    private AudioPlayer _audioPlayer;
    [SerializeField] private AudioClip _toggleSound;

    public delegate void ValueChanged(bool value);

    public event ValueChanged ToggleValueChanged;

    private void Awake()
    {
        _audioPlayer = GetComponent<AudioPlayer>();
    }

    private void Start()
    {
        _onX = _toggleIndicator.anchoredPosition.x;
        _offX = _backgroundImage.rectTransform.rect.width - _toggleIndicator.rect.width;
    }

    public void SetStartToggle(bool isOn)
    {
        _onX = _toggleIndicator.anchoredPosition.x;
        _offX = _backgroundImage.rectTransform.rect.width - _toggleIndicator.rect.width;
        InitializeToggle(isOn);
    }

    private void Toggle(bool value)
    {
        if (value != _isOn)
        {
            _audioPlayer.PlayOneShotSound(_toggleSound);
            _isOn = value;
            MoveIndicator(_isOn);
            ChangeColors(_isOn);

            if (ToggleValueChanged != null)
            {
                ToggleValueChanged(_isOn);
            }
        }
    }
    private void InitializeToggle(bool value)
    {
        if (value != _isOn)
        {
            _isOn = value;
            MoveIndicator(_isOn);
            ChangeColors(_isOn);

            if (ToggleValueChanged != null)
            {
                ToggleValueChanged(_isOn);
            }
        }
    }

    private void ChangeColors(bool isOn)
    {
        Color colorOn = isOn ?  _colorOff : _colorOn;
        Color colorOff = isOn ? _colorOn : _colorOff;
        _on.color = colorOn;
        _off.color = colorOff;
    }

    private void MoveIndicator(bool isOn)
    {
        _toggleIndicator.DOAnchorPosX(isOn ? _onX : -_offX, _tweenTime);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Toggle(!_isOn);
    }

    private void OnEnable()
    {
        if (_getPosOnEnable)
        {
            _childholder.SetPos();
        }
        StartCoroutine(_childholder.StartHolding(_holdTogglePosAfterSeconds));
    }

}
