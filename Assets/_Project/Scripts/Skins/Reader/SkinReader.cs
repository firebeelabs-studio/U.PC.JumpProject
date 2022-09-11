using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkinReader : MonoBehaviour
{
    [SerializeField] private List<SwampieSkin> _skins;
    [SerializeField] private List<SwampieSkin> _sortedSkins;
    [SerializeField] private SwampieSkin.SkinType _skinType;
    [SerializeField] private SwampieSkin.SwampieType _swampieType;
    private int _currentSkinIndex = 0;
    private int _currentVariantIndex = 0;
    private SpriteRenderer _skinSpriteRenderer;
    private List<SwampieSkin.SkinTransform> _positions;

    private void Awake()
    {
        _skinSpriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        LoadSkins();
        ChangeSkin(1);
    }

    //read skins
    private void LoadSkins()
    {
        _sortedSkins = _skins.Where(x => x.swampieType == _swampieType && x.skinType == _skinType).ToList();
    }
    //set skins
    public void ChangeSkin(int addToIndex)
    {
        var skinData = _sortedSkins[_currentSkinIndex];
        _skinSpriteRenderer.sprite = skinData.SkinSprite;
        _positions = skinData.Positions;
        SetSkinPos(0);
        ChangeIndex(addToIndex);
    }

    public void ChangeVariant(int variantNumber)
    {
        SetSkinPos(variantNumber);
    }

    private void SetSkinPos(int index)
    {
        transform.position = _positions[index].Pos;
        transform.rotation = _positions[index].Rot;
        transform.localScale = _positions[index].Scale;
    }
    private void ChangeIndex(int number)
    {
        if (number > 0)
        {
            if (_currentSkinIndex + number > _skins.Count - 1)
            {
                _currentSkinIndex = 0;
            }
            else
            {
                _currentSkinIndex += number;
            }
        }
        else if (number < 0)
        {
            if (_currentSkinIndex + number < 0)
            {
                _currentSkinIndex = _skins.Count - 1;
            }
            else
            {
                _currentSkinIndex += number;
            }
        }
    }
}
