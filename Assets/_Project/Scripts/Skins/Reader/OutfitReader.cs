using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class OutfitReader : MonoBehaviour
{
    [SerializeField] private SwampieSkin.SkinType _skinType;
    [SerializeField] private SwampieSkin _defaultSkin;
    
    //set only in menu
    [SerializeField] private bool _isInMenu;
    [SerializeField] private bool _createBones;
    [SerializeField] private CreateSpriteBonesFromSprite _boneCreator;
    
    private SpriteRenderer _spriteRenderer;
    private Transform _transform;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _transform = transform;
    }

    private void Start()
    {
        if (SkinsHolder.Instance == null)
        {
            _spriteRenderer.sprite = _defaultSkin.SkinSprite;
            return;
        }
        
        List<OutfitData> skinList = SkinsHolder.Instance.Skins.Count != 0 ? SkinsHolder.Instance.Skins : SkinsHolder.Instance.LastUsedSkins;

        //if in menu
        if (skinList.Count == 0 && _isInMenu)
        {
            _spriteRenderer.sprite = _defaultSkin.SkinSprite;
            
            if (_createBones)
            {
                _boneCreator.CreateBones();
                _boneCreator.CreateBones();
            }
            
            return;
        }
        if (_isInMenu && _createBones)
        {
            _boneCreator.CreateBones();
            _boneCreator.CreateBones();
            StartCoroutine(SetBones(0.1f));
            StartCoroutine(SetBones(0.2f));
        }

        //if not in menu
        if (skinList.Count == 0)
        {
            _spriteRenderer.sprite = _defaultSkin.SkinSprite;
        }
        foreach (var skin in skinList)
        {
            if (skin.skinType == _skinType)
            {
                _spriteRenderer.sprite = skin.SkinSprite;
            }
        }
    }

    IEnumerator SetBones(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        _boneCreator.CreateBones();
    }
}
