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

    private void Awake()
    {
        _activeCharacter = _swampies[0];
        CurrentCreator = _swampies[0].GetComponent<SkinCreator>();
        CurrentCreator.SkinSprite = _skinSprites[0];
    }

    public void ChangeCharacter(int index)
    {
        ChangeIndex(index);
        _activeCharacter.SetActive(false);
        GameObject newActiveCharacter = _swampies[_currentIndex];
        CurrentCreator = newActiveCharacter.GetComponent<SkinCreator>();
        CurrentCreator.SkinSprite = _skinSprites[0];
        newActiveCharacter.SetActive(true);
        _activeCharacter = newActiveCharacter;
        if (_currentIndex == _swampies.Count - 1)
        {
            _skinSprites.RemoveAt(0);
        }
    }
    
    private void ChangeIndex(int number)
    {
        if (number > 0)
        {
            if (_currentIndex + number > _swampies.Count - 1)
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
                _currentIndex = _swampies.Count - 1;
            }
            else
            {
                _currentIndex += number;
            }
        }
    }

    public void Save()
    {
        CurrentCreator.CreateSkin();
    }

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
