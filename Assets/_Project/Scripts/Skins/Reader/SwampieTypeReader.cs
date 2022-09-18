using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class SwampieTypeReader : MonoBehaviour
{
    [SerializeField] private GameObject _blueSwampie;
    [SerializeField] private GameObject _yellowSwampie;
    [SerializeField] private GameObject _greenSwampie;
    [SerializeField] private GameObject _turquoiseSwampie;
    [SerializeField] private GameObject _purpleSwampie;
    private SwampieSkin.SwampieType _swampieType;
    private void Start()
    {
        if (!SkinsHolder.Instance)
        {
            InstantiateSwampie(_blueSwampie);
            return;
        }
        
        ChangeSprite(SkinsHolder.Instance.Skins);
    }

    private void ChangeSprite(List<OutfitData> instanceSkins)
    {

        if (instanceSkins.Count > 0)
        {
            _swampieType = instanceSkins[0].swampieType;
        }
        
        switch (_swampieType)
        {
            case SwampieSkin.SwampieType.Blue:
                InstantiateSwampie(_blueSwampie);
                break;
            case SwampieSkin.SwampieType.Green:
                InstantiateSwampie(_greenSwampie);
                break;
            case SwampieSkin.SwampieType.Yellow:
                InstantiateSwampie(_yellowSwampie);
                break;
            case SwampieSkin.SwampieType.Turquoise:
                InstantiateSwampie(_turquoiseSwampie);
                break;
            case SwampieSkin.SwampieType.Purple:
                InstantiateSwampie(_purpleSwampie);
                break;
        }
    }

    private void InstantiateSwampie(GameObject swampieToSpawn)
    {
        var obj = Instantiate(swampieToSpawn, transform);
        obj.transform.position = transform.position;
    }
}
