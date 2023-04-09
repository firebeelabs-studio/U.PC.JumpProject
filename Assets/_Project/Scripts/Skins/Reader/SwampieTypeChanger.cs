using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SwampieTypeChanger : MonoBehaviour
{
    [SerializeField] private Sprite _blueSwampie;
    [SerializeField] private Sprite _yellowSwampie;
    [SerializeField] private Sprite _greenSwampie;
    [SerializeField] private Sprite _turquoiseSwampie;
    [SerializeField] private Sprite _purpleSwampie;
    [SerializeField] private List<SkinReader> _skinReaders;
    private SwampieSkin.SwampieType _swampieType;
    private SpriteRenderer _renderer;
    private int _swampieIndex = 0;

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        SwampieSkin.SwampieType type;
        if (SkinsHolder.Instance.LastUsedSkins.Count != 0)
        {
            type = SkinsHolder.Instance.LastUsedSkins[0].swampieType;
            switch (type)
            {
                case SwampieSkin.SwampieType.Turquoise:
                    ChangeVisuals(SwampieSkin.SwampieType.Turquoise, _turquoiseSwampie);
                    break;
                case SwampieSkin.SwampieType.Yellow:
                    ChangeVisuals(SwampieSkin.SwampieType.Yellow, _yellowSwampie);
                    break;
                case SwampieSkin.SwampieType.Purple:
                    ChangeVisuals(SwampieSkin.SwampieType.Purple, _purpleSwampie);
                    break;
                case SwampieSkin.SwampieType.Green:
                    ChangeVisuals(SwampieSkin.SwampieType.Green, _greenSwampie);
                    break;
                case SwampieSkin.SwampieType.Blue:
                    ChangeVisuals(SwampieSkin.SwampieType.Blue, _blueSwampie);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            ChangeType();
        }
        else
        {
            ChangeVisuals(SwampieSkin.SwampieType.Blue, _blueSwampie);
        }
    }

    public void ReplaceSwampie(int swampieIndex)
    {
        ChangeSwampie(swampieIndex);
    }

    private void ChangeSwampie(int swampieIndex)
    {
        switch (swampieIndex)
        {
            case 0:
                ChangeVisuals(SwampieSkin.SwampieType.Blue, _blueSwampie);
                break;
            case 1:
                ChangeVisuals(SwampieSkin.SwampieType.Green, _greenSwampie);
                break;
            case 2:
                ChangeVisuals(SwampieSkin.SwampieType.Yellow, _yellowSwampie);
                break;
            case 3:
                ChangeVisuals(SwampieSkin.SwampieType.Turquoise, _turquoiseSwampie);
                break;
            case 4:
                ChangeVisuals(SwampieSkin.SwampieType.Purple, _purpleSwampie);
                break;
        }
        
        ChangeType();
    }

    private void ChangeVisuals(SwampieSkin.SwampieType type, Sprite sprite)
    {
        _swampieType = type;
        _renderer.sprite = sprite;
    }
        
    private void ChangeType()
    {
        foreach (var skin in _skinReaders)
        {
            skin.ChangeSwampieType(_swampieType);
        }
    }
}
