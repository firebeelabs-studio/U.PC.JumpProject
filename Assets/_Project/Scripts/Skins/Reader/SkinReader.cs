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
    [SerializeField] private int _currentSkinIndex = 0;
    private SpriteRenderer _skinSpriteRenderer;
    private List<SwampieSkin.SkinTransform> _positions;
    [SerializeField] private bool _changeSkinAtStart = true;

    private void Awake()
    {
        _skinSpriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        
        if (_changeSkinAtStart || SkinsHolder.Instance == null || SkinsHolder.Instance.Skins.Count == 0)
        {
            LoadSkins();
            ChangeSkin(_optional ? 1 : 0);
        }
        //if in skinreader (this is only one place where we have to get swampie skins via skinreader and not by outfitreader)
        else
        {
            //get current skin from skin holder
            OutfitData skinFromHolder = SkinsHolder.Instance.Skins.Where(x => x.skinType == _skinType).FirstOrDefault();
            //read swampie type from skinsholder
            _swampieType = skinFromHolder.swampieType;
            //load skins with new swampietype
            LoadSkins();
            //find same skins with imageContentsHash because we don't store on skinsholder any other unique data :/
            SwampieSkin skinOnReader = _sortedSkins.Where(x => x.SkinSprite.texture.imageContentsHash == skinFromHolder.SkinSprite.texture.imageContentsHash).FirstOrDefault();
            ChangeSkin(_sortedSkins.IndexOf(skinOnReader));
        }
    }

    //read skins
    private void LoadSkins()
    {
        _sortedSkins = _skins.Where(x => x.swampieType == _swampieType && x.skinType == _skinType).ToList();
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
        _currentSkinIndex = index;
    }

    public void ChangeVariant(int variantNumber)
    {
        SetSkinPos(variantNumber);
    }

    public void ChangeSwampieType(SwampieSkin.SwampieType type)
    {
        _swampieType = type;
        LoadSkins();
        ChangeSkin(_currentSkinIndex);
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
