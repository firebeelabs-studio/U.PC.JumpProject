using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwampieChanger : MonoBehaviour
{
    private int _currentIndex = 0;
    [SerializeField] private List<GameObject> _swampies;
    private GameObject _activeCharacter;
    //prob should've cached this
    public SkinCreator CurrentCreator;
    public List<Sprite> _skinSprites;

    private void Start()
    {
        _activeCharacter = _swampies[0];
        CurrentCreator = _swampies[0].GetComponent<SkinCreator>();
        CurrentCreator.SkinSprite = _skinSprites[0];
        CurrentCreator.SetSkinSprite();
    }

    //change swampie type and handle skin sprite change
    public void ChangeCharacter(int index)
    {
        _currentIndex = ArcnesTools.IndexHelper.LoopIndex(index, _currentIndex, _swampies);
        _activeCharacter.SetActive(false);
        GameObject newActiveCharacter = _swampies[_currentIndex];
        CurrentCreator = newActiveCharacter.GetComponent<SkinCreator>();
        CurrentCreator.SkinSprite = _skinSprites[0];
        CurrentCreator.ResetPositions();
        newActiveCharacter.SetActive(true);
        CurrentCreator.SetSkinSprite();
        _activeCharacter = newActiveCharacter;
        if (_currentIndex == _swampies.Count - 1)
        {
            _skinSprites.RemoveAt(0);
        }
    }
    
    #if UNITY_EDITOR
    public void Save()
    {
        CurrentCreator.CreateSkin();
    }
    #endif
    public void AddVariant()
    {
        CurrentCreator.AddVariant();
    }
    
    public void ChangeSkinType(int type)
    {
        switch (type)
        {
            case 0:
                CurrentCreator.SkinType = SwampieSkin.SkinType.Hat;
                break;
            case 1:
                CurrentCreator.SkinType = SwampieSkin.SkinType.Jacket;
                break;
        }

        CurrentCreator.SetDefPos();
    }
}
