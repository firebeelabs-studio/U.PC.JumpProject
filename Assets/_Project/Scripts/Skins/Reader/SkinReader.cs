using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkinReader : MonoBehaviour
{
    [SerializeField] private List<SwampieSkin> _skins;
    [SerializeField] private List<SwampieSkin> _sortedSkins;
    public List<SwampieSkin> SortedSkins => _sortedSkins;
    [SerializeField] private SwampieSkin.SkinType _skinType;
    [SerializeField] private SwampieSkin.SwampieType _swampieType;
    [SerializeField] private bool _optional = false;
    private SwampieSkin _currentSkin;
    private int _currentSkinIndex = 0;
    private SpriteRenderer _skinSpriteRenderer;
    private List<SwampieSkin.SkinTransform> _positions;

    private void Awake()
    {
        _skinSpriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        LoadSkins();
        ChangeSkin(_optional ? 1 : 0);
    }

    //read skins
    private void LoadSkins()
    {
        _sortedSkins = _skins.Where(x => x.swampieType == _swampieType && x.skinType == _skinType).Reverse().ToList();
    }

    public List<SwampieSkin> LoadAllSkins()
    {
        return _skins.Where(x => x.swampieType == _swampieType).Reverse().ToList();
    }
    //set skins
    public void ChangeSkin(int index)
    {
        _currentSkin = _sortedSkins[index];
        _skinSpriteRenderer.sprite = _currentSkin.SkinSprite;
        _positions = _currentSkin.Positions;
        SetSkinPos(0);
        //_currentSkinIndex = ArcnesTools.IndexHelper.LoopIndex(addToIndex, _currentSkinIndex, _sortedSkins);
    }

    public void ChangeVariant(int variantNumber)
    {
        SetSkinPos(variantNumber);
    }

    public void ChangeSwampieType(SwampieSkin.SwampieType type)
    {
        _swampieType = type;
        LoadSkins();
        if (_currentSkinIndex > 0)
        {
            _currentSkinIndex--;
        }
        ChangeSkin(1);
    }

    private void SetSkinPos(int index)
    {
        transform.localPosition = _positions[index].Pos;
        transform.rotation = _positions[index].Rot;
        transform.localScale = _positions[index].Scale;
    }

    public void SaveSkin()
    {
        SkinsHolder.Instance.AddOutfitData(transform, _skinSpriteRenderer.sprite, _currentSkin);
    }
}
