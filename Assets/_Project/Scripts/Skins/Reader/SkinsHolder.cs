using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkinsHolder : MonoBehaviour
{
    public static SkinsHolder Instance = null;
    public List<OutfitData> Skins { get; private set; } = new();
    public List<OutfitData> LastUsedSkins { get; private set; } = new();

    private void Awake()
    {
        if (Instance != null && Instance != this) 
        { 
            Destroy(gameObject); 
        } 
        else 
        { 
            Instance = this; 
        } 
        DontDestroyOnLoad(gameObject);
    }

    public void AddOutfitData(Transform skinTransform, Sprite sprite, SwampieSkin swampieSkin)
    {
        OutfitData skinToAdd = new OutfitData
        {
            skinType = swampieSkin.skinType,
            swampieType = swampieSkin.swampieType,
            SkinSprite = sprite,
            Position = new SwampieSkin.SkinTransform
            {
                Scale = skinTransform.localScale,
                Pos = skinTransform.localPosition,
                Rot = skinTransform.rotation
            }
        };
        
        Skins.Add(skinToAdd);
    }

    public void ClearSkins()
    {
        LastUsedSkins = Skins.ToList();
        Skins.Clear();
    }
    
    
}
