using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SkinCreator : MonoBehaviour
{
    [SerializeField] private SwampieSkin _skinToLoad;
    private SwampieSkin skin;
    private int _currentIndex = 0;
    public Transform _transform;
    public string _name;
    public Sprite _sprite;
    List<SwampieSkin.SkinTransform> _skinTransforms = new List<SwampieSkin.SkinTransform>();

    [ContextMenu("Create Skin")]
    public void CreateSkin()
    {
        var data = SwampieSkin.CreateInstance<SwampieSkin>();
        data.skinType = SwampieSkin.SkinType.Hat;
        data.Positions = _skinTransforms;
        data.SkinSprite = _sprite;
        string path = $"Assets/_Project/Art/Characters/Skins/{SwampieSkin.SkinType.Jacket.ToString()}/{_name}.asset";
        AssetDatabase.CreateAsset(data, path);
        skin = data;
    }

    public void AddVariant()
    {
        _skinTransforms.Add(CreateSkinTransform());
        //if we are adding new Variant set current index to last one in list
        _currentIndex = _skinTransforms.Count - 1;
    }
    
    public void ChangeVariant(int number)
    {
        ChangeIndex(number);
        transform.position = _skinTransforms[_currentIndex].Pos;
        transform.rotation = _skinTransforms[_currentIndex].Rot;
        transform.localScale = _skinTransforms[_currentIndex].Scale;
    }

    [ContextMenu("ReadSkin")]
    public void ReadSkin()
    {
        skin = _skinToLoad;
        _currentIndex = 0;
        _skinTransforms = skin.Positions;
        transform.position = _skinTransforms[0].Pos;
        transform.rotation = _skinTransforms[0].Rot;
        transform.localScale = _skinTransforms[0].Scale;
    }

    private SwampieSkin.SkinTransform CreateSkinTransform()
    {
        return new SwampieSkin.SkinTransform
        {
            Scale = _transform.localScale,
            Rot = _transform.rotation,
            Pos = _transform.position
        };
    }

    private void ChangeIndex(int number)
    {
        if (number > 0)
        {
            if (_currentIndex + number > _skinTransforms.Count - 1)
            {
                _currentIndex = 0;
            }
            else
            {
                _currentIndex += number;
            }
        }
        else if (number < 0)
        {
            if (_currentIndex + number < 0)
            {
                _currentIndex = _skinTransforms.Count - 1;
            }
            else
            {
                _currentIndex += number;
            }
        }
    }
}
