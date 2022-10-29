using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class OutfitReader : MonoBehaviour
{
    [SerializeField] private SwampieSkin.SkinType _skinType;
    private SpriteRenderer _spriteRenderer;
    private Transform _transform;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _transform = transform;
    }

    private void Start()
    {
        if (SkinsHolder.Instance == null) return;
        List<OutfitData> skinList = SkinsHolder.Instance.Skins.Count != 0 ? SkinsHolder.Instance.Skins : SkinsHolder.Instance.LastUsedSkins;
        
        foreach (var skin in skinList)
        {
            if (skin.skinType == _skinType)
            {
                _spriteRenderer.sprite = skin.SkinSprite;
            }
        }
    }
}
