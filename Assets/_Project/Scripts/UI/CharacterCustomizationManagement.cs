using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CharacterCustomizationManagement : MonoBehaviour
{
    private int _numberOfPages;
    private List<List<SwampieSkin>> _splittedList = new();
    [SerializeField] private Button _buttonAll;
    [SerializeField] private Button _buttonHats;
    [SerializeField] private Button _buttonColor;
    [SerializeField] private Button _buttonBody;
    [SerializeField] private Button _buttonEyes;
    [SerializeField] private Button _buttonMouth;
    [SerializeField] private Button _buttonClothes;
    [SerializeField] private Button _buttonAccessories;

    [SerializeField] private SkinReader _hat;
    [SerializeField] private SkinReader _jacket;
    [SerializeField] private SwampieTypeChanger _body;

    [SerializeField] private int _pageSize = 10;

    [SerializeField] private GameObject _gridHolder;
    [SerializeField] private GameObject _template;
    private List<GameObject> _gridCells = new();
    
    private void Start()
    {
        //_splittedList = ArcnesTools.ListHelper.SplitList(_hat.LoadAllSkins(), 10);
        //InitializeGrid(0);
        _buttonAll.onClick.AddListener((() =>
        {
            _splittedList.Clear();
            _splittedList = ArcnesTools.ListHelper.SplitList(_hat.LoadAllSkins(), 10);
            ClearGrid();
            InitializeGrid(0);
        }));
        
        _buttonHats.onClick.AddListener((() =>
        {
            _splittedList.Clear();
            _splittedList = ArcnesTools.ListHelper.SplitList(_hat.SortedSkins, 10);
            ClearGrid();
            InitializeGrid(0);
        }));
        
        _buttonClothes.onClick.AddListener((() =>
        {
            _splittedList.Clear();
            _splittedList = ArcnesTools.ListHelper.SplitList(_jacket.SortedSkins, 10);
            ClearGrid();
            InitializeGrid(0);
        }));
        
    }
    
    private void InitializeGrid(int pageNumber=0)
    {
        for (int i = 0; i < _splittedList[pageNumber].Count; i++)
        {
            GameObject newGridCell = Instantiate(_template, _gridHolder.transform);
            SkinGridTemplate references = newGridCell.GetComponent<SkinGridTemplate>();
            references.SkinImage.enabled = true;
            references.SkinImage.sprite = _splittedList[pageNumber][i].SkinSprite;
            references.Id = i;
            references.SkinType = _splittedList[pageNumber][i].skinType;
            newGridCell.GetComponent<Button>().onClick.AddListener((() =>
            {
                ChangeSkin(references.SkinType, references.Id);
            }));
            _gridCells.Add(newGridCell);
        }

        for (int i = 0; i < 10 - _splittedList[pageNumber].Count; i++)
        {
            GameObject newGridCell = Instantiate(_template, _gridHolder.transform);
            newGridCell.GetComponent<Button>().interactable = false;
            _gridCells.Add(newGridCell);
        }
    }

    private void ChangeSkin(SwampieSkin.SkinType skinType, int id)
    {
        switch (skinType)
        {
            case SwampieSkin.SkinType.Hat:
                _hat.ChangeSkin(id);
                break;
            case SwampieSkin.SkinType.Jacket:
                _jacket.ChangeSkin(id);
                break;
        }
    }

    private void ClearGrid()
    {
        foreach (var gridCell in _gridCells)
        {
            Destroy(gridCell);
        }
    }
    
}
