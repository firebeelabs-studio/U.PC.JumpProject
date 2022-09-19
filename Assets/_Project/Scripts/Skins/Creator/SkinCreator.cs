using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;

public class SkinCreator : MonoBehaviour
{
    private SwampieSkin _skinToLoad;
    private SwampieSkin _skin;
    
    
    [SerializeField] private SwampieSkin.SwampieType _swampieType;
    public SwampieSkin.SkinType SkinType;

    [SerializeField] private Transform _skinTransform;
    [SerializeField] private Transform _hatTransform;
    [SerializeField] private Transform _jacketTransform;
    [SerializeField] private Transform _eyeTransform;
    [SerializeField] private Transform _mouthTransform;
    
    private int _currentIndex = 0;
    public Sprite SkinSprite;
    private List<SwampieSkin.SkinTransform> _skinTransforms = new List<SwampieSkin.SkinTransform>();

    public void SetSkinSprite()
    {
        _skinTransform.GetComponent<SpriteRenderer>().sprite = SkinSprite;
    }

    #if UNITY_EDITOR
    [ContextMenu("Create Skin")]
    public void CreateSkin()
    {
        var data = SwampieSkin.CreateInstance<SwampieSkin>();
        data.skinType = SkinType;
        data.swampieType = _swampieType;
        data.Positions = _skinTransforms.ToList();
        data.SkinSprite = SkinSprite;
        string path = $"Assets/_Project/Art/Characters/Skins/Data/{SkinSprite.name}_{_swampieType.ToString()}_{SkinType.ToString()}.asset";
        AssetDatabase.CreateAsset(data, path);
    }
    #endif
    public void AddVariant()
    {
        _skinTransforms.Add(CreateSkinTransform());
        //if we are adding new Variant set current index to last one in list
        _currentIndex = _skinTransforms.Count - 1;
    }

    public void ResetPositions()
    {
        _skinTransforms = new();
    }
    public void ChangeVariant(int number)
    {
        _currentIndex = ArcnesTools.IndexHelper.LoopIndex(number, _currentIndex, _skinTransforms);
        _skinTransform.position = _skinTransforms[_currentIndex].Pos;
        _skinTransform.rotation = _skinTransforms[_currentIndex].Rot;
        _skinTransform.localScale = _skinTransforms[_currentIndex].Scale;
    }
    
    private SwampieSkin.SkinTransform CreateSkinTransform()
    {
        return new SwampieSkin.SkinTransform
        {
            Scale = _skinTransform.localScale,
            Rot = _skinTransform.rotation,
            Pos = _skinTransform.position
        };
    }

    public void SetDefPos()
    {
        switch (SkinType)
        {
            case SwampieSkin.SkinType.Hat:
                ChangePos(_hatTransform);
                break;
            case SwampieSkin.SkinType.Jacket:
                ChangePos(_jacketTransform);
                break;
            case SwampieSkin.SkinType.Eyes:
                ChangePos(_jacketTransform);
                break;
            case SwampieSkin.SkinType.Mouth:
                ChangePos(_jacketTransform);
                break;
        }
    }

    private void ChangePos(Transform skinTransform)
    {
        _skinTransform.position = skinTransform.position;
        _skinTransform.rotation = skinTransform.rotation;
        _skinTransform.localScale = skinTransform.localScale;
    }
    public void ChangeSkinType()
    {
        SkinType = SwampieSkin.SkinType.Jacket;
        SetDefPos();
    }
}
