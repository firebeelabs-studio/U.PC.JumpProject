using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TheraBytes.BetterUi;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CharacterCustomizationManagement : MonoBehaviour
{
    private List<SwampieSkin> _sortedSkins = new();
    private List<List<SwampieSkin>> _allSortedSkins = new();
    
    private int _numberOfPages;
    [SerializeField] private Button _buttonAll;
    [SerializeField] private Button _buttonHats;
    [SerializeField] private Button _buttonColor;
    [SerializeField] private Button _buttonBody;
    [SerializeField] private Button _buttonEyes;
    [SerializeField] private Button _buttonMouth;
    [SerializeField] private Button _buttonClothes;
    [SerializeField] private Button _buttonAccessories;
    [SerializeField] private Button _buttonArrowRight;
    [SerializeField] private Button _buttonArrowLeft;

    [SerializeField] private SkinReader _hat;
    [SerializeField] private SkinReader _jacket;
    [SerializeField] private SwampieTypeChanger _body;

    [SerializeField] private int _pageSize = 10;
    private int _lastPage = 0;
    private int _currentPage = 0;

    [SerializeField] private GameObject _gridHolder;
    [SerializeField] private GameObject _template;
    [SerializeField] private GameObject _templateOff;
    private List<GameObject> _gridCells = new();
    private bool _allSkinsLoaded = false;

    private void Start()
    {
        _buttonAll.onClick.AddListener((() =>
        {
            _currentPage = 0;
            _sortedSkins = _hat.LoadAllSkins();
            ClearGrid();
            InitializeGridWithAllSkins(_currentPage);
            ResetArrows();
        }));
        
        _buttonHats.onClick.AddListener((() =>
        {
            _currentPage = 0;
            _sortedSkins = _hat.SortedSkins;
            ClearGrid();
            InitializeGrid2(_currentPage);
            ResetArrows();
        }));
        
        _buttonClothes.onClick.AddListener((() =>
        {
            _currentPage = 0;
            _sortedSkins = _jacket.SortedSkins;
            ClearGrid();
            InitializeGrid2(_currentPage);
            ResetArrows();
        }));
        
        _buttonArrowRight.onClick.AddListener((() =>
        {
            _buttonArrowLeft.interactable = true;
            ClearGrid();
            if (_currentPage < _lastPage)
            {
                _currentPage++;
                ChangePage(_currentPage);
            }
            
            if(_currentPage >= _lastPage)
            {
                _buttonArrowRight.interactable = false;
            }
        }));
        _buttonArrowLeft.onClick.AddListener((() =>
        {
            _buttonArrowRight.interactable = true;
            ClearGrid();
            if (_currentPage > 0)
            {
                _currentPage--;
                ChangePage(_currentPage);
            }

            if (_currentPage <= 0)
            {
                _buttonArrowLeft.interactable = false;
            }
        }));
    }

    private void ResetArrows()
    {
        _buttonArrowLeft.interactable = false;
        if (_lastPage == 0)
        {
            _buttonArrowRight.interactable = false;
        }
        else
        {
            _buttonArrowRight.interactable = true;
        }
    }

    private void ChangePage(int newPage)
    {
        if (_allSkinsLoaded)
        {
            InitializeGridWithAllSkins(newPage);
        }
        else
        {
            InitializeGrid2(newPage);
        }
        
        //TODO: change page number on UI
    }
    
    private void InitializeGrid2(int currentPage)
    {
        //create cells with skins
        int startingIndex = 10 * currentPage;
        int createdCells = 0;
        for (int i = startingIndex; i < _pageSize + startingIndex && i < _sortedSkins.Count; i++)
        {
            GameObject newGridCell = Instantiate(_template, _gridHolder.transform);
            SkinGridTemplate references = newGridCell.GetComponent<SkinGridTemplate>();
            references.SkinImage.enabled = true;
            references.SkinImage.sprite = _sortedSkins[i].SkinSprite;
            references.Id = i;
            references.SkinType = _sortedSkins[i].skinType;
            newGridCell.GetComponent<Button>().onClick.AddListener((() =>
            {
                ChangeSkin(references.SkinType, references.Id);
            }));
            _gridCells.Add(newGridCell);
            createdCells++;
        }
        _allSkinsLoaded = false;
        double maxPage = _sortedSkins.Count / 10.0;
        _lastPage = Convert.ToInt32(Math.Ceiling(maxPage) - 1);
        
        //create missing cells
        for (int i = 0; i < 10 - createdCells; i++)
        {
            GameObject newGridCell = Instantiate(_templateOff, _gridHolder.transform);
            _gridCells.Add(newGridCell);
        }
    }
    private void InitializeGridWithAllSkins(int currentPage)
    {
        _allSortedSkins.Clear();
        _allSortedSkins.Add(_jacket.SortedSkins);
        _allSortedSkins.Add(_hat.SortedSkins);
        //create cells with skins
        int startingIndex = 10 * currentPage;

        foreach (var sortedSkins in _allSortedSkins)
        {
            for (int i = 0; i < sortedSkins.Count; i++)
            {
                GameObject newGridCell = Instantiate(_template, _gridHolder.transform);
                SkinGridTemplate references = newGridCell.GetComponent<SkinGridTemplate>();
                references.SkinImage.enabled = true;
                references.SkinImage.sprite = sortedSkins[i].SkinSprite;
                references.Id = i;
                references.SkinType = sortedSkins[i].skinType;
                newGridCell.GetComponent<Button>().onClick.AddListener((() =>
                {
                    ChangeSkin(references.SkinType, references.Id);
                }));
                _gridCells.Add(newGridCell);
            }
        }

        foreach (var gridCell in _gridCells)
        {
            gridCell.SetActive(false);
        }

        int activeCells = 0;
        for (int i = startingIndex; i < _pageSize + startingIndex && i < _gridCells.Count; i++)
        {
            _gridCells[i].SetActive(true);
            activeCells++;
        }
        
        //Create missing tiles
        if (activeCells < 10)
        {
            for (int i = 0; i < 10 - activeCells; i++)
            {
                GameObject newGridCell = Instantiate(_templateOff, _gridHolder.transform);
                _gridCells.Add(newGridCell);
            }
        }
        

        _allSkinsLoaded = true;
        double maxPage = _gridCells.Count / 10.0;
        _lastPage = Convert.ToInt32(Math.Ceiling(maxPage) - 1);
        //create missing cells
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
        _gridCells.Clear();
    }
    
}
